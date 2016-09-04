var wolfBookingApp = angular.module(
    'WolfBooking', 
    [
    'ngRoute', 'ngAnimate', 'ui.grid', 'ui.grid.autoResize', 'ui.grid.edit', 'ui.grid.rowEdit', 'ui.bootstrap',
    'pascalprecht.translate', 'ngSanitize', 'ngCookies',
    'Breads', 'Authentication', 'Users', 'PageHistory', 'Roles', 'Tables', 'Rooms', 'Breadbookings', 'Accounting'
    ]);

wolfBookingApp.config(['$routeProvider', '$locationProvider', '$translateProvider', function ($routeProvider, $locationProvider, $translateProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'views/login',
            controller: 'LoginCtrl'
        })
        .when('/Breads', {
            templateUrl: 'views/Breads',
            controller: 'BreadsCtrl'
        })
        .when('/Login', {
            templateUrl: 'views/login',
            controller: 'LoginCtrl'
        })
        .when('/Users', {
            templateUrl: 'views/Users',
            controller: 'UsersCtrl'
        })
        .when('/Rooms', {
            templateUrl: 'views/Rooms',
            controller: 'RoomsCtrl'
        })
        .when('/Breadbookings', {
            templateUrl: 'views/Breadbookings',
            controller: 'BreadbookingsCtrl'
        })
        .when('/Accounting', {
            templateUrl: 'views/Accounting',
            controller: 'AccountingCtrl'
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