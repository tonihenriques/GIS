﻿AplicaValidacaoCPF();

jQuery(function ($) {
    $('#txtCPF').mask('999.999.999-99');
    $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });
    AplicaDatePicker();

});

function OnSuccessCadastrarEmpregado(data) {
    TratarResultadoJSON(data.resultado);

    $("#btnLimpar").click();
    $(".LoadingLayout").hide();
    $("#formCadastroEmpregado").css({ opacity: "1" });

    //$('#formCadastroEmpregado').removeAttr('style');
    //$(".LoadingLayout").hide();
    //$('#btnSalvar').show();
    //TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarEmpregado() {
    $(".LoadingLayout").show();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });
}