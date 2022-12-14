function GerenciarDepartamentos(pUKFuncao, pUKEmpresa) {
    alert();
    $('#modalFuncaoDepartamento').modal('show');
    $('#modalFuncaoDepartamentoX').hide();
    $('#modalFuncaoDepartamentoFechar').removeClass('disabled');
    $('#modalFuncaoDepartamentoFechar').removeAttr('disabled', 'disabled');
    $('#modalFuncaoDepartamentoSalvar').removeClass('disabled');
    $('#modalFuncaoDepartamentoSalvar').removeAttr('disabled', 'disabled');
    $('#modalFuncaoDepartamentoCorpo').html('');
    $('#modalFuncaoDepartamentoCorpoConfirmar').hide();
    $('#modalFuncaoDepartamentoCorpoLoading').show();

    $.ajax({
        method: "GET",
        url: "/CargoFuncAtiv/GerenciarDepartamentos",
        data: { UKFuncao: pUKFuncao },
        error: function (erro) {
            alert(erro.responseText);
            $('#modalFuncaoDepartamento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            CarregarDepartamentos(pUKEmpresa)
            $('#modalFuncaoDepartamentoCorpoLoading').hide();
            $('#modalFuncaoDepartamentoCorpo').show();
            $('#modalFuncaoDepartamentoCorpo').html(content);
        },
    });
}

function OnSuccessGerenciarDpto(data) {
    TratarResultadoJSON(data.resultado);
}

function OnBeginGerenciarDpto() {
    $('#modalFuncaoDepartamento').modal('show');
    $('#modalFuncaoDepartamentoX').hide();
    $('#modalFuncaoDepartamentoFechar').addClass('disabled');
    $('#modalFuncaoDepartamentoFechar').attr('disabled', 'disabled');
    $('#modalFuncaoDepartamentoSalvar').hide();
    $('#modalFuncaoDepartamentoCorpo').hide();
    $('#modalFuncaoDepartamentoCorpoConfirmar').hide();
    $('#modalFuncaoDepartamentoCorpoLoading').show();
}

function CarregarDepartamentos(UKEmpresa) {
    //$.ajax({
    //    url: "/Departamento/CarregarDepartamentos",
    //    type: 'POST',
    //    dataType: "json",
    //    data: { UKEmpresa: UKEmpresa },
    //    success: function (response) {
    //        var myObject = eval('(' + response + ')');

    //        var demo2 = $('.demo2').bootstrapDualListbox({
    //            nonSelectedListLabel: 'Non-selected',
    //            selectedListLabel: 'Selected',
    //            preserveSelectionOnMove: 'moved',
    //            moveOnSelect: false,
    //            nonSelectedFilter: 'ion ([7-9]|[1][0-2])'
    //        });

    //        $("#duallist").empty();
    //        for (var i = 0; i < myObject.length; i++) {
    //            $("#duallist").append('<option value="' + myObject[i].UniqueKey + '">' + myObject[i].Sigla + '</option>');
    //        }

    //        $("#duallist").bootstrapDualListbox('refresh');
    //    }
    //});
}
