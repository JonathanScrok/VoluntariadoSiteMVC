$(document).ready(function () {

});

function voluntariarNaVaga() {

    let SITE_URL = window.location.origin;
    $.ajax({
        method: "GET",
        url: SITE_URL + "/vaga/voluntariar/?id=" + idVaga,
        contentType: "application/json"
    }).done(function (result) {
        $("#BotaoVoluntatiarMe").html('Já Voluntariado');
        $('#BotaoVoluntatiarMe').prop('disabled', true);
    }).fail(function (err) {
        console.log(err);
    });
}

