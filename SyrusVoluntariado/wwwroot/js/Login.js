
function validaCheckManterConectado() {
    var chkManterConectado = document.getElementById('ManterConectado');

    if (chkManterConectado.checked) {
        document.getElementById("ManterConectado").value = true;
    }
    else {
        document.getElementById("ManterConectado").value = false;
    }
}