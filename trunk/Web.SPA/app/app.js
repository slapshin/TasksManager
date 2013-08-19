﻿/* main: startup script creates the 'todo' module and adds custom Ng directives */

// 'todo' is the one Angular (Ng) module in this app
// 'todo' module is in global namespace
window.tasks = angular.module('tasks', ['admin.users.services', 'ngGrid']);

// Add global "services" (like breeze and Q) to the Ng injector
// Learn about Angular dependency injection in this video
// http://www.youtube.com/watch?feature=player_embedded&v=1CpiB3Wk25U#t=2253s
tasks.value('breeze', window.breeze)
    .value('Q', window.Q);

// Configure routes
tasks.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.
        when('/', { templateUrl: 'app/views/home/index.html', controller: 'HomeCtrl' }).
        //admin
        // - users
        when('/admin/users/list', {
            templateUrl: 'app/admin/views/users/list.html',
            controller: 'admin.users.listCtrl',
            resolve: {
                total: function (UsersCount) {
                    return UsersCount();
                }                
            }
        }).
        when('/admin/users/', {
            templateUrl: 'app/admin/views/users/list.html',
            controller: 'admin.users.listCtrl',
            resolve: {
                total: function (UsersCount) {
                    return UsersCount();
                },
                users: function (MultiUserLoader) {
                    return MultiUserLoader();
                }
            }
        }).
        // - projects
        when('/admin/projects/index', { templateUrl: 'app/admin/views/projects/index.html', controller: 'admin.ProjectsCtrl' }).
        when('/admin/projects/', { templateUrl: 'app/admin/views/projects/index.html', controller: 'admin.ProjectsCtrl' }).

        otherwise({ redirectTo: '/' });
}]);

//#region Ng directives
/*  We extend Angular with custom data bindings written as Ng directives */
tasks.directive('onFocus', function () {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            elm.bind('focus', function () {
                scope.$apply(attrs.onFocus);
            });
        }
    };
})
    .directive('onBlur', function () {
        return {
            restrict: 'A',
            link: function (scope, elm, attrs) {
                elm.bind('blur', function () {
                    scope.$apply(attrs.onBlur);
                });
            }
        };
    })
    .directive('onEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if (event.which === 13) {
                    scope.$apply(function () {
                        scope.$eval(attrs.onEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
    .directive('selectedWhen', function () {
        return function (scope, elm, attrs) {
            scope.$watch(attrs.selectedWhen, function (shouldBeSelected) {
                if (shouldBeSelected) {
                    elm.select();
                }
            });
        };
    });
if (!Modernizr.input.placeholder) {
    // this browser does not support HTML5 placeholders
    // see http://stackoverflow.com/questions/14777841/angularjs-inputplaceholder-directive-breaking-with-ng-model
    tasks.directive('placeholder', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attr, ctrl) {
                var value;

                var placeholder = function () {
                    element.val(attr.placeholder);
                };
                var unplaceholder = function () {
                    element.val('');
                };

                scope.$watch(attr.ngModel, function (val) {
                    value = val || '';
                });

                element.bind('focus', function () {
                    if (value === '') unplaceholder();
                });

                element.bind('blur', function () {
                    if (element.val() === '') placeholder();
                });

                ctrl.$formatters.unshift(function (val) {
                    if (!val) {
                        placeholder();
                        value = '';
                        return attr.placeholder;
                    }
                    return val;
                });
            }
        };
    });
}
//#endregion 