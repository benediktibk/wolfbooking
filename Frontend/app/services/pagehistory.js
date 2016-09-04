var PageHistory = angular.module('PageHistory', []);
PageHistory.factory('PageHistory', function ($rootScope, $location) {
    var PageHistoryFactory = {};

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

    PageHistoryFactory.goToPreviousView = goToPreviousView;
    PageHistoryFactory.doesPreviousViewExist = doesPreviousViewExist;

    return PageHistoryFactory;
});