var pagehistory = angular.module('pagehistory', []);
pagehistory.factory('pagehistory', function ($rootScope, $location) {
    var pagehistoryFactory = {};

    var _viewHistory = [];

    $rootScope.$on('$routeChangeSuccess', function () {
        _viewHistory.push($location.$$path);
    });

    var goToPreviousView = function () {
        var index = _viewHistory.length - 2;

        if (index < 0)
            return;

        return $location.path(_viewHistory[index]);
    }

    pagehistoryFactory.goToPreviousView = goToPreviousView;

    return pagehistoryFactory;
});