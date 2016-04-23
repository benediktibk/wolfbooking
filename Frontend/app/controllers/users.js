﻿wolfBookingApp.controller('usersController', function ($scope, $q, $location, users, authentication, tables) {
    $scope.availableRooms = [];

    tables.initialize($scope, [
        { name: 'Id', field: 'Id', visible: false },
        { name: ' ', enableCellEdit: false, cellTemplate: '<div id="usersDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteUser(row)"></i></div>', width: 30 },
        { name: 'Login', field: 'Login', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
        { name: 'Password', field: 'Password', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
        { name: 'User', field: 'isUser', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isUser" ng-click="grid.appScope.markAsDirty(row)">' },
        { name: 'Manager', field: 'isManager', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isManager" ng-click="grid.appScope.markAsDirty(row)">' },
        { name: 'Administrator', field: 'isAdministrator', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isAdministrator" ng-click="grid.appScope.markAsDirty(row)">' },
        { name: 'Room', field: 'Room', enableCellEdit: false, cellTemplate: '<select ng-model="row.entity.Room"><option ng-repeat="room in row.entity.availableRooms" value="{{room.Id}}">{{room.Name}}</option></select>' }
    ]);

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        users.getAll().then(function (data) {
            tables.setAllRowsClean($scope, data.data);
        });
    };

    $scope.calculateTableHeight = function () {
        return tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return users.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return users.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return users.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        tables.persistAllChanges($scope);
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addUser = function () {
        tables.addRow($scope, {
            Id: 0,
            Login: '',
            Password: '',
            User: false,
            Manager: false,
            Administrator: false
        });
    };

    $scope.deleteUser = function (row) {
        tables.deleteRow($scope, row);
    };

    $scope.markAsDirty = function (row) {
        tables.markAsDirty($scope, row);
    }
    
    $scope.loadAll();
});