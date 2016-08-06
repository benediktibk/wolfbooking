var accounting = angular.module('accounting', []);
accounting.factory('accounting', function ($http, authentication) {
    var accountingFactory = {};

    var calculateBill = function (room, startDate, endDate) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/accounting/calculatebill/' + room + '?startDate=' + startDate.toDateString() + '&endDate=' + endDate.toDateString(),
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }
    
    accountingFactory.calculateBill = calculateBill;

    return accountingFactory;
});