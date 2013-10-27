app.controller('ctrl.login', ['$scope', '$http', '$location','$rootScope',
    function ($scope, $http, $location, $rootScope) {
        $scope.login = function () {
            $.post('/Token',
               {
                   grant_type: "password",
                   username: $scope.username,
                   password: $scope.pass
               })
               .done(function (data) {
                   if (!data.userName && !data.access_token) {
                       $scope.$apply(function () {
                           $scope.error = 'Не удалось получить ключ авторизации';
                       });
                       return;
                   }

                   if ($scope.remember) {
                       localStorage["accessToken"] = data.access_token;
                   } else {
                       sessionStorage["accessToken"] = data.access_token;
                   }
                   $http.defaults.headers.common = { 'Authorization': 'Bearer ' + data.access_token };
                   $rootScope.$broadcast('event:userLogged');
                   $scope.$apply(function () {
                       $location.path('/');
                   });
               })
               .error(function (response) {
                   $scope.$apply(function () {
                       $scope.error = response.responseJSON.error_description;
                   });
               });
        }
    }]);