app.controller('main-menu-controller', ['$scope', '$http', '$location',
    function ($scope, $http, $location) {
        updateUserView();
        $scope.$on('event:userLogged', function () {
            updateUserView();
        });

        $scope.logout = function () {
            $http.post('api/Account/Logout').success(function (data) {
                $scope.user = null;
                $scope.menu = null;
                localStorage.removeItem("accessToken");
                sessionStorage.removeItem("accessToken");
                $http.defaults.headers.common = { 'Authorization': '' };
                $location.path('/login');
            });
        }

        function updateUserView() {
            $http.get('api/Account/UserInfo').success(function (data) {
                $scope.user = data.userName;
                $scope.menu = [];
                for (var i = 0, len = data.roles.length; i < len; i++) {
                    switch (data.roles[i]) {
                        case 'Admin': {
                            $scope.menu.push({
                                title: 'Администрирование',
                                items: [
                                    { title: 'Пользователи', href: '#/admin/users/list' },
                                    { title: 'Проекты', href: '#/admin/projects/list' }
                                ]
                            })
                            break;
                        }

                        case 'Customer': {
                            $scope.menu.push({
                                title: 'Заказчик',
                                items: [
                                    { title: 'Требования', href: '#/customer/claims/list' }                                    
                                ]
                            })
                            break;
                        }
                    }
                }
            });
        }
    }]);