function validaCheckSemData() {
    var chkSemData = document.getElementById('SemData');
    var chkEventoRecorrente = document.getElementById('EventoRecorrente');

    if (chkSemData.checked) {
        document.getElementById("SemData").value = true;
        chkEventoRecorrente.checked = false;
        document.getElementById("EventoRecorrente").value = false;
    }
    else {
        document.getElementById("SemData").value = false;
    }
}

function validaCheckEventoRecorrente() {
    var chkEventoRecorrente = document.getElementById('EventoRecorrente');
    var chkSemData = document.getElementById('SemData');

    if (chkEventoRecorrente.checked) {
        document.getElementById("EventoRecorrente").value = true;
        chkSemData.checked = false;
        document.getElementById("SemData").value = false;
    }
    else {
        document.getElementById("EventoRecorrente").value = false;
    }
}