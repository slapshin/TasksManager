var utilsModule = angular.module('utils', ['ui.bootstrap.transition']);

utilsModule.controller('MessageBoxController', ['$scope', 'dialog', 'model', function ($scope, dialog, model) {
    $scope.title = model.title;
    $scope.message = model.message;
    $scope.buttons = model.buttons;
    $scope.close = function (res) {
        dialog.close(res);
    };
}]);

utilsModule.controller('CustomDialogController', ['$scope', 'dialog', 'model', function ($scope, dialog, model) {
    $scope.title = model.title;
    $scope.content = model.content;
    $scope.buttons = model.buttons;
    $scope.dialog = dialog;
    $scope.click = function (btn) {
        if (btn.onClick) {
            btn.onClick($scope);
        }
        else{
            dialog.close();
        }        
    };
}]);

utilsModule.provider("customDialog", function () {
    var defaults = {
        resolve: {}
    };

    var globalOptions = {};
    this.options = function (value) {
        globalOptions = value;
    };

    this.$get = ['$http', '$document', '$compile', '$rootScope', '$controller', '$templateCache', '$q', '$transition', '$injector',
        function ($http, $document, $compile, $rootScope, $controller, $templateCache, $q, $transition, $injector) {
            var body = $document.find('body');

            function Dialog(opts) {
                var self = this,
                    options = this.options = angular.extend({}, defaults, globalOptions, opts);
                this._open = false;
                this.modalEl = angular.element('<div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">');
            }

            Dialog.prototype.isOpen = function () {
                return this._open;
            };

            Dialog.prototype.open = function (templateUrl, controller) {
                var self = this, options = this.options;

                if (templateUrl) {
                    options.templateUrl = templateUrl;
                }
                if (controller) {
                    options.controller = controller;
                }

                if (!(options.template || options.templateUrl)) {
                    throw new Error('Dialog.open expected template or templateUrl, neither found. Use options or open method to specify them.');
                }

                this._loadResolves().then(function (locals) {
                    var $scope = locals.$scope = self.$scope = locals.$scope ? locals.$scope : $rootScope.$new();

                    self.modalEl.html(locals.$template);

                    if (self.options.controller) {
                        var ctrl = $controller(self.options.controller, locals);
                        self.modalEl.children().data('ngControllerController', ctrl);
                    }

                    $compile(self.modalEl)($scope);
                    self._addElementsToDom();

                    self.modalEl.on('hidden.bs.modal', function () {
                        self._removeElementsFromDom();
                    })
                    self.modalEl.modal('show');
                });

                this.deferred = $q.defer();
                return this.deferred.promise;
            };

            // closes the dialog and resolves the promise returned by the `open` method with the specified result.
            Dialog.prototype.close = function (result) {
                var self = this;

                this._onCloseComplete(result);

                function removeTriggerClass(el) {
                    el.removeClass(self.options.triggerClass);
                }

                function onCloseComplete() {
                    if (self._open) {
                        self._onCloseComplete(result);
                    }
                }
            };

            Dialog.prototype._onCloseComplete = function (result) {
                this.modalEl.modal('hide');
                this.deferred.resolve(result);
            };

            Dialog.prototype._addElementsToDom = function () {
                body.append(this.modalEl);
                this._open = true;
            };

            Dialog.prototype._removeElementsFromDom = function () {
                this.modalEl.remove();
                this._open = false;
            };

            // Loads all `options.resolve` members to be used as locals for the controller associated with the dialog.
            Dialog.prototype._loadResolves = function () {
                var values = [], keys = [], templatePromise, self = this;

                if (this.options.template) {
                    templatePromise = $q.when(this.options.template);
                } else if (this.options.templateUrl) {
                    templatePromise = $http.get(this.options.templateUrl, { cache: $templateCache })
                    .then(function (response) { return response.data; });
                }

                angular.forEach(this.options.resolve || [], function (value, key) {
                    keys.push(key);
                    values.push(angular.isString(value) ? $injector.get(value) : $injector.invoke(value));
                });

                keys.push('$template');
                values.push(templatePromise);

                return $q.all(values).then(function (values) {
                    var locals = {};
                    angular.forEach(values, function (value, index) {
                        locals[keys[index]] = value;
                    });
                    locals.dialog = self;
                    return locals;
                });
            };

            return {
                dialog: function (opts) {
                    return new Dialog(opts);
                },
                messageBox: function (title, message, buttons) {
                    return new Dialog({
                        templateUrl: 'template/dialog/message.html', controller: 'MessageBoxController', resolve:
                          {
                              model: function () {
                                  return {
                                      title: title,
                                      message: message,
                                      buttons: buttons
                                  };
                              }
                          }
                    });
                }
            };
        }];
});

utilsModule.factory('dialogs', ['customDialog', 'consts', function (customDialog, consts) {
    var dialogs = {
        confirm: confirm,
        confirmDelete: confirmDelete,
        show: show
    };
    return dialogs;

    function confirm(title, msg, callback) {
        var opts = {
            resolve:
              {
                  model: function () {
                      return {
                          title: title,
                          message: msg,
                          buttons: [{ result: 'cancel', label: 'Отмена' }, { result: 'ok', label: 'OK', cssClass: 'btn-primary' }]
                      };
                  }
              },
            controller: 'MessageBoxController',
            template: consts.messageBoxTemplate
        };

        customDialog.dialog(opts)
          .open()
          .then(function (result) {
              if (result === 'ok') {
                  callback();
              }
          });
    }

    function confirmDelete(callback) {
        return confirm('Подтверждение удаления', 'Объект будет удален. Продолжить?', callback);
    }

    function show(settings) {
        var opts = {
            resolve:
              {
                  model: function () {
                      return {
                          title: settings.title,
                          content: settings.content,
                          buttons: settings.buttons
                      };
                  }
              },
            controller: 'CustomDialogController',
            template: consts.customDialogTemplate
        };

        customDialog.dialog(opts)
          .open();
    }
}]);