const $snyapi = "https://snyapi.azurewebsites.net/";
const $snyweb = "https://snyweb.azurewebsites.net/";

$( "#LogForm" ).submit(function( event ) {
    event.preventDefault();
    FormReset();
    callLOGIN($("#email").val(), $("#password").val());
});

$( "#RegForm" ).submit(function( event ) {
    event.preventDefault();
    FormReset();
    callREG($("#reg-email").val(), $("#reg-password").val(), $("#reg-password2").val());
});


function loginFail() {
    addError("#email", 'Bohužel špatně');
    addError("#password");
}
function loginSuccess(data) {
    window.location.replace($snyweb + "/login?jwt=" + data.jwt);
}

function callLOGIN($email, $password) {
    let url = $snyapi + "account/login";
    $.ajax({
        type: 'POST',
        crossDomain: true,
        url: url,
        contentType: 'application/json',
        data : JSON.stringify({ "email": $email, "password": $password }),
    }).done(function(data) {
        loginSuccess(data);
    }).fail(function(errorThrown) {
        loginFail(errorThrown);
    });
}


function FormReset() {
    $( "input" )
        .each(function( ) {
            $(this).removeClass( 'is-invalid' );
        });
    $( ".invalid-tooltip" )
        .each(function( ) {
            $(this).remove();
        });
    $( ".has-validation" )
        .each(function( ) {
            $(this).removeClass( 'has-validation' );
        });
}

function addError($id, $error) {
    $( $id ).addClass( 'is-invalid' );
    if ($error) {
        $( $id ).next().text($error);
        $( $id ).parents('.input-group').addClass( 'has-validation' );
        $( $id ).parents('.input-group').append('<div class="invalid-tooltip">' + $error + '</div>');
    }
}


function regSuccess() {
    $("#registrationModal").hide();
    addAlert('<i class="bi bi-check-circle-fill"></i> Registrace se podařila. Rychle se přihlaš než zapomenš heslo', 'success');
}

function callREG($email, $password, $password2) {
    let url = $snyapi + "account/register";
    $.ajax({
        type: 'POST',
        crossDomain: true,
        url: url,
        contentType: 'application/json',
        data : JSON.stringify({ "email": $email, "password": $password, 'passwordAgain': $password2 }),
    }).done(function(data) {
        switch (data.registerStatus) {
            case 'Success': regSuccess(data.registerStatus); break;
            case 'WeakPassword': addError("#reg-password", 'Nastav si silnější heslo'); break
            case 'PasswordsNotSame': addError("#reg-password2", 'Obě hesla musí být stejný'); break;
            case 'AlreadyExists': addError("#reg-email", 'Tento Email už známe'); break;
            case 'BadEmailFormat': addError("#reg-email", 'Takto email nevypadá'); break;
            case 'Error': addError("#reg-email", 'Něco je špatně'); break;
        }

    }).fail(function() {
        addError("#reg-email", 'Něco je šeredně špatně');
    });
}


function addAlert(text, type) {
    let alert = $('<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">'
        + text +
        '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
    $('#alerts').append(alert);
}