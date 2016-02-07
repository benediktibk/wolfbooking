wolfBookingApp.controller('usersController', function ($scope, $q, $location, users, authentication) {
    $scope.deleted = [];

    if (!authentication.isAuthenticated()) {
        $location.path('/login');
        return;
    }

    $scope.loadAll = function () {
        users.getAll().then(function (data) {
            $scope.gridOptions.data = data.data;
            var dirtyRows = $scope.gridApi.rowEdit.getDirtyRows($scope.gridApi.grid);
            var dataDirtyRows = dirtyRows.map(function (gridRow) {
                return gridRow.entity;
            });
            $scope.gridApi.rowEdit.setRowsClean(dataDirtyRows);
        })
    };

    $scope.calculateTableHeight = function () {
        var rowHeight = 30;
        var headerRowHeight = 33;
        return {
            height: ($scope.gridOptions.data.length * rowHeight + headerRowHeight) + "px"
        };
    };

    $scope.persistUpdate = function (rowEntity) {
        var httpRequest = users.updateItem(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, httpRequest);
        return httpRequest;
    };

    $scope.persistCreate = function (rowEntity) {
        var httpRequest = users.createItem(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, httpRequest);
        return httpRequest;
    };

    $scope.persistDelete = function (rowEntity) {
        var httpRequest = users.deleteItem(rowEntity);
        return httpRequest;
    }

    $scope.persistAllChanges = function () {
        var dirtyRows = $scope.gridApi.rowEdit.getDirtyRows($scope.gridApi.grid);
        var dataDirtyRows = dirtyRows.map(function (gridRow) {
            return gridRow.entity;
        });

        var promises = [];
        var i;
        for (i = 0; i < dataDirtyRows.length; ++i) {
            var row = dataDirtyRows[i];
            if (row.Id == 0)
                promises.push($scope.persistCreate(row));
            else
                promises.push($scope.persistUpdate(row));
        }

        for (i = 0; i < $scope.deleted.length; ++i)
            promises.push($scope.persistDelete($scope.deleted[i]));

        $q.all(promises).then(function () {
            $scope.loadAll();
            $scope.deleted = [];
        });
    };

    $scope.cancelAllChanges = function () {
        $scope.loadAll();
    }

    $scope.addUser = function () {
        $scope.gridOptions.data.push({
            Id: 0,
            Name: '',
            Price: ''
        });
    };

    $scope.deleteUser = function (row) {
        var index = $scope.gridOptions.data.indexOf(row.entity);
        $scope.deleted.push(row.entity);
        $scope.gridOptions.data.splice(index, 1);
    };

    /*$scope.toggleIsUser = function (row) {
        row.entity.isUser = !row.entity.isUser;
    }

    $scope.toggleIsManager = function (row) {
        row.entity.isManager = !row.entity.isManager;
    }

    $scope.toggleIsAdministrator = function (row) {
        row.entity.isAdministrator = !row.entity.isAdministrator;
    }*/

    $scope.gridOptions = {
        data: [],
        enableHorizontalScrollbar: 0,
        enableVerticalScrollbar: 0,
        rowEditWaitInterval: -1,
        columnDefs: [
            { name: 'Id', field: 'Id', visible: false },
            { name: ' ', enableCellEdit: false, cellTemplate: '<div id="usersDeleteButton"><i class="fa fa-times fa-lg" ng-click="grid.appScope.deleteUser(row)"></i></div>', width: 30 },
            { name: 'Login', field: 'Login', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'Password', field: 'Password', enableCellEdit: true, type: 'string', enableCellEditOnFocus: true },
            { name: 'User', field: 'isUser', enableCellEdit: true, cellTemplate: '<input type="checkbox" ng-model="row.entity.isUser">' },
            { name: 'Manager', field: 'isManager', enableCellEdit: true, cellTemplate: '<input type="checkbox" ng-model="row.entity.isManager">' },
            { name: 'Administrator', field: 'isAdministrator', enableCellEdit: true, cellTemplate: '<input type="checkbox" ng-model="row.entity.isAdministrator">' }
        ],
        enableColumnMenus: false
    };

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };

    $scope.loadAll();
});