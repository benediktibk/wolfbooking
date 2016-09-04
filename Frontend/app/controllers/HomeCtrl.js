wolfBookingApp.controller('HomeCtrl', function ($scope, $translate, PageHistory, Authentication) {
    $scope.logout = function () {
        Authentication.logout();
    }

    $scope.isLoggedIn = function () {
        return Authentication.isAuthenticated();
    }

    $scope.isOnlyUser = function () {
        return Authentication.isOnlyUser();
    }

    $scope.isAdministrator = function () {
        return Authentication.isAdministrator();
    }

    $scope.changeLanguage = function () {
        var currentLanguage = $translate.use();
        var nextLanguage = 'en';

        if (currentLanguage == 'en')
            nextLanguage = 'de';

        $translate.use(nextLanguage);
    }
});