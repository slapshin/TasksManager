var services = angular.module('services.admin.projects', ['ngResource']);

services.factory('Project', ['$resource',
    function ($resource) {
        return $resource('api/Admin/Projects/:id', { id: '@id' });
    }]);

services.factory('MultiProjectLoader', ['Project', '$q',
    function (Project, $q) {
        return function () {
            var delay = $q.defer();
            Project.query(function (projects) {
                delay.resolve(projects);
            }, function () {
                delay.reject('Unable to fetch projects');
            });
            return delay.promise;
        };
    }]);

services.factory('ProjectLoader', ['Project', '$route', '$q',
function (Project, $route, $q) {
    return function () {
        var delay = $q.defer();
        Project.get({ id: $route.current.params.projectId }, function (project) {
            delay.resolve(project);
        }, function () {
            delay.reject('Unable to fetch project ' + $route.current.params.projectId);
        });
        return delay.promise;
    };
}]);

services.factory('ProjectsCount', ['$http', '$q', function ($http, $q) {
    return function () {
        var delay = $q.defer();
        $http.get('api/Admin/Projects/Count')
        .success(function (data) {
            delay.resolve(data);
        })
        .error(function () {
            delay.reject('ProjectsCount error');
        });
        return delay.promise;
    };
}]);

services.factory('ProjectMasters', ['$http', '$q', function ($http, $q) {
    return function () {
        var delay = $q.defer();
        $http.get('api/Admin/Projects/Masters')
        .success(function (data) {
            delay.resolve(data);
        })
        .error(function () {
            delay.reject('Getting masters error');
        });
        return delay.promise;
    };
}]);