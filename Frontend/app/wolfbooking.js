var wolfBookingApp = angular.module(
    'wolfBooking', 
    [
    'ngRoute', 'ngAnimate', 'ui.grid', 'ui.grid.autoResize', 'ui.grid.edit', 'ui.grid.rowEdit',
    'breads', 'authentication', 'users', 'pagehistory', 'roles', 'tables', 'rooms', 'breadbookings'
    ]);

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
        })
        .when('/login', {
            templateUrl: 'views/login',
            controller: 'loginController'
        })
        .when('/users', {
            templateUrl: 'views/users',
            controller: 'usersController'
        })
        .when('/rooms', {
            templateUrl: 'views/rooms',
            controller: 'roomsController'
        })
        .when('/breadbookings', {
            templateUrl: 'views/breadbookings',
            controller: 'breadBookingsController'
        });

    $locationProvider.html5Mode(true);
});