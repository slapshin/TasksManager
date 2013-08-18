var services = angular.module('admin.users.services', ['ngResource']);

services.factory('User', ['$resource',
function ($resource) {
    return $resource('api/Admin/Users/:id', { id: '@id' });
}]);

services.factory('MultiUserLoader', ['User', '$q',
function (Recipe, $q) {
    return function () {
        var delay = $q.defer();
        Recipe.query(function (users) {
            delay.resolve(users);
        }, function () {
            delay.reject('Unable to fetch users');
        });
        return delay.promise;
    };
}]);

services.factory('UserLoader', ['User', '$route', '$q',
function (Recipe, $route, $q) {
    return function () {
        var delay = $q.defer();
        Recipe.get({ id: $route.current.params.userId }, function (user) {
            delay.resolve(user);
        }, function () {
            delay.reject('Unable to fetch recipe ' + $route.current.params.userId);
        });
        return delay.promise;
    };
}]);