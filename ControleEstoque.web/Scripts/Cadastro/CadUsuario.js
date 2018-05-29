function abrir_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_login').val(dados.Login);
    $('#txt_senha').val(dados.Senha);
    $('#txt_nome').val(dados.Nome);
    $('#txt_email').val(dados.Email);
    $('#cbx_ativo').prop('checked', dados.Ativo);

    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: 'Cadastro de ' + titulo_pagina,
        message: modal_cadastro
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                $('#txt_login').focus();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        '<td class="detalhe-left">' + dados.Login + '</td>' +
        '<td class="detalhe-left">' + dados.Nome + '</td>' +
        '<td class="detalhe-left">' + dados.Email + '</td>' +
        '<td class="detalhe-center">' + (dados.Ativo ? 'Sim' : 'Não') + '</td>' +
        '<td>' +
        '<a class="btn-pequeno btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
        '<a class="btn-pequeno btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}

$(document).on('click', '#btn_incluir', function () {
    abrir_form({ Id: 0, Login: '', Senha: '', Nome: '', Email: '', Ativo: true });
})
    .on('click', '.btn-alterar', function () {
        var btn = $(this);
        var id = btn.closest('tr').attr('data-id');
        var url = url_alterar;
        var param = { 'id': id };

        $.post(url, param, function (response) {
            if (response) {
                abrir_form(response);
            }
        });

    })
    .on('click', '.btn-excluir', function () {
        var btn = $(this);
        var tr = btn.closest('tr');
        var id = tr.attr('data-id');
        var url = url_excluir;
        var param = { 'id': id };

        bootbox.confirm({
            message: "Confirma a exclusão do Usuário?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {

                    $.post(url, param, function (response) {
                        if (response) {
                            tr.remove();
                        }
                    });
                }
            }
        });
    })
    .on('click', '#btn_confirmar', function () {
        var btn = $(this);
        var tr = btn.closest('tr');
        var url = url_confirmar;
        var param = {
            Id: $('#id_cadastro').val(),
            Login: $('#txt_login').val(),
            Senha: $('#txt_senha').val(),
            Nome: $('#txt_nome').val(),
            Email: $('#txt_email').val(),
            Ativo: $('#cbx_ativo').prop('checked')
        };

        $.post(url, param, function (response) {
            if (response.Resultado == "OK") {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody')
                    var linha = criar_linha_grid(param);
                    table.append(linha);
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    linha
                        .eq(0).html(param.Login).end()
                        .eq(1).html(param.Nome).end()
                        .eq(2).html(param.Email).end()
                        .eq(3).html(param.Ativo ? 'Sim' : 'Não');
                }

                $('#modal_cadastro').parents('.bootbox').modal('hide');
            }
            else if (response.Resultado == "ERRO") {
                $('#msg_aviso').hide();
                $('#msg_mensagem_aviso').hide();
                $('#msg_erro').show();
            }
            else if (response.Resultado == "AVISO") {
                $('#msg_mensagem_aviso').html(formatar_mensagem_aviso(response.Mensagens));
                $('#msg_aviso').show();
                $('#msg_mensagem_aviso').show();
                $('#msg_erro').hide();
            }
        });
    });

function formatar_mensagem_aviso(mensagens) {
    var ret = '';

    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }

    return '<ul>' + ret + '</ul>';
}
