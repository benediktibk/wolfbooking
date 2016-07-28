wolfBookingApp.controller('breadBookingsController', function ($scope, $q, $location, breadbookings, authentication, tables, users) {
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
        var username = authentication.getUsername();
        
        users.getItemByUserName(username).then(function (data) {
            breadbookings.getCurrentByRoom(data.data.Room).then(function (data) {
                tables.setAllRowsClean($scope, data.data.Bookings);
            })
        });
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistAllChanges = function () {
        breadbookings.updateItem($scope.data).then(function () {
            $scope.loadAll();
        });
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    };

    $scope.loadAll();
});