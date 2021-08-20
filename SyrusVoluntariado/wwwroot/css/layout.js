

$(document).ready(function () {
    var aceitacaoCookies = sessionStorage.getItem("Cookies");
    console.log(aceitacaoCookies);
    if (aceitacaoCookies == null) {
        $('#exampleModal').modal('show');
    }
    
});


function SalvarConcordancia() {
    sessionStorage.setItem("Cookies", "True")

    $('#exampleModal').modal('hide');
}


/*var descendentes = document.querySelectorAll("#cookies-click").click();*/

//if (window.location.pathname == "/") {
    
//}
