window.app = angular.module('tasks', ['loadingService',
                                        'ngGrid',
                                        'common',
                                        'ui.bootstrap',
                                        'services.admin.users',
                                        'services.admin.projects']);

angular.module('loadingService', [],
    ['$provide', function ($provide) {
        $provide.factory('customHttpInterceptor', ['$q', '$window', '$location', 'logger', function ($q, $window, $location, logger) {
            return function (promise) {
                return promise.then(function (response) {
                    $('.ajax-indicator').hide();
                    return response;
                }, function (response) {
                    $('.ajax-indicator').hide();
                    if (response.status === 401) {                        
                        $location.path('/login');                                                
                    }
                    else {
                        logger.error(response.data.message);
                    }                    
                    return $q.reject(response);
                });
            };
        }]);
    }]);

app.controller('main-menu-controller', ['$scope', '$http',
    function ($scope, $http) {
        $http.get('api/Helper/MainMenu')
            .success(function (data) {
             
            })
            .error(function (data) {
              
            });

        $scope.test = 'test';
    }]);

app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.responseInterceptors.push('customHttpInterceptor');
    var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];
    if (accessToken) {
        $httpProvider.defaults.headers.common = { 'Authorization': 'Bearer ' + accessToken };
    }
   
    $httpProvider.defaults.transformRequest.push(function (data, headers) {
        $('.ajax-indicator').show();
        return data;
    });
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