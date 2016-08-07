var wolfBookingApp = angular.module(
    'wolfBooking', 
    [
    'ngRoute', 'ngAnimate', 'ui.grid', 'ui.grid.autoResize', 'ui.grid.edit', 'ui.grid.rowEdit', 'ui.bootstrap',
    'pascalprecht.translate', 'ngSanitize',
    'breads', 'authentication', 'users', 'pagehistory', 'roles', 'tables', 'rooms', 'breadbookings', 'accounting'
    ]);

wolfBookingApp.config(['$routeProvider', '$locationProvider', '$translateProvider', function ($routeProvider, $locationProvider, $translateProvider) {
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

    var translationsDe = {
        BLUB: 'BLUB in de'
    };
    var translationsEn = {
        BLUB: 'BLUB in en'
    };

    $translateProvider
        .translations('en', translationsEn)
        .translations('de', translationsDe)
        .preferredLanguage('de')
        .useSanitizeValueStrategy('sanitize');
}]);