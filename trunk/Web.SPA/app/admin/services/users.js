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

services.factory('UsersCount', ['$http', '$q', function ($http, $q) {
    return function () {
        var delay = $q.defer();
        $http.get('api/Admin/Users/Count')
        .success(function (data) {
            delay.resolve(data);
        })
        .error(function () {
            delay.reject('UsersCount error');
        });
        return delay.promise;
    };
}]);

services.factory('userUtils', function () {
    var utils = {
        userRolesStr: userRolesStr,
    };
    return utils;
    function userRolesStr(user) {
        var str = '';
        var delim = '';
        if (user.isAdmin) {
            str += delim + 'Администратор';
            delim = ', ';
        }

        if (user.isMaster) {
            str += delim + 'Мастер';
            delim = ', ';
        }

        if (user.isCustomer) {
            str += delim + 'Заказчик';
            delim = ', ';
        }

        if (user.isExecutor) {
            str += delim + 'Исполнитель';
            delim = ', ';
        }

        if (user.isRouter) {
            str += delim + 'Роутер';
            delim = ', ';
        }

        if (user.isTester) {
            str += delim + 'Тестировщик';
            delim = ', ';
        }
        return str;
    };
});