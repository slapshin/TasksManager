app.factory('main-menu', ['$http', '$q', function ($http, $q) {
    return function () {
        var delay = $q.defer();
        $http.get('api/Helper/MainMenu')
        .success(function (data) {
            delay.resolve(data);
        })
        .error(function (data) {
            delay.reject('Error: "main-menu"');
        });
        return delay.promise;
    };
}]);