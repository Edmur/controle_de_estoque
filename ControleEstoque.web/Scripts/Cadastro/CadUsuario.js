function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#txt_nome').val(dados.Nome);
    $('#txt_email').val(dados.Email);
    $('#cbx_ativo').prop('checked', dados.Ativo);

    var lista_perfil = $('#lista_perfil');
    lista_perfil.find('input[type=checkbox]').prop('checked', false);

    if (dados.Perfis) {
        for (var i = 0; i < dados.Perfis.length; i++) {
            var perfil = dados.Perfis[i];
            var cbx = lista_perfil.find('input[data-id-perfil=' + perfil.Id + ']');
            if (cbx) {
                cbx.prop('checked', true);
            }
        }
    }
}

function set_focus_form() {
    $('#txt_login').focus();
}

function set_dados_grid(dados) {
    return '<td class="detalhe-left">' + dados.Login + '</td>' +
        '<td class="detalhe-left">' + dados.Nome + '</td>' +
        '<td class="detalhe-left">' + dados.Email + '</td>' +
        '<td class="detalhe-center">' + (dados.Ativo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Login: '',
        Senha: '',
        Nome: '',
        Email: '',
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Login: $('#txt_login').val(),
        Senha: $('#txt_senha').val(),
        Nome: $('#txt_nome').val(),
        Email: $('#txt_email').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Login).end()
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.Email).end()
        .eq(3).html(param.Ativo ? 'Sim' : 'Não');
}
