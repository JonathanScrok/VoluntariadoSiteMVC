function imprimir() {

        // Remove Menu topo
        $("#navbarId").hide();
        $("#logoSiteBAH").hide();
        $("#navbarOptions").hide();
        $(".FoooterSite").hide();
        $(".botaoImprimir").hide();
        //$('body').css({ "padding-top": "0px" });

        // Imprime a página
        window.print();

        location.reload();

        // Adiciona Menu topo
        //$("#navbarId").show();
        //$("#logoSiteBAH").show();
        //$(".FoooterSite").show();
        //$(".botaoImprimir").show();
        //$('body').css({ "padding-top": "40px" });
    }