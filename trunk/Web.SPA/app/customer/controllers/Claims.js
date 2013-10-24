app.controller('customer.claims.list', ['$scope', '$http', 'logger', '$location', 'consts', 'utils',
    function ($scope, $http, logger, $location, consts, utils) {
        utils.createGrid(
            {
                $scope: $scope,
                pageUrl: 'api/Customer/Claims/Page',
                onSetPagingData: function (data) {
                    $scope.claim = undefined;
                },
                afterSelectionChange: function (rowItem, event) {
                    $scope.claim = rowItem.entity;
                },
                columnDefs:
                    [
                        { button: { onClick: 'remove', icon: 'trash' }, width: 40 },
                        { button: { onClick: 'view', icon: 'eye-open' }, width: 40 },
                        { button: { onClick: 'edit', icon: 'edit' }, width: 40 },
                        { field: 'title', displayName: 'Название' },
                        { field: 'created', displayName: 'Дата создания', cellFilter: 'date:\'HH:mm:ss\'', width: 120, cellClass: 'text-center' },
                        { field: 'type', displayName: 'Тип' },
                        { field: 'priority', displayName: 'Приоритет' },
                        { field: 'state', displayName: 'Состояние' },
                        { comment: true }
                    ]
            }
        );

        $scope.remove = function (row) {
            utils.confirmDelete(function () {
                var claim = row ? row.entity : $scope.claim;
                $http.delete('api/Customer/Claims/' + claim.id)
                    .success(function (data) {
                        delete claim;
                        $scope.getPagedDataAsync();
                    });
            });
        }

        $scope.new = function () {
            $location.path('/customer/claims/new');
        }

        $scope.view = function (row) {
            var id = row ? row.entity.id : $scope.claim.id;
            $location.path('/customer/claims/view/' + id);
        }

        $scope.edit = function (row) {
            var id = row ? row.entity.id : $scope.claim.id;
            $location.path('/customer/claims/edit/' + id);
        }
    }]);