var wolfBookingApp = angular.module(
    'wolfBooking', 
    [
    'ngRoute', 'ngAnimate', 'ui.grid', 'ui.grid.autoResize', 'ui.grid.edit', 'ui.grid.rowEdit', 'ui.bootstrap',
    'breads', 'authentication', 'users', 'pagehistory', 'roles', 'tables', 'rooms', 'breadbookings', 'accounting'
    ]);

wolfBookingApp.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'views/login',
            controller: 'loginController'
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
        })
        .when('/accounting', {
            templateUrl: 'views/accounting',
            controller: 'accountingController'
        });

    $locationProvider.html5Mode(true);
});