function Customer() {
    var RETURN_CALL_FORM = '<form id="returnCallForm">' +
                                '<input id="Id" name="id" type="hidden" value="{{id}}" />' +
                                '<div class="input-control text">' +
                                    '<textarea cols="20" id="Comment" name="comment" rows="2" class="valid" placeholder="Комментарий"></textarea>' +
                                '</div>' +
                                '<span class="result label" style="display: none"></span>' +
                            '</form>';

    var _this = this;
    var urls = {};
    var kanban_task_statuses = {
        Created: "Создано",
        Executing: "Выполняется",
        Completed: "Завершено",
        Returned: "Возвращено",
        Checked: "Проверено"
    };

    this.init = function () {
        urls.returnCall = $('#ReturnCallUrl').val();
        urls.setCallChecked = $('#SetCallCheckedUrl').val();
        urls.projectsReportUrl = $('#ProjectsReportUrl').val();

        $('.return-call').click(returnCall);
        $('.set-call-checked').click(setCallChecked);
        $('div.projects-report a').click(loadProjectsReport);
        $('div.projects-report a:first').click();
    };

    function returnCall(e) {
        e.preventDefault();
        var options = {
            title: 'Возвращение заявки',
            content: RETURN_CALL_FORM.replace('{{id}}', $(this).data('id')),
            buttons: {
                'OK': {
                    'action': function () {
                        var dialog = $('#returnCallForm');
                        var $result = dialog.find('.result');
                        $result.hide();

                        if (dialog.find('#Comment').val().trim() === '') {
                            common.errorResult($result, 'Введите комментарий');
                            return;
                        }

                        var options = {
                            url: urls.returnCall,
                            data: dialog.serialize(),
                            onError: function (error) {
                                common.errorResult($result, 'Ошибка! ' + error);
                            },
                            onSuccess: function () {
                                common.successResult($result, 'Заявка возвращена');
                                common.reloadPage();
                            }
                        };
                        common.doServerRequest(options);
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

    function setCallChecked(e) {
        e.preventDefault();
        var options = {
            url: urls.setCallChecked,
            data: { id: $(this).data('id') }
        };
        common.doServerRequest(options);
    }

    function loadProjectsReport(e) {
        var $this = $(this);
        if ($this.data('loaded')) {
            return;
        }

        var project = $this.data('id');
        var options = {
            url: urls.projectsReportUrl,
            data: { id: project },
            onSuccess: function (result) {
                var kanban = new KanbanEx();
                kanban.init({
                    target: $('.frames > #' + project),
                    statuses: kanban_task_statuses,
                    board: initBoard(result.data),
                    dataCellCreate: dataCellCreate,
                });
            }
        };
        common.doServerRequest(options);
        $this.data('loaded', true);

        function dataCellCreate(data) {
            return '<div class="ellipsis task">' +
                        '<div class="ellipsis height55">' + data.title + '</div>' +
                        '<div class="ellipsis height20 place-right"><strong>' + data.exec + '</strong></div>' +
                    '</div>';
        }
    }

    function initBoard(calls) {
        var board = {};
        board.groups = [];
        board.groups[0] = [];
        board.data = {};
        for (var i = 0, len = calls.length; i < len; i++) {
            var call = calls[i];
            board.groups[0].push({ id: call.id, childs: [], obj: call, title: '<p class="long-text"><small>Заявка №' + call.id + '</small></p>' + call.title });

            var _board = {};
            for (var j = 0, _len = call.tasks.length; j < _len; j++) {
                var task = call.tasks[j];

                if (!_board[task.status]) {
                    _board[task.status] = [];
                }
                _board[task.status].push(task);
            }
            board.data[call.id] = _board;
        }
        return board;
    }
}

var customer = null;
$().ready(function () {
    customer = new Customer();
    customer.init();
});