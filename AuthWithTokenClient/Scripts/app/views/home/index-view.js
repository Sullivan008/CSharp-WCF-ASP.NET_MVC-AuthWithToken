var indexView = {

    initActions: function () {
        $("#authentication-form").submit(function (event) {
            event.preventDefault();

            $.ajax({
                url: "../Home/Authenticate",
                method: "POST",
                data: $("#authentication-form").serialize(),
                success: function (response) {
                    $("#secretToken").html("<b>" + response.SecureToken + "<b>");
                    $('input[id="hiddenSecretToken"]').attr('value', response.SecureToken);
                },
                error: function (request, status, error) {
                    modal.warningModal(request.responseJSON);
                }
            });
        });

        $("#getTokenTest-form").submit(function (event) {
            event.preventDefault();

            var tokenData = $("#getTokenTest-form").serializeToJSON();
            
            $.ajax({
                url: "../Home/GetTokenTest",
                method: "POST",
                data: {
                    '__RequestVerificationToken': tokenData.__RequestVerificationToken,
                    'secureToken': tokenData.SecureToken
                },
                success: function (response) {
                    var bodyContent =
                        "<div><span class='response-title-span'>Message: </span><span>" + response.Message + "</span></div>" +
                        "<div><span class='response-title-span'>Logged in User Id: </span><span>" + response.UserId + "</span></div>" +
                        "<div><span class='response-title-span'>Logged in User Name: </span><span>" + response.UserName + "</span></div>";

                    $("#getTokenTestResult").html(bodyContent);
                },
                error: function (request, status, error) {
                    modal.warningModal(request.responseJSON, 650);
                }
            });
        });

        $("#postBasicAuthTest-form").submit(function (event) {
            event.preventDefault();

            var userData = $("#authentication-form").serializeToJSON();
           
            $.ajax({
                url: "../Home/PostBasicAuthTest",
                method: "POST",
                data: {
                    '__RequestVerificationToken': userData.__RequestVerificationToken,
                    'authorizationString': "Basic " + btoa(userData.UserName + ":" + userData.Password)
                },
                success: function (response) {
                    var bodyContent =
                        "<div><span class='response-title-span'>Message: </span><span>" + response.Message + "</span></div>" +
                        "<div><span class='response-title-span'>Logged in User Id: </span><span>" + response.UserId + "</span></div>" +
                        "<div><span class='response-title-span'>Logged in User Name: </span><span>" + response.UserName + "</span></div>";

                    $("#postBasicAuthTestResult").html(bodyContent);
                },
                error: function (request, status, error) {
                    modal.warningModal(request.responseJSON, 650);
                }
            });
        });
    },

    init: function () {
        this.initActions();
    }
}

$(document).ready(function () {
    indexView.init();
});