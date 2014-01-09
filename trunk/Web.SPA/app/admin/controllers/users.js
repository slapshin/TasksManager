app.controller('admin.users.list', ['$scope', '$http', 'logger', '$location', 'consts', 'utils', 'userUtils',
function ($scope, $http, logger, $location, consts, utils, userUtils) {
    var CHANGE_PASSWORD_FORM = '<form role="form">' +
                                    '<div class="form-group">' +
                                        '<label for="password">Пароль</label>' +
                                        '<input type="password" class="form-control" id="password" placeholder="Введите пароль" ng-model="password"/>' +
                                    '</div>' +
                                    '<div class="form-group">' +
                                        '<label for="confirmPassword">Подтверждение пароля</label>' +
                                        '<input type="password" class="form-control" id="confirmPassword" placeholder="Подтвердите пароль" ng-model="confirmPassword"/>' +
                                    '</div>' +
                                '</form>';
    utils.createGrid(
            {
                $scope: $scope,
                pageUrl: 'api/Admin/Users/Page',
                onSetPagingData: function (data) {
                    for (var i = 0, len = data.length; i < len; i++) {
                        data[i].roles = userUtils.userRolesStr(data[i]);
                    }
                    $scope.user = undefined;
                },
                afterSelectionChange: function (rowItem, event) {
                    $scope.user = rowItem.entity;
                },
                columnDefs:
                    [
                        { button: { onClick: 'remove', icon: 'trash' }, width: 40 },
                        { button: { onClick: 'view', icon: 'eye-open' }, width: 40 },
                        { button: { onClick: 'edit', icon: 'edit' }, width: 40 },
                        { button: { onClick: 'changePassword', icon: 'lock' }, width: 40 },
                        { field: 'login', displayName: 'Логин' },
                        { field: 'email', displayName: 'Email' },
                        { field: 'name', displayName: 'Имя' },
                        { field: 'surname', displayName: 'Фамилия' },
                        { field: 'patronymic', displayName: 'Отчество' },
                        { field: 'roles', displayName: 'Роли' }
                    ]
            }
    );

    $scope.remove = function (row) {
        utils.confirmDelete(function () {
            var user = row ? row.entity : $scope.user;
            $http.delete('api/Admin/Users/' + user.id)
                .success(function (data) {
                    delete user;
                    $scope.getPagedDataAsync();
                });
        });
    }

    $scope.new = function () {
        $location.path('/admin/users/new');
    }

    $scope.view = function (row) {
        var userId = row ? row.entity.id : $scope.user.id;
        $location.path('/admin/users/view/' + userId);
    }

    $scope.edit = function (row) {
        var userId = row ? row.entity.id : $scope.user.id;
        $location.path('/admin/users/edit/' + userId);
    }

    $scope.changePassword = function (row) {
        var userId = row ? row.entity.id : $scope.user.id;
        utils.show({
            title: 'Смена пароля',
            content: CHANGE_PASSWORD_FORM,
            init: function (scope) {
                scope.id = userId
            },
            buttons: [
                {
                    label: 'Сохранить',
                    onClick: function (scope) {
                        scope.error = '';
                        scope.success = '';
                        $http.post('api/Admin/Users/ChangePassword',
                            {
                                id: userId,
                                password: scope.password,
                                confirmPassword: scope.confirmPassword
                            })
                        .success(function (data) {
                            scope.success = 'Пароль сменен';
                            scope.dialog.close();
                        })
                        .error(function (data) {
                            scope.error = data.message;
                        });
                    }
                },
                {
                    label: 'Отмена',
                    cssClass: 'btn-primary',
                    onClick: function (scope) {
                        scope.dialog.close();
                    }
                }]
        })
    }
}]);

app.controller('admin.users.view',
    ['$scope', 'user', 'userUtils', '$location',
    function ($scope, user, userUtils, $location) {
        $scope.user = user;
        $scope.roles = userUtils.userRolesStr(user);

        $scope.edit = function () {
            $location.path('/admin/users/edit/' + $scope.user.id);
        }
    }]);

app.controller('admin.users.edit', ['$scope', 'user', '$location', '$http', 'utils', 'logger',
        function ($scope, user, $location, $http, utils, logger) {
            $scope.user = user;
            $scope.save = function () {
                $scope.user.$save(function (user) {
                    $location.path('/admin/users/view/' + user.id);
                });
            }

            $scope.remove = function () {
                utils.confirmDelete(function () {
                    $http.delete('api/Admin/Users/' + $scope.user.id)
                        .success(function (data) {
                            delete $scope.user;
                            $location.path('/admin/users/');
                        });
                });
            };
        }]);

app.controller('admin.users.new', ['$scope', 'User', '$location', 'logger',
    function ($scope, User, $location, logger) {
        $scope.user = new User();
        $scope.save = function () {
            $scope.user.$save(function (user) {
                $location.path('/admin/users/view/' + user.id);
            });
        }
    }]);