var breadBookings = angular.module('breadbookings', []);
breadBookings.factory('breadbookings', function ($http, authentication) {
    var breadBookingsFactory = {};

    var getCurrentByRoom = function (roomId) {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breadbookings/currentbyroom/' + roomId,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (breadBookings) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/breadbookings/item/' + breadBookings.Id,
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: breadBookings
        });

        return httpRequest;
    }

    breadBookingsFactory.getCurrentByRoom = getCurrentByRoom;
    breadBookingsFactory.updateItem = updateItem;

    return breadBookingsFactory;
});