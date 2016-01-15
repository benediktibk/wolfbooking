var wolfBookingApp = angular.module('wolfBooking', ['ngRoute']);

wolfBookingApp.config(function ($routeProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'views/home',
            controller: 'homeController'
        });
})

wolfBookingApp.controller('homeController', function ($scope) { });