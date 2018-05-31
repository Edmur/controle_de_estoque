function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#id_uf').val(dados.Id_Uf);
    $('#txt_nome').val(dados.Nome);
}

function set_focus_form() {
    $('#id_uf').focus();
}

function set_dados_grid(dados) {
    return '<td class="detalhe-center">' + dados.Id_Uf + '</td>' +
        '<td class="detalhe-left">' + dados.Nome + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Id_Uf: 0,
        Nome: ''
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Id_Uf: $('#id_uf').val(),
        Nome: $('#txt_nome').val()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Id_Uf).end()
        .eq(1).html(param.Nome).end();
}