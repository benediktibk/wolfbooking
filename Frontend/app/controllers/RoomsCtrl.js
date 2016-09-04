wolfBookingApp.controller('RoomsCtrl', function ($scope, $q, $location, Rooms, Authentication, Tables) {
    Tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="RoomsDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteRoom(row)"></i></div>', width: 30 },
            { name: 'Rooms.Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
            { name: 'Rooms.Description', field: 'Description', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' }
    ]);

    if (!Authentication.isAuthenticated()) {
        $location.path('/Login');
        return;
    }

    $scope.loadAll = function () {
        Rooms.getAll().then(function (data) {
            Tables.setAllRowsClean($scope, data.data);
        })
    };

    $scope.calculateTableHeight = function () {
        return Tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return Rooms.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return Rooms.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return Rooms.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        Tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addRoom = function () {
        Tables.addRow($scope, {
            Id: 0,
            Name: '',
            Description: ''
        });
    };

    $scope.deleteRoom = function (row) {
        Rooms.isInUse(row.entity).then(function (data) {
            if (data.data)
                window.alert("You cannot delete this room, it is still assigned to an user.");
            else
                Tables.deleteRow($scope, row);
        });

    };

    $scope.loadAll();
});