function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_sigla').val(dados.Sigla);
    $('#txt_descricao').val(dados.Descricao);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {
    $('#txt_sigla').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Sigla: '',
        Descricao: '',
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Sigla: $('#txt_sigla').val(),
        Descricao: $('#txt_descricao').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Sigla).end()
        .eq(1).html(param.Descricao).end()
        .eq(2).html(param.Ativo ? 'Sim' : 'Não');
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});

