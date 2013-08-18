tasks.controller('admin.UsersCtrl',
    ['$scope', 'users', 'logger',
    function ($scope, users, logger) {
        $scope.users = users;
        logger.log('creating AdminUsersCtrl');
    }]);