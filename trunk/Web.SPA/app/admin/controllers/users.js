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
    }]);