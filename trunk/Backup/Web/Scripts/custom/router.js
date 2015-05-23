function Router() {
    var _this = this;
    var projectstUrl;
    var assignProjectUrl;

    this.init = function () {
        projectstUrl = $('#ProjectstUrl').val();
        assignProjectUrl = $('#AssignProjectUrl').val();
        $('.assign-project').click(setProject);
    };

    function setProject(e) {
        e.preventDefault();
        var $this = $(this);
        var options = {
            url: projectstUrl,
            data: {},
            title: 'Назначение проекта',
            buttons: {
                'OK': {
                    'action': function () {
                        var dialog = $('.assign-project-dialog');
                        var $result = dialog.find('.result');
                        $result.hide();

                        var project = dialog.find('.project-select').val();
                        if (project) {
                            $.ajax({
                                url: assignProjectUrl,
                                data: { claim: $this.data('id'), project: project },
                                type: 'POST',
                                success: function (data, textStatus) {
                                    if (data.success) {
                                        $result.removeClass('error').addClass('success').html('Успешно').show();
                                        common.reloadPage();
                                    }
                                    else {
                                        $result.removeClass('success').addClass('error').html('Ошибка: ' + data.error).show();                                        
                                    }
                                }
                            });
                        }
                        else {
                            $.Dialog.close();
                        }
                    }
                },
                'Отмена': {
                    'action': function () {
                        $.Dialog.close();
                    }
                }
            }
        };
        dialogs.ajaxModalDialog(options);
    }
}

var router = null;
$().ready(function () {
    router = new Router();
    router.init();
});