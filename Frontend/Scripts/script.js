var wolfBookingApp = angular.module('wolfBooking', ['ngRoute', 'ui.grid']);

wolfBookingApp.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'views/home',
            controller: 'homeController'
        })
        .when('/home', {
            templateUrl: 'views/home',
            controller: 'homeController'
        })
        .when('/breads', {
            templateUrl: 'views/breads',
            controller: 'breadsController'
        });

    $locationProvider.html5Mode(true);
})

wolfBookingApp.controller('homeController', function ($scope) { });
wolfBookingApp.controller('breadsController', function ($scope, $http) {
    $scope.breads = [];

    $scope.loadBreads = function () {
        var httpRequest = $http({
            method: 'GET',
            url: 'api/breads/all'
        }).then(function (data) {
            $scope.breads = data.data;
        })
    };

    $scope.loadBreads();
});