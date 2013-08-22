tasks.controller('admin.users.listCtrl',
    ['$scope', '$http', 'total', 'logger', '$location',
    function ($scope, $http, total, logger, $location) {
        $scope.totalServerItems = total;
        $scope.user;

        logger.log('total ' + total);

        $scope.filterOptions = {
            filterText: "",
            useExternalFilter: true
        };

        $scope.pagingOptions = {
            pageSizes: TASKS_PAGE_SIZES,
            pageSize: TASKS_GRID_ROW_COUNT,
            currentPage: 1
        };

        $scope.refreshGrid = function (full) {
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
        }

        $scope.setPagingData = function (data) {
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
            columnDefs: [
                { cellTemplate: '<span type="button" class="btn btn-default btn-xs"><span class="glyphicon glyphicon-star"></span></span>', width: '40px' },
                { field: 'login', displayName: 'Логин' },
                { field: 'email', displayName: 'Email' },
                { field: 'name', displayName: 'Имя' },
                { field: 'surname', displayName: 'Фамилия' },
                { field: 'patronymic', displayName: 'Отчество' }
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

        $scope.remove = function () {
            $http.delete('api/Admin/Users/' + $scope.user.id)
                .success(function (data) {
                    $scope.totalServerItems--;
                    $scope.refreshGrid();
                })
                .error(function (data) {
                    logger.error('delete error: ' + data);
                });
        }

        $scope.view = function () {
            $location.path('/admin/users/view/' + $scope.user.id);
        }
    }]);

tasks.controller('admin.users.viewCtrl',
    ['$scope', 'user', 'userUtils', '$location',
    function ($scope, user, userUtils, $location) {
        $scope.user = user;
        $scope.roles = userUtils.userRolesStr(user);

        $scope.edit = function () {
            $location.path('/admin/users/edit/' + $scope.user.id);
        }
    }]);

tasks.controller('admin.users.editCtrl', ['$scope', 'user', '$location',
    function ($scope, user, $location) {
        $scope.user = user;

        $scope.save = function () {
            $scope.user.$save(function (user) {
                $location.path('/admin/users/view/' + user.id);
            });
        }

        $scope.remove = function () {
            $http.delete('api/Admin/Users/' + $scope.user.id)
               .success(function (data) {
                   delete $scope.user;
                   $location.path('/admin/users/');
               })
               .error(function (data) {
                   logger.error('delete error: ' + data);
               });
        }
    }]);