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

    var getByIds = function (ids) {
        var url = 'api/breads/items?'

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
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    var updateItem = function (bread) {
        var httpRequest = $http({
            method: 'PUT',
            url: 'api/breads/item/' + bread.Id,
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: bread
        });

        return httpRequest;
    }

    var createItem = function (bread) {
        var httpRequest = $http({
            method: 'POST',
            url: 'api/breads',
            headers: authentication.getHttpHeaderWithAuthorization(),
            data: bread
        });

        return httpRequest;
    }

    var deleteItem = function (bread) {
        var httpRequest = $http({
            method: 'DELETE',
            url: 'api/breads/item/' + bread.Id,
            headers: authentication.getHttpHeaderWithAuthorization()
        });

        return httpRequest;
    }

    breadsFactory.getAll = getAll;
    breadsFactory.updateItem = updateItem;
    breadsFactory.createItem = createItem;
    breadsFactory.deleteItem = deleteItem;
    breadsFactory.getByIds = getByIds;

    return breadsFactory;
});
