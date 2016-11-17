wolfBookingApp.controller('UsersCtrl', function ($scope, $q, $location, $window, Users, Authentication, Tables) {
    $scope.availableRooms = [];

    Tables.initialize($scope, [
        { name: 'Id', field: 'Id', visible: false },
        { name: ' ', enableCellEdit: false, cellTemplate: '<div id="UsersDeleteButton"><i class="fa fa-trash-o fa-lg" ng-click="grid.appScope.deleteUser(row)"></i></div>', width: 30 },
        { name: 'Users.Login', field: 'Login', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
        { name: 'Users.Password', field: 'Password', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true, headerCellFilter: 'translate' },
        { name: 'Users.User', field: 'isUser', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isUser" ng-click="grid.appScope.markAsDirty(row)" checked>', headerCellFilter: 'translate' },
        { name: 'Users.Manager', field: 'isManager', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isManager" ng-click="grid.appScope.markAsDirty(row)">', headerCellFilter: 'translate' },
        { name: 'Users.Administrator', field: 'isAdministrator', enableCellEdit: false, cellTemplate: '<input type="checkbox" ng-model="row.entity.isAdministrator" ng-click="grid.appScope.markAsDirty(row)">', headerCellFilter: 'translate' },
        { name: 'Users.Room', field: 'Room', enableCellEdit: false, cellTemplate: '<select ng-model="row.entity.selectedRoom" ng-options="room.Name for room in row.entity.availableRooms track by room.Id" ng-change="grid.appScope.roomSelectionChanged(row.entity.selectedRoom)"></select>', headerCellFilter: 'translate' }
    ]);

    if (!Authentication.isAuthenticated()) {
        $location.path('/Login');
        return;
    }

    $scope.loadAll = function () {
        Users.getAll().then(function (data) {
            Tables.setAllRowsClean($scope, data.data);
        });
    };

    $scope.calculateTableHeight = function () {
        return Tables.calculateTableHeight($scope);
    };

    $scope.persistUpdate = function (rowEntity) {
        return Users.updateItem(rowEntity);
    };

    $scope.persistCreate = function (rowEntity) {
        return Users.createItem(rowEntity);
    };

    $scope.persistDelete = function (rowEntity) {
        return Users.deleteItem(rowEntity);
    }

    $scope.persistAllChanges = function () {
        var data = $scope.gridOptions.data;
        var validationErrors = [];

        for (var i = 0; i < data.length; ++i) {
            var current = data[i];

            if (!current.Login)
                validationErrors.push('missing login');

            if (current.NewlyAdded && !current.Password)
                validationErrors.push('user ' + current.Login + ' has an empty password');
        }

        if (validationErrors.length == 0)
            Tables.persistAllChanges($scope);
        else {
            var message = 'cannot save the changes because of the following errors:';

            for (var i = 0; i < validationErrors.length; ++i)
                message += '\n' + validationErrors[i];

            $window.alert(message);
        }
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addUser = function () {
        var user = {
            Id: 0,
            Login: '',
            Password: '',
            isUser: false,
            isManager: false,
            isAdministrator: false,
            NewlyAdded: true
        };
        Users.fillNewUserWithAvailableRooms(user).then(function (user) {
            user.selectedRoom = user.availableRooms[0];
            Tables.addRow($scope, user);
        });
    };

    $scope.deleteUser = function (row) {
        Tables.deleteRow($scope, row);
    };

    $scope.markAsDirty = function (row) {
        Tables.markAsDirty($scope, row);
    }

    $scope.roomSelectionChanged = function (selectedItem) {
        var user = selectedItem.ParentUser;
        var rows = $scope.gridApi.grid.rows;

        for (var i = 0; i < rows.length; ++i) {
            var row = rows[i];
            var rowEntity = row.entity;

            if (rowEntity.Id == user.Id) {
                Tables.markAsDirty($scope, row);
                return;
            }
        }
    }
    
    $scope.loadAll();
});