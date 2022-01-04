$(document).ready(function () {
    //var aceitacaoCookies = sessionStorage.getItem("Cookies");
    var aceitaCookie = getCookie('AceitaCookies');
    document.getElementById("CompartilharWpp").href = "https://api.whatsapp.com/send?text="+ window.location.href;
    document.getElementById("CompartilharFacebook").href = "https://www.facebook.com/sharer/sharer.php?u=" + window.location.href;

    if (aceitaCookie == null) {
        $('#exampleModal').modal('show');
    }
});

function SalvarConcordancia() {
    //sessionStorage.setItem("Cookies", "True")
    setCookie('AceitaCookies', 'True', 2);

    $('#exampleModal').modal('hide');
}

function ConfirmacaoExcluirVaga() {
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

function share() {
    $('#compartilhar-vaga').modal('show');
}

function CopiarLink() {
    mensagem.style.display = "block";

    var copyText = window.location.href;

    /* Copy the text inside the text field */
    navigator.clipboard.writeText(copyText);
}

let mensagem = document.querySelector(".mensagem");

// mostra a mensagem
function showMessage() {
    mensagem.style.display = "block";
}
// esconde a mensagem
function hideMessage() {
    mensagem.style.display = "none";
}