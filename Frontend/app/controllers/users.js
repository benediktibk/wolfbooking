wolfBookingApp.controller('usersController', function ($scope, $q, $location, users, authentication, tables) {
    $scope.availableRooms = [];

    tables.initialize($scope, [
        { name: 'Id', field: 'Id', visible: false },
        { name: ' ', enableCellEdit: false, cellTemplate: '<div id="usersDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteUser(row)"></i></div>', width: 30 },
        { name: 'Users.Login', field: 'Login', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
        { name: 'Users.Password', field: 'Password', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
        { name: 'Users.User', field: 'isUser', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isUser" ng-click="grid.appScope.markAsDirty(row)">', headerCellFilter: 'translate' },
        { name: 'Users.Manager', field: 'isManager', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isManager" ng-click="grid.appScope.markAsDirty(row)">', headerCellFilter: 'translate' },
        { name: 'Users.Administrator', field: 'isAdministrator', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isAdministrator" ng-click="grid.appScope.markAsDirty(row)">', headerCellFilter: 'translate' },
        { name: 'Users.Room', field: 'Room', enableCellEdit: false, cellTemplate: '<select ng-model="row.entity.selectedRoom" ng-options="room.Name for room in row.entity.availableRooms track by room.Id" ng-change="grid.appScope.roomSelectionChanged(row.entity.selectedRoom)"></select>', headerCellFilter: 'translate' }
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
        var user = {
            Id: 0,
            Login: '',
            Password: '',
            User: false,
            Manager: false,
            Administrator: false
        };
        users.fillNewUserWithAvailableRooms(user).then(function (user) {
            tables.addRow($scope, user);
        });
    };

    $scope.deleteUser = function (row) {
        tables.deleteRow($scope, row);
    };

    $scope.markAsDirty = function (row) {
        tables.markAsDirty($scope, row);
    }

    $scope.roomSelectionChanged = function (selectedItem) {
        var user = selectedItem.ParentUser;
        var rows = $scope.gridApi.grid.rows;

        for (var i = 0; i < rows.length; ++i) {
            var row = rows[i];
            var rowEntity = row.entity;

            if (rowEntity.Id == user.Id) {
                tables.markAsDirty($scope, row);
                return;
            }
        }
    }
    
    $scope.loadAll();
});