$(document).ready(function () {
    //var aceitacaoCookies = sessionStorage.getItem("Cookies");
    var aceitaCookie = getCookie('AceitaCookies');

    if (aceitaCookie == null) {
        $('#exampleModal').modal('show');
    }

    $.ajax({
        method: "GET",
        url: "/Notificacao/QtdNotificacao"
    }).done(function (result) {
        if (result > 0) {
            $('#group').find('.popupNotify').text(result);
        } else {
            $('#nPopup').hide();
        }
    });

    //$(".btn").click(function () {
    //    var val = parseInt($('#group').find('.popupNotify').text());

    //    // Check for the button clicked
    //    if ($(this).hasClass('btn-danger')) {
    //        $('#group').find('.popupNotify').text(val - 1);
    //    } else if ($(this).hasClass('btn-success')) {
    //        $('#group').find('.popupNotify').text(val + 1);
    //    }
    //});
});

function SalvarConcordancia() {
    //sessionStorage.setItem("Cookies", "True")
    setCookie('AceitaCookies', 'True', 2);

    $('#exampleModal').modal('hide');
}

function ConfirmacaoExcluirEvento() {
    
    $('#confirmar-excluir').modal('show');
}

function FecharModal() {
    $('#confirmar-excluir').modal('hide');
}

function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

function eraseCookie(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}