function validaCheckSemData() {
    var chkSemData = document.getElementById('SemData');
    if (chkSemData.checked) {
        document.getElementById("SemData").value = true;
    }
    else {
        document.getElementById("SemData").value = false;
    }
}

function validaCheckEventoRecorrente() {
    var chkEventoRecorrente = document.getElementById('EventoRecorrente');
    if (chkEventoRecorrente.checked) {
        document.getElementById("EventoRecorrente").value = true;
    }
    else {
        document.getElementById("EventoRecorrente").value = false;
    }
}