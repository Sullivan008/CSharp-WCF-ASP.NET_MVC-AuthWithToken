# C# - WCF - ASP.NET Authentication with Token [Year of Development: 2018 and 2020]

About the application technologies and operation:

### Technologies:
- Programming Language: C#
- FrontEnd Side: ASP.NET MVC with JQuery
- BackEnd Side: .NET Framework 4.7.2.
- Descriptive Language: HTML5
- Style Description Language: CSS (Bootstrap 4.4.1)
- Database: SQL Server (Database First)
- Other used modul: Entity Framework 6.0.0.0

### Installation/ Configuration:

1. Restore necessary Packages on the selected project, run the following command in **PM Console**

    ```
    Update-Package -reinstall
    ```

2. Connect to **MSSQLLocalDb Instance** with **Windows Authentication**

    ```
    (LocalDB)\MSSQLLocalDb
    ```
   
3. **CREATE** necessary **DATABASE** with the following **SCRIPT**

    ```SQL
    CREATE DATABASE AuthenticationExampleDB;
    ```
   
4. **CREATE** necessary **TABLES** with the following **SCRIPT** (The scripts can be found in the following folder: **DB TABLES**)

    ```SQL
    USE AuthenticationExampleDB;

    CREATE TABLE [dbo].[User] (
        [Id]       INT            IDENTITY (1, 1) NOT NULL,
        [Name]     NVARCHAR (50)  NOT NULL,
        [Password] NVARCHAR (250) NOT NULL,
        [Salt]     NVARCHAR (250) NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [Unique_User] UNIQUE NONCLUSTERED ([Name] ASC)
    );

   CREATE TABLE [dbo].[Token] (
        [Id]         	INT            IDENTITY (1, 1) NOT NULL,
        [SecureToken]	NVARCHAR (250) NOT NULL,
        [UserId]     	INT            NOT NULL,
        [CreateDate] 	DATETIME       NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [Unique_Token] UNIQUE NONCLUSTERED ([SecureToken] ASC),
        CONSTRAINT [FK_Token_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
    );
    ```

4. The following **SCRIPT** add a user as follows (**User**: user1, **Password**: pass1, **Salt**: salt1):

    ```SQL
    USE AuthenticationExampleDB;

    INSERT INTO [User] ([Name],[Password],[Salt]) VALUES ('user1','63dc4400772b90496c831e4dc2afa4321a4c371075a21feba23300fb56b7e19c','salt1') 
    ```
   
6. An example JSON file that you can test with for example Postman:

    ```yaml
    {
        "User": "user1",
        "Password": "pass1"
    }
    ```
  
### About the application:

### 1. Authenticate once and store a token

This option seems the most secure. After authenticating, a token is returned to the user. The other web services can be accessed by using the token. Now the credentials are not stored. They are only passed over the network at authentication time.

I decided that for simple authentication, there needs to be an example on the web of a **Basic Token Service**.

In the basic token service, there is a the idea of a single service that provides authentication. That service returns a token if authenticated, an authentication failure otherwise. If authenticated, the front end is responsible for passing the token to any subsequent web services. This could be a header value, a cookie or a url parameter. I am going to use a header value in my project.

Authentication Token Service for WCF Services , we created a project that exposes an **AuthenticationTokenService** and a **TestService**. The user is to first authenticate using the **AuthenticationTokenService**. Authentication provides a token. Calls made to additional services should include the token as a header value.

### 2. IUserCredentialsValidator and Hash Class

Use **IUserCredentialsValidator** and the **Hash** class to check if those credentials match what is in the User table of the database.

### 3. ITokenBuilder and TokenBuilder() Method

We will create a new better random string generator using **RNGCryptoServiceProvider**. Se the **BuildSecureToken()** method below.

### 4. ITokenValidator and TokenIsValid() - TokenIsExpired() Method

Use a database query to check the validity of the token.

### 5. Implement Interceptors in the Server Application.

