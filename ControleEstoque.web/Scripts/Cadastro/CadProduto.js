function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#ddl_categoria').val(dados.Id_Categoria);
    $('#ddl_fornecedor').val(dados.Id_Fornecedor);
    $('#txt_ean').val(dados.Ean);
    $('#txt_descricao').val(dados.Descricao);
    $('#txt_preco_custo').val(dados.PrecoCusto).mask('#,##0.00');
    $('#txt_preco_venda').val(dados.PrecoVenda).mask('#,##0.00');
    $('#ddl_unidade_medida').val(dados.Id_UnidadeMedida);
    $('#txt_qt_unidade').val(dados.Qt_UnidadeMedida);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {
    $('#txt_ean').focus();
}

function set_dados_grid(dados) {
    return '<td class="detalhe-left">' + dados.Id_Categoria + '</td>' +
        '<td class="detalhe-left">' + dados.Id_Fornecedor + '</td>' +
        '<td class="detalhe-center">' + dados.Ean + '</td>' +
        '<td class="detalhe-left">' + dados.Descricao + '</td>' +
        '<td class="detalhe-right">' + dados.PrecoCusto + '</td>' +
        '<td class="detalhe-right">' + dados.PrecoVenda + '</td>' +
        '<td class="detalhe-center">' + dados.Id_UnidadeMedida + '</td>' +
        '<td class="detalhe-right">' + dados.Qt_UnidadeMedida + '</td>' +
        '<td class="detalhe-center">' + (dados.Ativo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Id_Categoria: 0,
        Id_Fornecedor: 0,
        Ean: '',
        Descricao: '',
        PrecoCusto: 0.00,
        PrecoVenda: 0.00,
        Id_UnidadeMedida: 0,
        Qt_UnidadeMedida: 0,
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Id_Categoria: $('#ddl_categoria').val(),
        Id_Fornecedor: $('#ddl_fornecedor').val(),
        Ean: $('#txt_ean').val(),
        Descricao: $('#txt_descricao').val(),
        PrecoCusto: $('#txt_preco_custo').val(),
        PrecoVenda: $('#txt_preco_venda').val(),
        Id_UnidadeMedida: $('#ddl_unidade_medida').val(),
        Qt_UnidadeMedida: $('#txt_qt_unidade').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Id_Categoria).end()
        .eq(1).html(param.Id_Fornecedor).end()
        .eq(2).html(param.Ean).end()
        .eq(3).html(param.Descricao).end()
        .eq(4).html(param.PrecoCusto).end()
        .eq(5).html(param.PrecoVenda).end()
        .eq(6).html(param.Id_UnidadeMedida).end()
        .eq(7).html(param.Qt_UnidadeMedida).end()
        .eq(8).html(param.Ativo ? 'Sim' : 'Não');
}

$(document)
    .ready(function () {
        $('#txt_preco_custo,#txt_preco_venda').mask('#.##0,00', { reverse: true });
        $('#txt_qt_unidade').mask('00000');
    });