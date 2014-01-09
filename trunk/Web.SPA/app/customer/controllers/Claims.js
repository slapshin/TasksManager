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
                        { field: 'created', displayName: 'Дата создания', cellFilter: 'date:\'medium\'', width: 120, cellClass: 'text-center' },
                        { field: 'type', displayName: 'Тип', cellFilter: 'taskType' },
                        { field: 'priority', displayName: 'Приоритет', cellFilter: 'taskPriority' },
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

app.controller('customer.claims.view', ['$scope', 'claim', '$location',
    function ($scope, claim, $location) {
        $scope.claim = claim;
        $scope.edit = function () {
            $location.path('/customer/claims/edit/' + $scope.claim.id);
        }
    }]);

app.controller('customer.claims.edit', ['$scope', '$location', '$http', 'claim', 'utils', 'consts',
    function ($scope, $location, $http, claim, utils, consts) {
        $scope.claim = claim;

        var emptyComboBoxItem = { id: null, display: '' };

        $scope.taskTypes = [];
        $.each(consts.taskType, function (key, value) {
            var item = { id: key, display: value.translation };
            $scope.taskTypes.push(item);
            if (claim.type == key) {
                $scope.selectedTaskType = item;
            }
        });

        $scope.$watch('selectedTaskType', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.claim.type = $scope.selectedTaskType.id;
            }
        }, true);

        $scope.priorities = [];
        $.each(consts.taskPriority, function (key, value) {
            var item = { id: key, display: value.translation };
            $scope.priorities.push(item);
            if (claim.priority == key) {
                $scope.selectedPriority = item;
            }
        });

        $scope.$watch('selectedPriority', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.claim.priority = $scope.selectedPriority.id;
            }
        }, true);

        $scope.save = function () {
            $scope.claim.$save(function (claim) {
                $location.path('/customer/claims/view/' + claim.id);
            });
        }

        $scope.remove = function () {
            utils.confirmDelete(function () {
                $scope.claim.$delete(function () {
                    delete $scope.claim;
                    $location.path('/customer/claims/');
                });
            });
        };
    }]);

app.controller('customer.claims.new', ['$scope', 'Claim', '$location', 'consts',
    function ($scope, Claim, $location, consts) {
        $scope.claim = new Claim();

        var emptyComboBoxItem = { id: null, display: '' };

        $scope.taskTypes = [];
        $.each(consts.taskType, function (key, value) {
            var item = { id: key, display: value.translation };
            $scope.taskTypes.push(item);
            if ($scope.claim.type == key) {
                $scope.selectedTaskType = item;
            }
        });

        $scope.$watch('selectedTaskType', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.claim.type = $scope.selectedTaskType.id;
            }
        }, true);

        $scope.priorities = [];
        $.each(consts.taskPriority, function (key, value) {
            var item = { id: key, display: value.translation };
            $scope.priorities.push(item);
            if ($scope.claim.priority == key) {
                $scope.selectedPriority = item;
            }
        });

        $scope.$watch('selectedPriority', function (newVal, oldVal) {
            if (newVal !== oldVal) {
                $scope.claim.priority = $scope.selectedPriority.id;
            }
        }, true);

        $scope.save = function () {
            $scope.claim.$save(function (claim) {
                $location.path('/customer/claims/view/' + claim.id);
            });
        }
    }]);