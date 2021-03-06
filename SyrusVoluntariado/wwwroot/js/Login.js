let Dominio = window.location.origin;

function validaCheckManterConectado() {
    var chkManterConectado = document.getElementById('ManterConectado');

    if (chkManterConectado.checked) {
        document.getElementById("ManterConectado").value = true;
    }
    else {
        document.getElementById("ManterConectado").value = false;
    }
}

function mascara(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value)
}
function mtel(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    v = v.replace(/^(\d{2})(\d)/g, "($1) $2"); //Coloca parênteses em volta dos dois primeiros dígitos
    v = v.replace(/(\d)(\d{4})$/, "$1-$2"); //Coloca hífen entre o quarto e o quinto dígitos
    return v;
}
function id(el) {
    return document.getElementById(el);
}

if (window.location.href == Dominio + "/login/cadastrarusuario") {
    window.onload = function () {
        id('celularcadastro').onkeyup = function () {
            mascara(this, mtel);
        }
    }
}
