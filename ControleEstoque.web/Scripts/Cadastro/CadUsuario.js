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
        Ativo: $('#cbx_ativo').prop('checked'),
        idPerfis: get_lista_perfis_marcados()
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Login).end()
        .eq(1).html(param.Nome).end()
        .eq(2).html(param.Email).end()
        .eq(3).html(param.Ativo ? 'Sim' : 'Não');
}

function get_lista_perfis_marcados() {
    var ids = [],
        lista_perfil = $('#lista_perfil');

    lista_perfil.find('input[type=checkbox]').each(function (index, input) {
        var cbx = $(input),
            marcado = cbx.is(':checked');

        if (marcado) {
            ids.push(parseInt(cbx.attr('data-id-perfil')));
        }
    });

    return ids;
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});

