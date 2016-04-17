var users = angular.module('tables', []);
users.factory('tables', function ($q) {
    var tablesFactory = {};

    var initialize = function (scope, columnDefinitions) {
        scope.deleted = [];

        $scope.gridOptions = {
            data: [],
            enableHorizontalScrollbar: 0,
            enableVerticalScrollbar: 0,
            rowEditWaitInterval: -1,
            columnDefs: columnDefinitions,
            enableColumnMenus: false
        };
    }

    var setAllRowsClean = function (scope, data) {
        scope.gridOptions.data = data;
        var dirtyRows = scope.gridApi.rowEdit.getDirtyRows(scope.gridApi.grid);
        var dataDirtyRows = dirtyRows.map(function (gridRow) {
            return gridRow.entity;
        });
        scope.gridApi.rowEdit.setRowsClean(dataDirtyRows);
        scope.deleted = [];
    }

    var calculateTableHeight = function (scope) {
        var rowHeight = 30;
        var headerRowHeight = 33;
        return {
            height: (scope.gridOptions.data.length * rowHeight + headerRowHeight) + "px"
        };
    }

    var persistAllChanges = function (scope) {
        var dirtyRows = scope.gridApi.rowEdit.getDirtyRows(scope.gridApi.grid);
        var dataDirtyRows = dirtyRows.map(function (gridRow) {
            return gridRow.entity;
        });

        var promises = [];
        var i;
        for (i = 0; i < dataDirtyRows.length; ++i) {
            var row = dataDirtyRows[i];
            if (row.Id == 0)
                promises.push(scope.persistCreate(row));
            else
                promises.push(scope.persistUpdate(row));
        }

        for (i = 0; i < scope.deleted.length; ++i)
            promises.push(scope.persistDelete(scope.deleted[i]));

        $q.all(promises).then(function () {
            scope.loadAll();
            scope.deleted = [];
        });
    };

    tablesFactory.setAllRowsClean = setAllRowsClean
    tablesFactory.calculateTableHeight = calculateTableHeight;
    tablesFactory.persistAllChanges = persistAllChanges;
    tablesFactory.initialize = initialize;

    return tablesFactory;
});
