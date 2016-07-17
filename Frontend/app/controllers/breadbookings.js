wolfBookingApp.controller('breadsBookingsController', function ($scope, $q, $location, breadBookings, authentication, tables) {
    tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: 'Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Price', field: 'Price', enableCellEdit: false, type: 'number', enableCellEditOnFocus: false },
            { name: 'Amount', field: 'Amount', enableCellEdit: false, type: 'number', enableCellEditOnFocus: false }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        breadBookings.getAll().then(function (data) {
            tables.setAllRowsClean($scope, data.data);
        })
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistAllChanges = function () {
        breadBookings.updateItem($scope.data).then(function () {
            $scope.loadAll();
        });
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    };

    $scope.loadAll();
});