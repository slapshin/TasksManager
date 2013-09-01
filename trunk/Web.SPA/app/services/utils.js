var utilsModule = angular.module('utils', ['ui.bootstrap.transition']);

utilsModule.controller('MessageBoxController', ['$scope', 'dialog', 'model', function ($scope, dialog, model) {
    $scope.title = model.title;
    $scope.message = model.message;
    $scope.buttons = model.buttons;

    $scope.close = function (res) {
        dialog.close(res);
    };
}]);

utilsModule.controller('CustomDialogController', ['$scope', 'dialog', 'model', '$compile', function ($scope, dialog, model, $compile) {
    $scope.title = model.title;
    $scope.buttons = model.buttons;
    $scope.dialog = dialog;
    if (model.init) {
        model.init($scope);
    }
    $scope.content = $compile(model.content)($scope);
    $scope.click = function (btn) {
        if (btn.onClick) {
            btn.onClick($scope);
        }
        else {
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
        messageBox: messageBox,
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
                          buttons:
                          [
                              { result: 'ok', label: 'OK', cssClass: 'btn-primary' },
                              { result: 'cancel', label: 'Отмена' }
                          ]
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

    function messageBox(title, msg) {
        var opts = {
            resolve:
              {
                  model: function () {
                      return {
                          title: title,
                          message: msg,
                          buttons:
                          [
                              { result: 'ok', label: 'OK', cssClass: 'btn-primary' }                            
                          ]
                      };
                  }
              },
            controller: 'MessageBoxController',
            template: consts.messageBoxTemplate
        };

        customDialog.dialog(opts).open();
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
                          buttons: settings.buttons,
                          init: settings.init
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

utilsModule.factory('customGrid', ['$http', 'consts', 'dialogs', function ($http, consts, dialogs) {
    var builder = {
        build: build
    };
    return builder;

    function build(settings) {
        var $scope = settings.$scope;

        $scope.totalServerItems = settings.total;

        $scope.pagingOptions = {
            pageSizes: consts.pageSizes,
            pageSize: consts.pageSize,
            currentPage: 1
        };

        $scope.refreshGrid = function () {
            $scope.getPagedDataAsync($scope.pagingOptions.pageSize, $scope.pagingOptions.currentPage);
        }

        $scope.getPagedDataAsync = function (pageSize, page) {
            setTimeout(function () {
                $http.get(settings.pageDataUrl,
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

        $scope.setPagingData = function (data) {
            if (settings.onSetPagingData) {
                settings.onSetPagingData(data);
            }

            $scope.gridData = data;
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };

        $scope.$watch('pagingOptions', function (newVal, oldVal) {
            if (newVal !== oldVal && newVal.currentPage !== oldVal.currentPage) {
                $scope.refreshGrid();
            }
        }, true);

        var columnDefs = [];
        for (var i = 0, len = settings.columnDefs.length; i < len; i++) {
            var col = settings.columnDefs[i];
            var def = {};
            if (col.comment)
            {
                def.width = '40px';
                def.cellTemplate = '<a href="" class="btn btn-small btn-link" ng-click="gridShowComment(row)"><i class="glyphicon glyphicon-comment" /></a>';
                if (!$scope.gridShowComment) {
                    $scope.gridShowComment = function (row) {
                        dialogs.messageBox("Комментарий", row.entity.comment);
                    }
                }                
            }

            if (col.field) {
                def.field = col.field;
            }

            if (col.displayName) {
                def.displayName = col.displayName;
            }

            if (col.width) {
                def.width = col.width + 'px';
            }

            if (col.button) {
                def.cellTemplate = '<a href="" class="btn btn-small btn-link" ng-click="' + col.button.onClick + '(row)"><i class="glyphicon glyphicon-' + col.button.icon + '" /></a>';
            }

            columnDefs.push(def);
        }

        $scope.gridOptions = {
            data: 'gridData',
            multiSelect: false,
            enableColumnResize: true,
            showColumnMenu: true,
            columnDefs: columnDefs,
            enablePaging: true,
            showFooter: true,
            totalServerItems: 'totalServerItems',
            pagingOptions: $scope.pagingOptions,
            afterSelectionChange: settings.afterSelectionChange
        };

        $scope.refreshGrid();
    };
}]);