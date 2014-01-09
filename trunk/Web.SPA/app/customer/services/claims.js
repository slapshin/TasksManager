var services = angular.module('services.customer.claims', ['ngResource']);

services.factory('Claim', ['$resource',
    function ($resource) {
        return $resource('api/Customer/Claims/:id', { id: '@id' });
    }]);

services.factory('MultiClaimLoader', ['Claim', '$q',
    function (Claim, $q) {
        return function () {
            var delay = $q.defer();
            Claim.query(function (claims) {
                delay.resolve(claims);
            }, function () {
                delay.reject('Unable to fetch claims');
            });
            return delay.promise;
        };
    }]);

services.factory('ClaimLoader', ['Claim', '$route', '$q',
function (Claim, $route, $q) {
    return function () {
        var delay = $q.defer();
        Claim.get({ id: $route.current.params.claimId }, function (claim) {
            delay.resolve(claim);
        }, function () {
            delay.reject('Unable to fetch claim ' + $route.current.params.claimId);
        });
        return delay.promise;
    };
}]);