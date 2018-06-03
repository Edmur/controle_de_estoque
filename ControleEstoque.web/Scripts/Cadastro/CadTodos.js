function formatar_mensagem_aviso(mensagens) {
    var template =
        '<ul>' +
        '{{ #. }}' +
        '<li>{{ . }}</li>' +
        '{{ /. }}' +
        '</ul>';
    return Mustache.render(template, mensagens);
}

function abrir_form(dados) {
    set_dados_form(dados);

    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagem_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagem_aviso').hide();
    $('#msg_erro').hide();

    bootbox.dialog({
        title: titulo_pagina,
        message: modal_cadastro
    })
    .on('shown.bs.modal', function () {
        modal_cadastro.show(0, function () {
            set_focus_form();
        });
    })
    .on('hidden.bs.modal', function () {
        modal_cadastro.hide().appendTo('body');
    });
}

function criar_linha_grid(dados) {
    var template = $('#template-grid').html();
    return Mustache.render(template, dados);
}

$(document)
    .on('click', '#btn_incluir', function () {
        abrir_form(get_dados_inclusao());
    })
    .on('click', '.btn-alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_alterar,
            param = { 'id': id };

        $.post(url, param, function (response) {
            if (response) {
                abrir_form(response);
            }
        })
        .fail(function () {
            swal(
                'Aviso',
                'Não foi possível recuperar as informações. Tente novamente em instantes.',
                'warning'
            );
        });

    })
    .on('click', '.btn-excluir', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = url_excluir,
            param = { 'id': id };

        bootbox.confirm({
            message: "Confirma a exclusão de " + titulo_pagina + "?",
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
                            var quant = $('#grid_cadastro > tbody > tr').length;
                            if (quant == 0) {
                                $('#mensagem-grid').removeClass('invisivel');
                                $('#grid_cadastro').addClass('invisivel');
                            }
                        }
                    })
                    .fail(function () {
                        swal(
                        'Aviso',
                        'Não foi possível excluir as informações. Tente novamente em instantes.',
                        'warning'
                        );
                    });
                }
            }
        });
    })
    .on('click', '#btn_confirmar', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            url = url_confirmar,
            param = get_dados_form();

        $.post(url, param, function (response) {
            if (response.Resultado == "OK") {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo,
                        table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);

                    table.append(linha);
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem-grid').addClass('invisivel');
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    preencher_linha_grid(param, linha);
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
        })
        .fail(function () {
            swal(
                'Aviso',
                'Não foi possível salvar as informações. Tente novamente em instantes.',
                'warning'
            );
        });
    })
    .on('click', '.page-item', function () {
        var btn = $(this),
            filtro = $('#txt_filtro'),
            tamPag = $('#ddl_tam_pag').val(),
            pagina = btn.text(),
            url = url_pagina,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, param, function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem-grid').addClass('invisivel');

                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#mensagem-grid').removeClass('invisivel');
                    $('#grid_cadastro').addClass('invisivel');
                }

                btn.siblings().removeClass('active');
                btn.addClass('active');
            }
        })
        .fail(function () {
            swal(
                'Aviso',
                'Não foi possível recuperar as informações. Tente novamente em instantes.',
                'warning'
            );
        });
    })
    .on('change', '#ddl_tam_pag', function () {
        var ddl = $(this),
            filtro = $('#txt_filtro'),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_pagina,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, param, function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem-grid').addClass('invisivel');

                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#mensagem-grid').removeClass('invisivel');
                    $('#grid_cadastro').addClass('invisivel');
                }

                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }
        })
        .fail(function () {
            swal(
                'Aviso',
                'Não foi possível recuperar as informações. Tente novamente em instantes.',
                'warning'
            );
        });
    })
    .on('keyup', '#txt_filtro', function () {
        var filtro = $(this),
            ddl = $('#ddl_tam_pag'),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_filtro_change,
            param = { 'pagina': pagina, 'tamPag': tamPag, 'filtro': filtro.val() };

        $.post(url, param, function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');

                table.empty();
                if (response.length > 0) {
                    $('#grid_cadastro').removeClass('invisivel');
                    $('#mensagem-grid').addClass('invisivel');

                    for (var i = 0; i < response.length; i++) {
                        table.append(criar_linha_grid(response[i]));
                    }
                }
                else {
                    $('#mensagem-grid').removeClass('invisivel');
                    $('#grid_cadastro').addClass('invisivel');
                }

                ddl.siblings().removeClass('active');
                ddl.addClass('active');
            }
        })
        .fail(function () {
            swal(
                'Aviso',
                'Não foi possível recuperar as informações. Tente novamente em instantes.',
                'warning'
            );
        });
    });