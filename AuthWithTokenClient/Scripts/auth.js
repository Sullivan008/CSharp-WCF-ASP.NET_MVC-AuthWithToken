

var getAuthenticateButtonClickResult = function (data) {
    var jsonArrayLength = 0;

    try
    {
        $.each(data, function (index, value) {
            jsonArrayLength++;
        });
    }
    catch (err) {
        console.log("++++ ERROR - Message: " + err);
    }

    if (jsonArrayLength > 1) {
        alert("ERROR CODE: \t\t\t" + data.StatusCode + "\n\nA hiba oka: \t\t\t\t" + data.Reason + "\n\nInformáció a hibáról: \t\t" + data.InformationAbouotReason);
    }
    else {
        console.log(data);
        document.getElementById('token').innerHTML = data;
        document.getElementById('hiddenTokenText').value = data;
    }
};

var getTokenButtonClickResult = function (data) {
    var jsonArrayLength = 0;

    try {
        $.each(data, function (index, value) {
            jsonArrayLength++;
        });
    }
    catch (err) {
        console.log("++++ ERROR - Message: " + err);
    }

    if (jsonArrayLength > 1) {
        alert("ERROR CODE: \t\t\t" + data.StatusCode + "\n\nA hiba oka: \t\t\t\t" + data.Reason + "\n\nInformáció a hibáról: \t\t" + data.InformationAbouotReason);
    }
    else {
        document.getElementById('getTokenTextResult').innerHTML = data;
    }
}

var postBasicAuthenticateButtonClickResult = function (data) {
    var jsonArrayLength = 0;

    try {
        $.each(data, function (index, value) {
            jsonArrayLength++;
        });
    }
    catch (err) {
        console.log("++++ ERROR - Message: " + err);
    }

    if (jsonArrayLength > 1) {
        alert("ERROR CODE: \t\t\t" + data.StatusCode + "\n\nA hiba oka: \t\t\t\t" + data.Reason + "\n\nInformáció a hibáról: \t\t" + data.InformationAbouotReason);
    }
    else {
        document.getElementById('postBasicAuthTextResult').innerHTML = data;
    }
}

$(document).ready(function () {
    $('authenticationForm').attr('autocomplete', 'new-password');

    $('#postBasicAuthTestButton').click(function () {
        document.getElementById('hiddenBasicAuthText').value = "Basic " + btoa(document.getElementById('User').value + ":" + document.getElementById('Password').value);
    });
});