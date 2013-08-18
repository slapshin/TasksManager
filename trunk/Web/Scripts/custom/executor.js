function Executor() {
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
        initUrls();

        $('.set-status').change(onSetStatusChange);

        createKanbanBoard();
    };

    function initUrls() {
        urls.setStatus = $('#SetStatusUrl').val();
        urls.tasks = $('#TasksUrl').val();
    }

    function onSetStatusChange(e) {
        var $this = $(this);
        $.ajax({
            url: urls.setStatus,
            data: {
                id: $this.data('id'),
                status: $this.val()
            },
            type: 'POST',
            success: function (data, textStatus) {
                if (!data.success) {
                    dialogs.showError(data.error);
                    common.reloadPage();
                }
            }
        });
    }

    function createKanbanBoard() {
        var target = $('#TasksKanbanBoard');
        if (target.length === 0) {
            return;
        }

        var options = {
            url: urls.tasks,
            data: {},
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
                    url: urls.setStatus,
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
                        '<div class="ellipsis height20 place-right"><strong>' + story.project + '</strong></div>' +
                   '</div>';
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
    }
}

var executor = null;
$().ready(function () {
    executor = new Executor();
    executor.init();
});