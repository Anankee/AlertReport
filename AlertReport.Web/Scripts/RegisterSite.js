var isLoginValid = false;
var isEmailValid = false;


$('#Login').focusout(function () {
    var input = $(this);
    $.get('/Account/CheckLoginAvailability', { Login: $(this).val() }, function (data) {
        isLoginValid = consumeDataResponse(data, input, isLoginValid);
        checkFormValidation();
    });
});

$('#Email').focusout(function () {
    var input = $(this);
    $.get('/Account/CheckEmailAvailability', { Email: $(this).val() }, function (data) {
        isEmailValid = consumeDataResponse(data, input, );
        checkFormValidation();
    });
});

function consumeDataResponse(data, input) {
    var errorSpan = input.parent().find('.field-validation-valid');

    if (data.result === false && errorSpan.text() !== data.text) {  
        errorSpan.text(data.text);
        return false;
    }
    else if (data.result === true) {
        errorSpan.text('');
        return true;
    }
}

function checkFormValidation() {
    console.log(isLoginValid);
    console.log(isEmailValid);
    if (isLoginValid && isEmailValid) {
        $('#registration-btn').prop('disabled', false);
    }
    else {
        $('#registration-btn').prop('disabled', true);
    }
}