var Breads = angular.module('Breads', []);
Breads.factory('Breads', function ($http, Authentication) {
    var BreadsFactory = {};

    var getAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/Breads/all',
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var getByIds = function (ids) {
        var url = 'api/Breads/items';

        for (var i = 0; i < ids.length; ++i) {
            var separator = '';
            if (i == 0)
                separator = '?';
            else
                separator = '&';

            url = url + separator + '=' + ids[i];
        }

        var httpRequest = $http({
            method: 'GET',
            url: url,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (bread) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/Breads/item/' + bread.Id,
            headers: Authentication.getHttpHeaderWithAuthorization(),
            data: bread
        });

        return httpRequest;
    }

    var createItem = function (bread) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/Breads',
            headers: Authentication.getHttpHeaderWithAuthorization(),
            data: bread
        });

        return httpRequest;
    }

    var deleteItem = function (bread) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/Breads/item/' + bread.Id,
            headers: Authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    BreadsFactory.getAll = getAll;
    BreadsFactory.updateItem = updateItem;
    BreadsFactory.createItem = createItem;
    BreadsFactory.deleteItem = deleteItem;
    BreadsFactory.getByIds = getByIds;

    return BreadsFactory;
});
