var breads = angular.module('breads', []);
breads.factory('breads', function ($http, authentication) {
    var breadsFactory = {};

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breads/all',
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (bread) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/breads/item/' + bread.Id,
            data: bread
        });

        return httpRequest;
    }

    var createItem = function (bread) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/breads',
            data: bread
        });

        return httpRequest;
    }

    var deleteItem = function (bread) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/breads/item/' + bread.Id
        });

        return httpRequest;
    }

    breadsFactory.getAll = getAll;
    breadsFactory.updateItem = updateItem;
    breadsFactory.createItem = createItem;
    breadsFactory.deleteItem = deleteItem;

    return breadsFactory;
});
