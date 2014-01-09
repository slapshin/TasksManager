app.filter('taskPriority', ['consts', function (consts) {
    return function (input) {
        return consts.taskPriority[input].translation;
    };
}]);

app.filter('taskType', ['consts', function (consts) {
    return function (input) {
        return consts.taskType[input].translation;
    };
}]);