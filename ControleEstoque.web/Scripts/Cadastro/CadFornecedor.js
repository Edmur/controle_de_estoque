function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_cpf_cnpj').val(dados.CpfCnpj);
    $('#txt_tel_fixo').val(dados.TelefoneFixo);
    $('#txt_tel_celular_1').val(dados.TelefoneCelular1);
    $('#txt_tel_celular_2').val(dados.TelefoneCelular2);
    $('#txt_email').val(dados.Email);
    $('#txt_endereco').val(dados.Endereco);
    $('#txt_numero').val(dados.Numero);
    $('#txt_complemento').val(dados.Complemento);
    $('#txt_bairro').val(dados.Bairro);
    $('#txt_cep').val(dados.Cep);
    $('#txt_cidade').val(dados.Cidade);
    $('#txt_uf').val(dados.UF);
    $('#cbx_ativo').prop('checked', dados.Ativo);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        CpfCnpj: '',
        TelefoneFixo: '',
        TelefoneCelular1: '',
        TelefoneCelular2: '',
        Email: '',
        Endereco: '',
        Numero: 0,
        Complemento: '',
        Bairro: '',
        Cep: '',
        Cidade: '',
        UF: '',
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        CpfCnpj: $('#txt_cpf_cnpj').val(),
        TelefoneFixo: $('#txt_tel_fixo').val(),
        TelefoneCelular1: $('#txt_tel_celular_1').val(),
        TelefoneCelular2: $('#txt_tel_celular_2').val(),
        Email: $('#txt_email').val(),
        Endereco: $('#txt_endereco').val(),
        Numero: $('#txt_numero').val(),
        Complemento: $('#txt_complemento').val(),
        Bairro: $('#txt_bairro').val(),
        Cep: $('#txt_cep').val(),
        Cidade: $('#txt_cidade').val(),
        UF: $('#txt_uf').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Cpf_Cnpj).end()
        .eq(2).html(param.TelefoneCelular1).end()
        .eq(3).html(param.Email).end()
        .eq(4).html(param.Ativo ? 'Sim' : 'Não');
}

$(document).ready(function () {
    var grid = $('#grid_cadastro > tbody');
    for (var i = 0; i < linhas.length; i++) {
        grid.append(criar_linha_grid(linhas[i]));
    }
});

