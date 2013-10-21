app.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/', { templateUrl: 'app/views/home/index.html', controller: 'ctrl.home' });
    registerAdminRoutes($routeProvider);
    //registerCustomerRoutes($routeProvider);
    $routeProvider.otherwise({ redirectTo: '/' });
}]);

function registerAdminRoutes($routeProvider) {
    $routeProvider.
        // - users
    when('/admin/users/list', {
        templateUrl: 'app/admin/views/users/list.html',
        controller: 'admin.users.list'
    }).when('/admin/users', {
        templateUrl: 'app/admin/views/users/list.html',
        controller: 'admin.users.list'
    }).when('/admin/users/edit/:userId', {
        controller: 'admin.users.edit',
        templateUrl: 'app/admin/views/users/edit.html',
        resolve: {
            user: ["UserLoader", function (UserLoader) {
                return UserLoader();
            }]
        }
    }).when('/admin/users/view/:userId', {
        controller: 'admin.users.view',
        resolve: {
            user: ["UserLoader", function (UserLoader) {
                return UserLoader();
            }]
        },
        templateUrl: 'app/admin/views/users/view.html'
    }).when('/admin/users/new', {
        controller: 'admin.users.new',
        templateUrl: 'app/admin/views/users/edit.html',
    }).
    // - projects
    when('/admin/projects/list', {
        templateUrl: 'app/admin/views/projects/list.html',
        controller: 'admin.projects.list'
    }).when('/admin/projects', {
        templateUrl: 'app/admin/views/projects/list.html',
        controller: 'admin.projects.list'
    }).when('/admin/projects/edit/:projectId', {
        controller: 'admin.projects.edit',
        templateUrl: 'app/admin/views/projects/edit.html',
        resolve: {
            project: ["ProjectLoader", function (ProjectLoader) {
                return ProjectLoader();
            }],
            masters: ["ProjectMasters", function (ProjectMasters) {
                return ProjectMasters();
            }]
        }
    }).when('/admin/projects/view/:projectId', {
        controller: 'admin.projects.view',
        templateUrl: 'app/admin/views/projects/view.html',
        resolve: {
            project: ["ProjectLoader", function (ProjectLoader) {
                return ProjectLoader();
            }]
        },
    }).when('/admin/projects/new/', {
        controller: 'admin.projects.new',
        templateUrl: 'app/admin/views/projects/edit.html',
        resolve: {
            masters: ["ProjectMasters", function (ProjectMasters) {
                return ProjectMasters();
            }]
        }
    });
}

//function registerAdminRoutes($routeProvider) {
//    $routeProvider.
//        // - claims
//    when('/customer/claims/list', {
//        templateUrl: 'app/customer/views/claims/list.html',
//        controller: 'customer.claims.list'
//    }).
//    when('/customer/claims', {
//        templateUrl: 'app/customer/views/claims/list.html',
//        controller: 'customer.claims.list'
//    }).
//    when('/customer/claims/edit/:claimId', {
//        controller: 'customer.claims.edit',
//        templateUrl: 'app/customer/views/claims/edit.html',
//        resolve: {
//            user: ["ClaimLoader", function (service) {
//                return service();
//            }]
//        }
//    }).
//    when('/customer/users/view/:claimId', {
//        controller: 'customer.claims.view',
//        resolve: {
//            user: ["ClaimLoader", function (service) {
//                return service();
//            }]
//        },
//        templateUrl: 'app/customer/views/claims/view.html'
//    }).
//    when('/customer/users/new', {
//        controller: 'customer.claims.new',
//        templateUrl: 'app/customer/views/claims/edit.html'
//    });
//}