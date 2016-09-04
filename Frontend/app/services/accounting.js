var Accounting = angular.module('Accounting', []);
Accounting.factory('Accounting', function ($http, Authentication) {
    var AccountingFactory = {};

    var calculateBill = function (room, startDate, endDate) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Accounting/calculatebill/' + room + '?startDate=' + startDate.toDateString() + '&endDate=' + endDate.toDateString(),
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }
    
    AccountingFactory.calculateBill = calculateBill;

    return AccountingFactory;
});