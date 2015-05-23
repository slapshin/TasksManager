function Common() {
    var _this = this;

    this.reloadPage = function () {
        window.location.reload();
    };

    this.doServerRequest = function (options) {
        $.ajax({
            url: options.url,
            data: options.data,
            type: 'POST',
            success: function (data, textStatus) {
                if (data.success) {
                    if (options.onSuccess) {
                        options.onSuccess(data);
                    }
                    else {
                        _this.reloadPage();
                    }
                }
                else {
                    if (options.onError) {
                        options.onError(data.error);
                    }
                    else {
                        dialogs.showError(data.error);
                    }
                }
            }
        });
    };

    this.errorResult = function (element, msg) {
        element.removeClass('success').addClass('error').html(msg).show();
    };

    this.successResult = function (element, msg) {
        element.removeClass('error').addClass('success').html(msg).show();
    };
}

function Dialogs() {
    var _this = this;

    this.showError = function (message) {
        this.messageBox("Ошибка", '<span class="fg-color-red">' + message + '</span>');
    };

    this.showComment = function (message) {
        this.messageBox('Комментарий', '<pre>' + message + '</pre>');
    };

    this.messageBox = function (title, message) {
        var options = { title: title, content: message, buttons: {} };
        this.modalDialog(options);
    };

    this.ajaxModalDialog = function (options) {
        $.ajax({
            url: options.url,
            data: options.data,
            type: 'POST',
            success: function (data, textStatus) {
                _this.modalDialog({
                    title: options.title,
                    content: data,
                    buttons: options.buttons,
                });
            }
        });
    };

    this.modalDialog = function (options) {
        $.Dialog({
            'title': options.title,
            'content': options.content,
            'draggable': true,
            'overlay': true,
            'closeButton': true,
            'buttonsAlign': 'center',
            'keepOpened': true,
            'position': {
                'zone': 'center'
            },
            'buttons': options.buttons
        });
    };
}

function Handlers() {
    var _this = this;

    this.init = function () {
        $('[data-comment]').click(function (e) {
            e.preventDefault();
            dialogs.showComment($(this).data('comment'));
        });
        $('[data-confirm]').click(function (e) {
            var question = $(this).data('confirm');
            if ((question) && (!confirm(question))) {
                e.preventDefault();
            }
        });

        var $ajaxIndicator = $('.ajax-indicator');
        $ajaxIndicator.hide();
        $(document).ajaxSend(function () {
            $ajaxIndicator.show();
        }).ajaxComplete(function () {
            $ajaxIndicator.hide();
        });

        addMouseWheel();

        $('.custom-calendar').each(function () {
            var $this = $(this);
            var $input = $this.find('input');

            $(this).data('param-lang', 'ru')
                    .data('param-initDate', $input.attr('value'))
                    .customDatepicker();
        });
    };

    function addMouseWheel() {
        $("body").mousewheel(function (event, delta) {
            var scroll_value = delta * 50;
            $(document).scrollTop($(document).scrollTop() - scroll_value);
            return false;
        });
    }
}

function Kanban() {

}

Kanban.prototype.init = function (options) {
    var table = $('<div id=\"board\"></div>');

    $.each(options.states, function (key, value) {
        table.append(createColumn(options, key, value));
    });

    options.target.html(table);
    options.target.find('.kanban-stories-list').dragsort({
        dragBetween: true,
        dragSelector: "div",
        placeHolderTemplate: "<li class='kanban-place-holder'><div></div></li>",
        dragEnd: options.dragEnd
    });

    function createColumn(options, status, headline) {
        return $('<div class="kanban-state-column"></div>')
                .append($('<h3>' + headline + '</h3>'))
                .append(createList(options, status));
    }

    function createList(options, status) {
        var list = $('<ul class="kanban-stories-list unstyled" data-state="' + status + '"></ul>');
        var stories = options.board[status];
        if (stories) {
            for (var i = 0, len = stories.length; i < len; i++) {
                list.append('<li>' + options.cellCreate(stories[i]) + '</li>');
            }
        }
        return list;
    }
};

function KanbanEx() {

}

KanbanEx.prototype.init = function (options) {
    var table = $('<table></table>');
    var head = '';

    for (var i = 0, len = options.board.groups.length; i < len; i++) {
        head += '<th></th>';
    }

    $.each(options.statuses, function (key, value) {
        head += '<th>' + value + '</th>';
    });

    table.append('<thead><tr>' + head + '</tr><thead>');
    var body = $('<tbody></tbody>');

    calculateRowspans(options.board.groups);

    var rows = [];
    addGroups(options.board.groups[0], rows);
    body.append(rows);
    table.append(body);
    options.target.html(table);

    function addGroups(groups, rows) {
        var _rows = [];
        for (var i = 0, len = groups.length; i < len; i++) {
            var group = groups[i];
            if (i >= rows.length) {
                rows.push($('<tr></tr>'));
            }
            var row = rows[i];
            if (group.rowspan) {
                row.append('<td rowspan=' + group.rowspan + '>' + group.title + '</td>');
            }
            else {
                row.append('<td>' + group.title + '</td>');
            }

            if (group.childs.length > 0) {
                addGroups(group.childs, _rows);
            }
            else {
                addData(options.board.data[group.id], row);
            }
        }
    }

    function addData(data, row) {
        $.each(options.statuses, function (key, value) {
            var stories = data[key];
            var td = $('<td></td>');
            if (stories) {
                for (var i = 0, len = stories.length; i < len; i++) {
                    td.append(options.dataCellCreate(stories[i]));
                }
            }
            row.append(td);
        });

        var stories = options.board[status];
        if (stories) {
            for (var i = 0, len = stories.length; i < len; i++) {
                list.append('<li>' + options.cellCreate(stories[i]) + '</li>');
            }
        }
    }

    function calculateRowspans(groups) {
        for (var i = groups.length - 1; i >= 0; i--) {
            var group = groups[i];
            for (var j = 0, len = group.length; j < len; j++) {
                var _group = group[j];
                if (!_group.parent) {
                    continue;
                }

                if (!_group.rowspan) {
                    _group.rowspan = 1;
                }

                if (!_group.parent.rowspan) {
                    _group.parent.rowspan = 0;
                }
                _group.parent.rowspan += _group.rowspan;
            }
        }
    }
};

var common = null;
var dialogs = null;
var handlers = null;

$().ready(function () {
    common = new Common();

    dialogs = new Dialogs();

    handlers = new Handlers();
    handlers.init();
});