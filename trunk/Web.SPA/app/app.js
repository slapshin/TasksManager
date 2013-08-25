window.app = angular.module('tasks', [
                                        'ngGrid',
                                        'utils',
                                        'ui.bootstrap',
                                        'admin.users.services',
]);
app.value('Q', window.Q);

app.config(['$routeProvider', function ($routeProvider) {
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
                }
            }
        }).
        when('/admin/users/edit/:userId', {
            controller: 'admin.users.editCtrl',
            templateUrl: 'app/admin/views/users/edit.html',
            resolve: {
                user: ["UserLoader", function (UserLoader) {
                    return UserLoader();
                }]
            }
        }).
        when('/admin/users/view/:userId', {
            controller: 'admin.users.viewCtrl',
            resolve: {
                user: ["UserLoader", function (UserLoader) {
                    return UserLoader();
                }]
            },
            templateUrl: 'app/admin/views/users/view.html'
        }).
        when('/admin/users/new/', {
            controller: 'admin.users.newCtrl',
            templateUrl: 'app/admin/views/users/edit.html',
        }).
        // - projects
        when('/admin/projects/index', { templateUrl: 'app/admin/views/projects/index.html', controller: 'admin.ProjectsCtrl' }).
        when('/admin/projects/', { templateUrl: 'app/admin/views/projects/index.html', controller: 'admin.ProjectsCtrl' }).

        otherwise({ redirectTo: '/' });
}]);

app.directive('onFocus', function () {
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
    app.directive('placeholder', function () {
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