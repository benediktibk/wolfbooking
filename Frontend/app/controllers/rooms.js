﻿wolfBookingApp.controller('roomsController', function ($scope, $q, $location, rooms, authentication, tables) {
    tables.initialize($scope, [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="roomsDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteRoom(row)"></i></div>', width: 30 },
            { name: 'Rooms.Name', field: 'Name', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
            { name: 'Rooms.Description', field: 'Description', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        rooms.getAll().then(function (data) {
            tables.setAllRowsClean($scope, data.data);
        })
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return rooms.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return rooms.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return rooms.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addRoom = function () {
        tables.addRow($scope, {
            Id: 0,
            Name: '',
            Description: ''
        });
    };

    $scope.deleteRoom = function (row) {
        rooms.isInUse(row.entity).then(function (data) {
            if (data.data)
                window.alert("You cannot delete this room, it is still assigned to an user.");
            else
                tables.deleteRow($scope, row);
        });

    };

    $scope.loadAll();
});