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

    $translateProvider.useStaticFilesLoader({
        prefix: 'app/localization/locale-',
        suffix: '.json'
    });

    $translateProvider.useSanitizeValueStrategy('escape');
    var language = window.navigator.userLanguage || window.navigator.language;
    var languageCode = language.substring(0, 2);

    if (languageCode != 'de')
        languageCode = 'en';

    $translateProvider.preferredLanguage(languageCode);
}]);