This is because the above code of attaching a cookie to the Operation Context seems to be a tedious job every time we need to do a service call. We can move these tasks to the background using WCF Interceptors.
 
MSDN: WCF Data Services enables an application to intercept request messages so that you can add custom logic to an operation. You can use this custom logic to validate data in incoming messages. You can also use it to further restrict the scope of a query request, such as to insert a custom authorization policy on a per request basis.

The following are the activities involved in this step.
  - Add Interceptor Behavior inside Server web.config.
  - Create the Interceptor Behavior class. (See in the project)
  - Create the **TokenValidationBehaviorExtension** class. (See in the project)

#### The following code to the web.config file of the service just under <system.serviceModel>

  ```xml
  <system.serviceModel>
    <services>
      <service name="AuthWithTokenServer.TestService" behaviorConfiguration="AuthenticationTokenServiceBehavior">
      <endpoint address="" behaviorConfiguration="AjaxEnabledBehavior" binding="webHttpBinding" bindingConfiguration="webBindingSSL" contract="AuthWithTokenServer.TestService" />
      </service>
    </services>

    <behaviors>           
      <serviceBehaviors>
        <behavior name="AuthenticationTokenServiceBehavior">
          <serviceMetadata httpGetEnabled="true" httpsGetEnabled="true" />
          <serviceDebug includeExceptionDetailInFaults="true" />
          <TokenValidationBehaviorExtension />
        </behavior>
      </serviceBehaviors>
    </behaviors>

    <extensions>
      <behaviorExtensions>
        <add name="TokenValidationBehaviorExtension" type="AuthWithTokenServer.Core.Extensions.IDispatchMessageInspector.TokenValidationBehaviorExtension, AuthWithTokenServer, Version=1.0.0.0, Culture=neutral" />
      </behaviorExtensions>
    </extensions>

    <serviceHostingEnvironment aspNetCompatibilityEnabled="true" multipleSiteBindingsEnabled="true" />
  </system.serviceModel>
  ```

### 6. Basic Authentication

Basic Authentication is an html request header. The header is named **Authorization** and the value is as follows:

  ```yaml
  Basic amFyZWQ6dGVzdHB3
  ```

The first part of the **Authorization** header value is just the word **Basic** followed by a space.
The second part is the username and password concatenated together with a **semicolon separator** and then **Base64 encoded**.

  ```yaml
  testuser:testpassword
  Basic dGVzdHVzZXI6dGVzdHBhc3N3b3Jk
  ```

### 7. IBasicAuthenticationDecoder and GetUserCredentials() Method

Use **IBasicAuthenticationDecoder** and **GetUserCredentials** to decode user basic credentials.

### 8. Setting Up Web Services for SSL

- Add an Binding configuration with the security mode set to Transport.
- Use webHttpBinding because we are using **JSON** and **REST-like** (not full REST) WCF services.
- Configure the endpoints to use the newly created Binding configuration (use **HTTPS** not **HTTP**)

    ```xml
    <services>
      <service name="AuthWithTokenServer.AuthenticationTokenService" behaviorConfiguration="ServiceBehaviorHttp">
        <endpoint address="" behaviorConfiguration="AjaxEnabledBehavior" binding="webHttpBinding" bindingConfiguration="webBindingSSL" contract="AuthWithTokenServer.AuthenticationTokenService" />
      </service>
      <service name="AuthWithTokenServer.TestService" behaviorConfiguration="AuthenticationTokenServiceBehavior">
        <endpoint address="" behaviorConfiguration="AjaxEnabledBehavior" binding="webHttpBinding" bindingConfiguration="webBindingSSL" contract="AuthWithTokenServer.TestService" />
      </service>
    </services>

    <bindings>
      <webHttpBinding>
        <binding name="webBindingSSL">
          <security mode="Transport">
            <transport clientCredentialType="None"/>
          </security>
        </binding>
      </webHttpBinding>
    </bindings>
    ```
