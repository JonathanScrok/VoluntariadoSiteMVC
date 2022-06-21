$(document).ready(function () {
    //var aceitacaoCookies = sessionStorage.getItem("Cookies");
    var aceitaCookie = getCookie('AceitaCookies');
    document.getElementById("CompartilharWpp").href = "https://api.whatsapp.com/send?text=" + window.location.href;
    document.getElementById("CompartilharFacebook").href = "https://www.facebook.com/sharer/sharer.php?u=" + window.location.href;
    document.getElementById("GerarQrcode").href = window.location.origin + "/qrcode/?qrtexto=" + window.location.href;

    if (aceitaCookie == null) {
        //$('#exampleModal').modal('show');
        var divCookies = document.querySelector(".cookies-container");

        if (divCookies.style.display === "none") {
            divCookies.style.display = "block";
        }

    } else {
        var divCookies = document.querySelector(".cookies-container");

        if (divCookies.style.display === "block") {
            divCookies.style.display = "none";
        }
    }
});

function SalvarConcordancia() {
    //sessionStorage.setItem("Cookies", "True")
    setCookie('AceitaCookies', 'True', 2);

    $('#exampleModal').modal('hide');

    var divCookies = document.querySelector(".cookies-container");

    if (divCookies.style.display === "none") {
        divCookies.style.display = "block";
    } else {
        divCookies.style.display = "none";
    }
}

function ConfirmacaoExcluirEvento() {
    $('#confirmar-excluir').modal('show');
}


function ConfirmacaoAdicionarAdministrador() {
    $('#adicionar-administrador').modal('show');
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
    $('#compartilhar-evento').modal('show');
}

let mensagem = document.querySelector(".mensagem");

function CopiarLink() {
    mensagem.style.display = "flex";

    var copyText = window.location.href;

    // Copia Texto para Area de Transferencia
    navigator.clipboard.writeText(copyText);
}

// mostra a mensagem
function showMessage() {
    mensagem.style.display = "flex";
}
// esconde a mensagem
function hideMessage() {
    mensagem.style.display = "none";
}

var toastTrigger = document.getElementById('liveToastBtn')
var toastLiveExample = document.getElementById('liveToast')
var toastLiveExample2 = document.getElementById('liveToast2')
if (toastTrigger) {
    toastTrigger.addEventListener('click', function () {
        var toast = new bootstrap.Toast(toastLiveExample)
        var toast2 = new bootstrap.Toast(toastLiveExample2)

        toast.show()
        toast2.show()
    })
}

function ValidaEmailAdm() {
    var email = document.getElementById('emailAdmAdicionar').value;
    var mensagemEmailNE = document.querySelector("#mensagemEmailNE");
    let SITE_URL = window.location.origin;
    if (email != null && email != "") {
        $.ajax({
            method: "GET",
            url: SITE_URL + "/evento/ExisteEmail/?Email=" + email,
            contentType: "application/json"
        }).done(function (result) {
            if (!result) {
                mensagemEmailNE.style.display = "block";
            } else {
                mensagemEmailNE.style.display = "none";

                var administradoresStorage = localStorage.getItem("administrador");

                if (administradoresStorage == null || administradoresStorage == "") {
                    let valor = email + ",";
                    localStorage.setItem("administrador", valor);
                } else {
                    if (!administradoresStorage.includes(email)) {
                        let valor = email + ",";
                        let valorFinal = administradoresStorage + valor;
                        localStorage.setItem("administrador", valorFinal);
                    }
                }
            }
        }).fail(function (err) {
            if (!err) {
                mensagemEmailNE.style.display = "block";
            } else {
                mensagemEmailNE.style.display = "none";

                var administradoresStorage = localStorage.getItem("administrador");

                if (administradoresStorage == null || administradoresStorage == "") {
                    let valor = email + ",";
                    localStorage.setItem("administrador", valor);
                } else {
                    if (!administradoresStorage.includes(email)) {
                        let valor = email + ",";
                        let valorFinal = administradoresStorage + valor;
                        localStorage.setItem("administrador", valorFinal);
                    }
                }
            }
        });
    }
}

function LimpaEmailAdm() {
    document.getElementById('emailAdmAdicionar').value = "";
    var mensagemEmailNE = document.querySelector("#mensagemEmailNE");
    mensagemEmailNE.style.display = "none";
}