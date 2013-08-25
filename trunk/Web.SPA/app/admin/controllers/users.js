app.controller('admin.users.listCtrl', ['$scope', '$http', 'total', 'logger', '$location', 'consts', 'dialogs', 'userUtils',
function ($scope, $http, total, logger, $location, consts, dialogs, userUtils) {
    $scope.totalServerItems = total;
    $scope.user;

    logger.log('total ' + total);

    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };

    $scope.pagingOptions = {
        pageSizes: consts.pageSizes,
        pageSize: consts.pageSize,
        currentPage: 1
    };

    $scope.refreshGrid = function (full) {
        $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
    }

    $scope.setPagingData = function (data) {
        for (var i = 0, len = data.length; i < len; i++) {
            data[i].roles = userUtils.userRolesStr(data[i]);
        }
        $scope.users = data;
        $scope.user = undefined;
        if (!$scope.$$phase) {
            $scope.$apply();
        }
    };
    $scope.getPagedDataAsync = function (pageSize, page, searchText) {
        setTimeout(function () {
            $http.get('api/Admin/Users/Page',
                {
                    params:
                      {
                          page: page,
                          pageSize: pageSize
                      }
                }).success(function (data) {
                    $scope.setPagingData(data);
                });
        }, 100);
    };

    $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);

    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
            $scope.refreshGrid();
        }
    }, true);
    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.refreshGrid();
        }
    }, true);

    $scope.gridOptions = {
        data: 'users',
        multiSelect: false,
        enableColumnResize: true,
        showColumnMenu: true,
        columnDefs: [
            { cellTemplate: '<a href="" class="btn btn-small btn-link" ng-click="remove(row)"><i class="glyphicon glyphicon-trash" /></a>', width: '40px' },
            { cellTemplate: '<a href="" class="btn btn-small btn-link" ng-click="view(row)"><i class="glyphicon glyphicon-eye-open" /></a>', width: '40px' },
            { cellTemplate: '<a href="" class="btn btn-small btn-link" ng-click="edit(row)"><i class="glyphicon glyphicon-edit" /></a>', width: '40px' },
            { cellTemplate: '<a href="" class="btn btn-small btn-link" ng-click="changePassword(row)"><i class="glyphicon glyphicon-lock" /></a>', width: '40px' },
            { field: 'login', displayName: 'Логин' },
            { field: 'email', displayName: 'Email' },
            { field: 'name', displayName: 'Имя' },
            { field: 'surname', displayName: 'Фамилия' },
            { field: 'patronymic', displayName: 'Отчество' },
            { field: 'roles', displayName: 'Роли' }
        ],
        enablePaging: true,
        showFooter: true,
        totalServerItems: 'totalServerItems',
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions,
        afterSelectionChange: function (rowItem, event) {
            $scope.user = rowItem.entity;
        }
    };

    $scope.remove = function (row) {
        dialogs.confirmDelete(function () {
            var user = row ? row.entity : $scope.user;
            $http.delete('api/Admin/Users/' + user.id)
                .success(function (data) {
                    delete user;
                    $scope.totalServerItems--;
                    $scope.refreshGrid();
                })
                .error(function (data) {
                    logger.error('delete error: ' + data);
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
        alert("Not implemented");
    }
}]);

app.controller('admin.users.viewCtrl',
    ['$scope', 'user', 'userUtils', '$location',
    function ($scope, user, userUtils, $location) {
        $scope.user = user;
        $scope.roles = userUtils.userRolesStr(user);

        $scope.edit = function () {
            $location.path('/admin/users/edit/' + $scope.user.id);
        }
    }]);

app.controller('admin.users.editCtrl', ['$scope', 'user', '$location', '$http', 'dialogs',
        function ($scope, user, $location, $http, dialogs) {
            $scope.user = user;
            $scope.save = function () {
                $scope.user.$save(function (user) {
                    $location.path('/admin/users/view/' + user.id);
                });
            }

            $scope.remove = function () {
                dialogs.confirmDelete(function () {
                    $http.delete('api/Admin/Users/' + $scope.user.id)
                        .success(function (data) {
                            delete $scope.user;
                            $location.path('/admin/users/');
                        })
                        .error(function (data) {
                            logger.error('delete error: ' + data);
                        });
                });
            };
        }]);

app.controller('admin.users.newCtrl', ['$scope', 'User', '$location',
    function ($scope, User, $location) {
        $scope.user = new User();
        $scope.save = function () {
            $scope.user.$save(function (user) {
                $location.path('/admin/users/view/' + user.id);
            });
        }
    }]);