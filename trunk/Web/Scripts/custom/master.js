function Master() {
    var _this = this;
    var urls = {};
    var projectId;
    var callId;

    var kanban_call_statuses = {
        Created: "Создано",
        Completed: "Завершено",
        Returned: "Возвращено",
        Checked: "Проверено"
    };

    var kanban_task_statuses = {
        Created: "Создано",
        Executing: "Выполняется",
        Completed: "Завершено",
        Returned: "Возвращено",
        Checked: "Проверено"
    };

    this.init = function () {
        initUrls();

        $('.add-exec').click(addExec);
        $('.remove-exec').click(removeExec);
        $('.create-call').click(createCall);
        $('.add-observer').click(addObserver);
        $('.remove-observer').click(removeObserver);

        createKanbanBoards();
    };

    function initUrls() {
        projectId = $('#ProjectId').val();
        callId = $('#CallId').val();

        urls.createCall = $('#CreateCallUrl').val();
        urls.removeExecutor = $('#RemoveExecutorUrl').val();
        urls.availableExecutors = $('#AvailableExecutorsUrl').val();
        urls.addExecutor = $('#AddExecutorUrl').val();
        urls.projectCalls = $('#ProjectCallsUrl').val();
        urls.setCallStatus = $('#SetCallStatusUrl').val();
        urls.callTasks = $('#CallTasksUrl').val();
        urls.setTaskStatus = $('#SetTaskStatusUrl').val();
        urls.removeObserver = $('#RemoveObserverUrl').val();
        urls.availableObservers = $('#AvailableObserversUrl').val();
        urls.addObserver = $('#AddObserverUrl').val();
    }

    function createKanbanBoards() {
        createCallsKanbanBoard();
        createTasksKanbanBoard();
    }

    function createCallsKanbanBoard() {
        var target = $('#CallsKanbanBoard');
        if (target.length === 0) {
            return;
        }

        var options = {
            url: urls.projectCalls,
            data: { id: projectId },
            onSuccess: function (result) {
                var kanban = new Kanban();
                kanban.init({
                    target: target,
                    states: kanban_call_statuses,
                    board: initBoard(result.data),
                    dragEnd: dragEnd,
                    cellCreate: cellCreate
                });
            }
        };
        common.doServerRequest(options);

        function dragEnd() {
            var id = this.find('[data-id]').data('id');
            var status = this.parent().data('state');
            if ((status) && (id)) {
                var options = {
                    url: urls.setCallStatus,
                    data: { id: id, status: status },
                    onError: function () {
                        common.reloadPage();
                    },
                    onSuccess: function () {
                        //do nothing
                    }
                };
                common.doServerRequest(options);
            }
        }

        function cellCreate(story) {
            return '<div data-id="' + story.id + '">' +
                        '<div class="ellipsis height70"><b>Заявка №' + story.id + '</b>: ' + story.title + '</div>' +
                   '</div>';
        }
    }

    function createTasksKanbanBoard() {
        var target = $('#TasksKanbanBoard');
        if (target.length === 0) {
            return;
        }

        var options = {
            url: urls.callTasks,
            data: { id: callId },
            onSuccess: function (result) {
                var kanban = new Kanban();
                kanban.init({
                    target: target,
                    states: kanban_task_statuses,
                    board: initBoard(result.data),
                    dragEnd: dragEnd,
                    cellCreate: cellCreate
                });
            }
        };
        common.doServerRequest(options);

        function dragEnd() {
            var id = this.find('[data-id]').data('id');
            var status = this.parent().data('state');
            if ((status) && (id)) {
                var options = {
                    url: urls.setTaskStatus,
                    data: { id: id, status: status },
                    onError: function () {
                        common.reloadPage();
                    },
                    onSuccess: function () {
                        //do nothing
                    }
                };
                common.doServerRequest(options);
            }
        }

        function cellCreate(story) {
            return '<div data-id="' + story.id + '">' +
                        '<div class="ellipsis height55">' + story.title + '</div>' +
                        '<div class="ellipsis height20 place-right"><strong>' + story.exec + '</strong></div>' +
                   '</div>';
        }
    }

    function initBoard(stories) {
        var board = {};
        for (var i = 0, len = stories.length; i < len; i++) {
            var story = stories[i];
            if (!board[story.status]) {
                board[story.status] = [];
            }
            board[story.status].push(story);
        }
        return board;
    }

    function addExec() {
        var $this = $(this);
        var options = {
            url: urls.availableExecutors,
            data: { id: projectId },
            title: 'Добавление исполнителя',
            buttons: {
                'OK': {
                    'action': function () {
                        var dialog = $('.add-user-dialog');
                        var $result = dialog.find('.result');
                        $result.hide();

                        var exec = $('.exec-select').val();
                        if (exec) {
                            common.doServerRequest({
                                url: urls.addExecutor,
                                data: { id: projectId, exec: exec },
                                onError: function (error) {
                                    common.errorResult($result, error);
                                },
                                onSuccess: function () {
                                    common.successResult($result, "Успешно");
                                    common.reloadPage();
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

    function removeExec(e) {
        e.preventDefault();
        var exec = $(this).data('id');
        if (exec) {
            common.doServerRequest({ url: urls.removeExecutor, data: { id: projectId, exec: exec } });
        }
    }

    function addObserver() {
        var $this = $(this);
        var options = {
            url: urls.availableObservers,
            data: { id: projectId },
            title: 'Добавление наблюдателя',
            buttons: {
                'OK': {
                    'action': function () {
                        var dialog = $('.add-user-dialog');
                        var $result = dialog.find('.result');
                        $result.hide();

                        var observer = $('.observer-select').val();
                        if (observer) {
                            common.doServerRequest({
                                url: urls.addObserver,
                                data: { id: projectId, observer: observer },
                                onError: function (error) {
                                    common.errorResult($result, error);
                                },
                                onSuccess: function () {
                                    common.successResult($result, "Успешно");
                                    common.reloadPage();
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

    function removeObserver(e) {
        e.preventDefault();
        var observer = $(this).data('id');
        if (observer) {
            common.doServerRequest({ url: urls.removeObserver, data: { id: projectId, observer: observer } });
        }
    }

    function createCall(e) {
        e.preventDefault();
        var claim = $(this).data('id');
        if (claim) {
            common.doServerRequest({ url: urls.createCall, data: { id: claim } });
        }
    }
}

var master = null;

$().ready(function () {
    master = new Master();
    master.init();
});