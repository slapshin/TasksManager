tasks.controller('admin.users.listCtrl',
    ['$scope', '$http', 'total', 'logger',
    function ($scope, $http, total, logger) {
        $scope.totalServerItems = total;
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

        $scope.setPagingData = function (data) {
            $scope.users = data;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.getPagedDataAsync = function (pageSize, page, searchText) {
            setTimeout(function () {
                var data;
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
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
            }
        }, true);
        $scope.$watch('filterOptions', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage, $scope.filterOptions.filterText);
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
                { field: 'patronymic', displayName: 'Отчетство' }
            ],
            enablePaging: true,
            showFooter: true,
            totalServerItems: 'totalServerItems',
            pagingOptions: $scope.pagingOptions,
            filterOptions: $scope.filterOptions
        };

        logger.log('creating admin.UsersCtrl');
    }]);