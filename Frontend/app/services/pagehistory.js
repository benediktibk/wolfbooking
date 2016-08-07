var pagehistory = angular.module('pagehistory', []);
pagehistory.factory('pagehistory', function ($rootScope, $location) {
    var pagehistoryFactory = {};

    var _viewHistory = [];

    $rootScope.$on('$routeChangeSuccess', function () {
        _viewHistory.push($location.$$path);
    });

    var calculatePreviousViewIndex = function () {
        return _viewHistory.length - 2;
    }

    var doesPreviousViewExist = function () {
        return calculatePreviousViewIndex() >= 0;
    }

    var goToPreviousView = function () {
        var index = calculatePreviousViewIndex();

        if (index < 0)
            return;

        return $location.path(_viewHistory[index]);
    }

    pagehistoryFactory.goToPreviousView = goToPreviousView;
    pagehistoryFactory.doesPreviousViewExist = doesPreviousViewExist;

    return pagehistoryFactory;
});