const $snyapi = "https://snyapi.azurewebsites.net/";
const $snyweb = "https://snyweb.azurewebsites.net/";

let $images = [
    'HappyPerson1.jpg',
    'plan.jpg',
    'scales.jpg',
    'secret-to-success.jpg',
    '1_FJk7Rcb64pK-Kv0AfauTKw.png',
    'FOTO_6-752x500.jpg',
    '5e22-image.png',
    'silné-stránky.jpg',
    'soft-hard-skills.png',
    'Want-to-develop-a-culture-of-continuous-improvement_Blog.jpg',
    'process-improvement-planning.png',
    '197094-3.jpg',
    '11-best-ways-to-improve-Personal-Development.jpg',
];
let $quotes = [
    'Nepropadejte Panice!',
    'Všechno bude v pořádku',
    'Přestaňte krvácet z konečníku',
    'Nemusíš se bát',
    'Dosáhni úspěchu',
    'Splň si své sny',
    'Jdi do toho',
    'Pracuj na sobě',
    'Dokážete to',
    'Nevzdávej se',
    'Nauč se něco nového',
    'Nauč se nový jazyk',
    'Pravidelně cvič',
    'Čti víc knih',
    'Více času na rodinu',
    'Více času na sebe',
    'Víc peněz na drogy',
];

$counter = 3;


$( "#LogForm" ).submit(function( event ) {
    event.preventDefault();
    FormReset();
    let $email = $("#email").val();
    let $password = $("#password").val();
    switch (true) {
        case !!$email && !!$password: callLOGIN($email, $password); break;
        case !$email && !$password: addError("#email", 'Email je potřeba napsat'); addError("#password", 'a heslo taky'); break;
        case !$email: addError("#email", 'Zapoměl jsi napsat email'); break;
        case !$password: addError("#password", 'Zapoměl jsi heslo?'); break;
        default: addError("#email", 'Něco je špatně'); addError("#password");
    }
});

$( "#RegForm" ).submit(function( event ) {
    event.preventDefault();
    FormReset();
    let $email = $("#reg-email").val();
    let $password = $("#reg-password").val();
    let $password2 = $("#reg-password2").val();
    if (!!$email && !!$password && !!$password2) {
        callREG($email, $password, $password2);
    } else {
        if (!$email) addError("#reg-email", 'Email je potřeba napsat');
        if (!$password) addError("#reg-password", 'Heslo je potřeba napsat');
        if (!$password2) addError("#reg-password2", 'Heslo je potřeba napsat ještě jednou');
    }
});

$('.randomQuote').text(getRandomQuote());

window.onscroll = function() {
    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
        addImage();
    }
};

function addImage() {
    if ($images.length > 0 && $quotes.length > 0 && $counter > 0) {
        $('.siter').append(generateLine());
        $counter--;
    }
}

function loginSuccess(data) {
    window.location.replace($snyweb + "/login?jwt=" + data.jwt);
}

function Loading($do) {
    if ($do) {
        $('#loading').modal('show');
    }
    else {
        setTimeout(function() {
            $('#loading').modal('hide');
        }, 500);
    }
}

function callLOGIN($email, $password) {
    Loading(true);
    let url = $snyapi + "account/login";

    $.ajax({
        type: 'POST',
        crossDomain: true,
        url: url,
        contentType: 'application/json',
        data : JSON.stringify({ "email": $email, "password": $password }),
    }).done(function(data) {
        loginSuccess(data);
        Loading(false);
    }).fail(function() {
        addError("#email", 'Bohužel špatně');
        addError("#password");
        Loading(false);
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
    $('#registrationModal').modal('hide');
    addAlert('<i class="bi bi-check-circle-fill"></i> Registrace se podařila. Rychle se přihlaš než zapomeneš heslo', 'success');
}

function callREG($email, $password, $password2) {
    Loading(true);
    let url = $snyapi + "account/register";
    $.ajax({
        type: 'POST',
        crossDomain: true,
        url: url,
        contentType: 'application/json',
        data : JSON.stringify({ "email": $email, "password": $password, 'passwordAgain': $password2 }),
    }).done(function(data) {
        switch (data.registerStatus) {
            case 'Success': regSuccess(); break;
            case 'WeakPassword': addError("#reg-password", 'Nastav si silnější heslo'); addError("#reg-password2"); break
            case 'PasswordsNotSame': addError("#reg-password2", 'Obě hesla musí být stejný'); break;
            case 'AlreadyExists': addError("#reg-email", 'Tento Email už známe'); break;
            case 'BadEmailFormat': addError("#reg-email", 'Takto email nevypadá'); break;
            case 'Error': addError("#reg-email", 'Něco je špatně'); break;
        }
        Loading(false);
    }).fail(function() {
        addError("#reg-email", 'Něco je šeredně špatně');
        Loading(false);
    });
}


function addAlert(text, type) {
    let alert = $('<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">'
        + text +
        '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>');
    $('#alerts').append(alert);
}


function getRandomQuote() {
    if ($quotes.length === 0) return '';
    let random = Math.floor(Math.random() * $quotes.length);
    let $ret = $quotes[random];
    $quotes.splice(random, 1);
    return $ret;
}

function getRandomImage() {
    if ($images.length === 0) return '';
    let random = Math.floor(Math.random() * $images.length);
    let $ret = $images[random];
    $images.splice(random, 1);
    return 'HappyImages/' + $ret;
}

function getRandomSize() {
    return Math.floor(Math.random() * 5) + 4;
}

function generateLine() {
    let $size = getRandomSize();
    let $imageLine = '<div class="col-lg-' + $size + '"><div class="border"><img alt="image" src="' + getRandomImage() + '"></div></div>';
    let $quoteLine = '<div class="col-lg-' + (12 - $size) + '"> ' + getRandomQuote() + '</div>';
    return (Math.floor(Math.random() * 2) === 0 ) ? $imageLine + $quoteLine : $quoteLine + $imageLine;
}