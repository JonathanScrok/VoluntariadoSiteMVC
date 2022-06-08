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

function autoComplete(cidade) {

    const destino = ['Salvador', 'Vitória', 'Maceió', 'Ceará', 'Rio Branco', 'Macapá', 'Porto Velho', 'Olinda', 'Aracaju', 'Capitólio', 'São Paulo', 'Paraty'];
    return destino.filter((valor) => {
        const valorMinusculo = valor.toLowerCase()
        const cidadeMinusculo = cidade.toLowerCase()

        return valorMinusculo.includes(cidadeMinusculo)
    })
}
const campo = document.querySelector('#Cidade_Estado')
const sugestoes = document.querySelector('.sugestoes')

campo.addEventListener('input', ({ target }) => {
    const dadosDoCampo = target.value
    if (dadosDoCampo.length) {
        const autoCompleteValores = autoComplete(dadosDoCampo)
        sugestoes.innerHTML = `
            ${autoCompleteValores.map((value) => {
                return (`<li><a href="#" onclick="valorEscolhido('${value}')" style="color: black;">${value}</a></li>`)
            }).join('')}`
    }
})

function valorEscolhido(valor) {
    document.querySelector('#Cidade_Estado').value = valor;
    document.querySelector('.sugestoes').innerHTML = '';
}
