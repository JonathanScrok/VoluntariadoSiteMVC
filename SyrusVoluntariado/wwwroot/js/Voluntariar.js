$(document).ready(function () {

});

function voluntariarNoEvento() {

    let SITE_URL = window.location.origin;
    $.ajax({
        method: "GET",
        url: SITE_URL + "/evento/voluntariar/?id=" + idEvento,
        contentType: "application/json"
    }).done(function (result) {
        $("#BotaoVoluntatiarMe").html('Já Voluntariado');
        $('#BotaoVoluntatiarMe').prop('disabled', true);
    }).fail(function (err) {
        console.log(err);
    });
}

