app.factory('logger', function () {
    var logger = {
        log: log,
        error: error,
        info: info,
        warn: warn
    };
    return logger;

    function log(message) {
        console.log(message);
    }

    function error(message) {
        alert(message);
        console.error(message);
    }

    function info(message) {
        console.info(message);
    }

    function warn(message) {
        console.warn(message);
    }
});