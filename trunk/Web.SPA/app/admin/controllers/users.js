tasks.controller('admin.users.listCtrl',
    ['$scope', 'users', 'total', 'logger',
    function ($scope, users, total, logger) {
        $scope.server_users = users;
        logger.log('total ' + total);

        $scope.filterOptions = {
            filterText: "",
            useExternalFilter: true
        };
        $scope.totalServerItems = 0;
        $scope.pagingOptions = {
            pageSizes: TASKS_PAGE_SIZES,
            pageSize: TASKS_GRID_ROW_COUNT,
            currentPage: 1
        };

        $scope.setPagingData = function (data, page, pageSize) {
            var pagedData = data.slice((page - 1) * pageSize, page * pageSize);
            $scope.users = pagedData;
            $scope.totalServerItems = data.length;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };
        $scope.getPagedDataAsync = function (pageSize, page, searchText) {
            $scope.setPagingData(users, page, pageSize);
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