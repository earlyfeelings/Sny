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
    'a2889794017_10.jpg',
    'ezgif.com-gif-maker.jpg',
    
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

    'A von hodně trpěl, protože trpěl. - Jolanda',
    'Ale ze sraček můžete vytáhnout se. - Jolanda',
    'Hodně budeš někde. - Jolanda',
    'Šíří se fámy, že mám velmi rád becherovku a pivo. Obě jsou pravdivé. —  Miloš Zeman',
    'ÚSPĚCH je 80% spěch, 20% Ú a 0% kritické myšlení. —  Mikýř',

    'Zatímco ztrácíme svůj čas váháním a odkládáním, život utíká.',
    'Přijmi to co je. Nech plavat to, co bylo. A měj víru v to, co přijde.',
    'Jestli chceš něco, co jsi nikdy neměl, tak musíš dělat něco, co jsi nikdy nedělal.',
    'Tvůj život je výsledkem rozhodnutí, které děláš. Pokud se ti tvůj život nelíbí, je čas vybrat si lépe.',
    'Lidé ve svém životě nelitují toho co udělali, ale toho co neudělali.',
    'Každá chyba je příležitostí se něčemu naučit. ',
    'Jedinou překážkou mezi tebou a tvým cílem je ten blábol, kterým si odůvodňuješ, proč to nejde.',
    'Přijít můžeš jen o to, co máš. To čím jsi, to neztratíš.',
    'Sebejistota nevychází z toho, že vždycky víš, co děláš, ale z toho, že se nebojíš šlápnout vedle.',
    'Víra znamená věřit tomu, co nevidíš. Za odměnu pak uvidíš to, v co věříš.',
    'Život není problém, který je třeba řešit, je to skutečnost, kterou je třeba poznat.',
    'Lva / Vlka nezajímá, co si o něm ovce myslí.',
    'Existuje tisíce způsobů, jak zabít čas, ale žádný, jak ho vzkřísit.',
    'Nemusíte být skvělí, abyste začali, ale musíte začít, abyste byli skvělí.',
    'Ničeho jsem nenabyl lehce, každá věc mně stála nejtvrdší práci. Nehledejte lehké cesty. Ty hledá tolik lidí, že se po nich nedá přijít nikam.',
    'Osud míchá karty, my hrajeme.',
    'Je jenom jedna cesta za štěstím a to přestat se trápit nad tím, co je mimo naši moc.',
    'Když něco opravdu chceš, celý vesmír se spojí, abys to mohl uskutečnit.',
    'Slaboši čekají na příležitost, silní ji vytvářejí.',
    'Představivost je důležitější než vědomosti.',
    'Vysoké cíle, třebaže nesplnitelné, jsou cennější než nízké, třebaže splnitelné.',
    'Co chceš, můžeš.',
    'Nejlepší způsob, jak se do něčeho pustit, je přestat o tom mluvit a začít to dělat.',
    'Nepřej si, aby to bylo snazší; přej si, abys byl lepší.',
    'Jestliže neumíš, naučíme, jestliže nemůžeš, pomůžeme ti, jestliže nechceš, nepotřebujeme tě.',
    'Čím jsem starší, tím méně si všímám, co lidé říkají, myslí si a v co doufají. Všímám si toho, co dělají, jak žijí a o co usilují.',
    'Žij, jako bys měl zítra zemřít. Uč se, jako bys měl navždy žít.',
    'Je lepší zemřít pro něco než žít pro nic.',
    'Život se nepíše, život se žije.',
    'Nauč se slzami v očích smát, nauč se pohladit se zavřenou dlaní, nauč se rozdat všechno a nemít nic, pak poznáš, že stojí za to žít.',
    'Pokud chcete být nenahraditelní, musíte být odlišní.',
    'Žijete jenom jednou. Tak by to měla být zábava.',
    'Svůj úspěch hodnoťte podle toho, čeho všeho jste se pro něj vzdali.',
    'Jestliže tvoje štěstí závisí na tom, co dělá někdo druhej, pak máš, myslím, problém!',
    'Jediná věc, která stojí mezi vámi a vaším cílem, jsou ty kecy o tom, jak to nezvládnete, které si neustále namlouváte.',
    'Člověk, který přichází s novou myšlenkou je blázen do té doby, než jeho myšlenka zvítězí.',
    'Není nic moudřejšího, než přesně vědět, kdy máš co začít a kdy s čím přestat.',
    'Jestli se ti něco nelíbí, změň to! Nejsi strom.',
    'Co tě nezabije, to tě posílí',
    'Svět je krásný a stojí za to o něj bojovat.',
    'Žádný člověk není takový hlupák, aby nedosáhl úspěchu aspoň v jedné věci, je-li vytrvalý.',
    'Nejde-li o život, jde o hovno.',
    'Stojí za to nebát se smrti, protože pak se neumíš bát ničeho.',
    'Včera jsem byl chytrý, proto jsem chtěl změnit svět. Dnes jsem moudrý, proto měním sám sebe.',
    'Můžete mít buď výmluvy nebo výsledky. Nikdy ne obojí.',
    'Šampióni nevznikají v posilovnách. Šampióni vznikají z něčeho, co mají hluboko v sobě – z touhy, snu a vize.',
    'Žij přítomností, sni o budoucnosti, uč se minulostí.',
    'Jediná omezení, která v lidských životech existují si klademe my sami.',
    'Není pravda, že máme málo času, avšak pravda je, že ho hodně promarníme.',
    'Dělej dobro a dobro se ti vrátí.',
    'Věřím, že fantazie je silnější než vědění. Že mýty mají větší moc než historie. Že sny jsou mocnější než skutečnost. Že naděje vždy zvítězí nad zkušeností. Že smích je jediným lékem na zármutek. A věřím, že láska je silnější než smrt.',
    'Neodpoutávej se nikdy od svých snů! Když zmizí, budeš dál existovat, ale přestaneš žít.',
    'Abyste mohl být ten nejlepší šampion, musíte věřit, že jím skutečně jste. Pokud nejste, musíte alespoň předstírat, že jím jste.',
    'Nejhorší ze všech tragédií není zemřít mladý, nýbrž žít do pětasedmdesáti, a přece nikdy nežít doopravdy.',
    'Nemůžeš-li létat, běž, nemůžeš-li běžet, jdi, nemůžeš-li ani jít, plaz se. Ale ať už děláš cokoli, musíš se neustále pohybovat kupředu.',
    'Když prohrajete, nezapomeňte na tu lekci.',
    'V podnikání nemůžete čekat, až bouřka přejde, je nutné naučit se tančit v dešti.',
    'Život je někdy velmi skoupý, uplynou dny, týdny, měsíce a roky, aniž člověk zažije něco nového. Ale pak se otevřou dveře a dovnitř se vřítí lavina. V jednu chvíli nemáte nic, a najednou máte víc, než dokážete přijmout.',
    'Svět patří těm, co se neposerou.',
    'Obyčejný člověk přemýšlí, jak by zaplnil čas. Talentovaný člověk se ho snaží využít.',
    'Jestli najdeš v životě cestu bez překážek, určitě nikam nevede.',
    'Pamatuj, že i ta nejtěžší hodina ve tvém životě, má jen 60 minut.',
    'Je zhola zbytečné se ptát, má-li život smysl či ne. Má takový smysl, jaký mu dáme.',
    'Jsem něžný, jsem krutý, ale jsem život. Pláčeš? I v slzách je síla. Tak jdi a žij.',
    'První krok proto, abyste od života získali to, co chcete je rozhodnout se, co to je.',
    'Otázkou není, zda mít nebo nemít pasivní příjem. Otázkou je kdy ho budete mít?',
    'Budoucnost patří těm, kdo věří svým krásným snům.',
    'Vše, co je v člověku krásné, je očima neviditelné.',
    'Žij každý den, jako bys právě v něm měl prožít celý svůj život.',
    'Pro život, ne pro školu se učíme.',
    'Špatné věci nejsou to nejhorší, co se nám může stát. Ta nejhorší věc, která se nám může stát, je NIC.',
    'Nenáviděl jsem každou minutu tréninku, ale vždy jsem si říkal: Teď protrpíš trénink a žij zbytek života jako mistr.  – Muhammad Ali',
    'Neselhal jsem 10000 krát. Našel jsem 10000 způsobů, které nefugnují. - Albert Einstein',
    'Všimli jste si, že ti nejchytřejší žáci ve škole nejsou těmi, kterým se daří v životě.',
    'Nikdo se mě neptal jestli se chci narodit, tak ať mi neříká jak mám žít.',
    'Dělejte to, z čeho máte strach. A dělejte to opakovaně. To je nejrychlejší a nejjistějších způsob, jak strach porazit.',
    'Špatná zpráva je ta, že čas letí. Dobrá zpráva je ta, že vy jste pilot.',
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
    return Math.floor(Math.random() * 3) + 6;
}

function generateLine() {
    let $size = getRandomSize();
    let $imageLine = '<div class="col-lg-' + $size + '"><div class="border"><img alt="image" src="' + getRandomImage() + '"></div></div>';
    let $quoteLine = '<div class="col-lg-' + (12 - $size) + '"> ' + getRandomQuote() + '</div>';
    return (Math.floor(Math.random() * 2) === 0 ) ? $imageLine + $quoteLine : $quoteLine + $imageLine;
}