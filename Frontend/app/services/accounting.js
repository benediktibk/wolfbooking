var accounting = angular.module('accounting', []);
accounting.factory('accounting', function ($http, authentication) {
    var accountingFactory = {};

    var getBill = function (startDate, endDate, rooms) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breadbookings/currentbyroom/' + roomId,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    breadBookingsFactory.getBill = getBill;

    return accountingFactory;
});