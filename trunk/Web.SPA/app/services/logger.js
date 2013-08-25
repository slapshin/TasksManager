app.factory('logger', function () {
    var logger = {
        log: log,
        error: error
    };
    return logger;

    function log(message) {
        console.log(message);
    }

    function error(message) {
        console.error(message);
    }
});