wolfBookingApp.controller('HomeCtrl', function ($scope, $translate, PageHistory, Authentication) {
    $scope.logout = function () {
        Authentication.logout();
    }

    $scope.isLoggedIn = function () {
        return Authentication.isAuthenticated();
    }

    $scope.isAdministrator = function () {
        return Authentication.isAuthenticated() && Authentication.isAdministrator();
    }

    $scope.isMoreThanUser = function () {
        return Authentication.isAuthenticated() && !Authentication.isOnlyUser();
    }

    $scope.isNotLoggedIn = function () {
        return !Authentication.isAuthenticated();
    }

    $scope.changeLanguage = function () {
        var currentLanguage = $translate.use();
        var nextLanguage = 'en';

        if (currentLanguage == 'en')
            nextLanguage = 'de';

        $translate.use(nextLanguage);
    }
});