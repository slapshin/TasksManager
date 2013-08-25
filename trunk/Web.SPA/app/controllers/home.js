app.controller('HomeCtrl',
    ['$scope', '$dialog',
    function ($scope, $dialog) {
        $scope.openMessageBox = function () {
            var title = 'This is a message box';
            var msg = 'This is the content of the message box';
            var btns = [{ result: 'cancel', label: 'Cancel' }, { result: 'ok', label: 'OK', cssClass: 'btn-primary' }];

            $dialog.messageBox(title, msg, btns)
              .open()
              .then(function (result) {
                  alert('dialog closed with result: ' + result);
              });
        };
    }]);