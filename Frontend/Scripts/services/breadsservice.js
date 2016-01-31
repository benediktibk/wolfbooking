var breads = angular.module('breads', []);
breads.factory('breads', function ($http) {
    var breadsFactory = {};

    var loadAll = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breads/all'
        });

        return httpRequest;
    }

    breadsFactory.loadAll = loadAll;

    return breadsFactory;
});
