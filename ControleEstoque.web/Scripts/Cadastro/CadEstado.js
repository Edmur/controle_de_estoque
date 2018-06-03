function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_uf').val(dados.Uf);
    $('#ddl_pais').val(dados.Id_Pais);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {
    $('#ddl_pais').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        Uf: '',
        Id_Pais: '',
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        Uf: $('#txt_uf').val(),
        Id_Pais: $('#ddl_pais').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Uf).end()
        .eq(2).html(param.Id_Pais).end()
        .eq(3).html(param.Ativo ? 'Sim' : 'Não');
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});
