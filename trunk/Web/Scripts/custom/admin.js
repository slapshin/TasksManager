function Admin() {
    var CHANGE_PASSWORD_FORM = '<form id="changePasswordDialog">' +
                                    '<input id="Id" name="Id" type="hidden" value="{{id}}" />' +
                                    '<h4>Пароль</h4>' +
                                    '<div class="input-control password">' +
                                        '<input id="Password" name="Password" type="password">' +
                                    '</div>' +
                                    '<h4>Подтверждение пароля</h4>' +
                                    '<div class="input-control password">' +
                                        '<input id="ConfirmPassword" name="ConfirmPassword" type="password">' +
                                    '</div>' +
                                    '<span class="result label" style="display: none"></span>' +
                                '</form>';

    var _this = this;
    var changePasswordUrl;

    this.init = function () {
        changePasswordUrl = $('#ChangePasswordUrl').val();
        $('.change-pass').click(changePassword);        
    };

    function changePassword(e) {
        e.preventDefault();
        var options = {
            title: 'Смена пароля',
            content: CHANGE_PASSWORD_FORM.replace('{{id}}', $(this).data('id')),
            buttons: {
                'OK': {
                    'action': function () {
                        var dialog = $('#changePasswordDialog');
                        var $result = dialog.find('.result');
                        $result.hide();
                        $.ajax({
                            url: changePasswordUrl,
                            data: dialog.serialize(),
                            type: 'POST',
                            success: function (data, textStatus) {                                
                                if (data.success) {
                                    $result.removeClass('error').addClass('success').html('Пароль сменен').show();
                                    $.Dialog.close();
                                }
                                else {
                                    $result.removeClass('success').addClass('error').html('Ошибка при смене пароля: ' + data.error).show();
                                }
                            }
                        });
                    }
                },
                'Отмена': {
                    'action': function () {
                        $.Dialog.close();
                    }
                }
            }
        };
        dialogs.modalDialog(options);
    }
}

var admin = null;
$().ready(function () {
    admin = new Admin();
    admin.init();
});