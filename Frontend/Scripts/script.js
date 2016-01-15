var wolfBookingApp = angular.module('wolfBooking', ['ngRoute']);

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
wolfBookingApp.controller('breadsController', function ($scope) { });