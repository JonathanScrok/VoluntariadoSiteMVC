

$(document).ready(function () {
    var aceitacaoCookies = sessionStorage.getItem("Cookies");
    if (aceitacaoCookies == null) {
        $('#exampleModal').modal('show');
    }
});

function SalvarConcordancia() {
    sessionStorage.setItem("Cookies", "True")

    $('#exampleModal').modal('hide');
}

function ConfirmacaoExcluirVaga() {
    $('#confirmar-excluir').modal('show');
}

function FecharModal() {
    $('#confirmar-excluir').modal('hide');
}