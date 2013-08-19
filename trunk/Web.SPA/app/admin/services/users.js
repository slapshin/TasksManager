var services = angular.module('admin.users.services', ['ngResource']);

services.factory('User', ['$resource',
function ($resource) {
    return $resource('api/Admin/Users/:id', { id: '@id' });
}]);

services.factory('MultiUserLoader', ['User', '$q',
function (User, $q) {
    return function () {
        var delay = $q.defer();
        User.query(function (users) {
            delay.resolve(users);
        }, function () {
            delay.reject('Unable to fetch users');
        });
        return delay.promise;
    };
}]);

services.factory('UserLoader', ['User', '$route', '$q',
function (User, $route, $q) {
    return function () {
        var delay = $q.defer();
        User.get({ id: $route.current.params.userId }, function (user) {
            delay.resolve(user);
        }, function () {
            delay.reject('Unable to fetch user ' + $route.current.params.userId);
        });
        return delay.promise;
    };
}]);

services.factory('UsersCount', ['$http', function ($http) {
    return function () {
        var count = 0;
        $http.get('api/Admin/Users/UsersCount')
        .success(function (data) {
            count = data;
        })
        .error(function (data, status, headers, config) {
            var t = 8;
        });
        return count;
    };
}]);