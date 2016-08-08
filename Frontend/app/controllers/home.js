wolfBookingApp.controller('homeController', function ($scope, $translate, pagehistory, authentication) {
    $scope.logout = function () {
        authentication.logout();
    }

    $scope.isLoggedIn = function () {
        return authentication.isAuthenticated();
    }

    $scope.isOnlyUser = function () {
        return authentication.isOnlyUser();
    }

    $scope.isAdministrator = function () {
        return authentication.isAdministrator();
    }

    $scope.changeLanguage = function () {
        var currentLanguage = $translate.use();
        var nextLanguage = 'en';

        if (currentLanguage == 'en')
            nextLanguage = 'de';

        $translate.use(nextLanguage);
    }
});