var breadBookings = angular.module('Breadbookings', []);
breadBookings.factory('Breadbookings', function ($http, Authentication) {
    var breadBookingsFactory = {};

    var getCurrentByRoom = function (roomId) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Breadbookings/currentbyroom/' + roomId,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (breadBookings) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/Breadbookings/item/' + breadBookings.Id,
            headers: Authentication.getHttpHeaderWithAuthorization(),
            data: breadBookings
        });

        return httpRequest;
    }

    breadBookingsFactory.getCurrentByRoom = getCurrentByRoom;
    breadBookingsFactory.updateItem = updateItem;

    return breadBookingsFactory;
});