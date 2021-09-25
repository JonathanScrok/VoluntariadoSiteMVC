$(document).ready(function () {
    const btn = document.querySelector("button");

    btn.onclick = () => {
        var nota = $("input[name=rate]:checked").val();
        console.log(window.location.origin + "/avaliar/postavaliacao/" + nota);
        window.location.href = window.location.origin + "/avaliar/postavaliacao/" + nota;

        return false;
    }
});
