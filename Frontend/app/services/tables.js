var users = angular.module('tables', []);
users.factory('tables', function ($q) {
    var tablesFactory = {};

    var initialize = function (scope, columnDefinitions) {
        scope.deleted = [];

        scope.gridOptions = {
            data: [],
            enableHorizontalScrollbar: 0,
            enableVerticalScrollbar: 0,
            rowEditWaitInterval: -1,
            columnDefs: columnDefinitions,
            enableColumnMenus: false
        };        

        scope.gridOptions.onRegisterApi = function (gridApi) {
            scope.gridApi = gridApi;
        };
    }

    var addColumnDefinition = function (scope, columnDefinition) {
        scope.gridOptions.columnDefs.splice(scope.gridOptions.columnDefs.length, 0, columnDefinition);
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
            if (row.Id == 0) {
                var promise = scope.persistCreate(row);
                scope.gridApi.rowEdit.setSavePromise(row, promise);
                promises.push(promise);
            }
            else {
                var promise = scope.persistUpdate(row);
                scope.gridApi.rowEdit.setSavePromise(row, promise);
                promises.push(promise);
            }
        }

        for (i = 0; i < scope.deleted.length; ++i)
            promises.push(scope.persistDelete(scope.deleted[i]));

        $q.all(promises).then(function () {
            scope.loadAll();
            scope.deleted = [];
        });
    };

    var deleteRow = function (scope, row) {
        var index = scope.gridOptions.data.indexOf(row.entity);
        scope.deleted.push(row.entity);
        scope.gridOptions.data.splice(index, 1);
    }

    var addRow = function (scope, row) {
        scope.gridOptions.data.push(row);
    }

    var markAsDirty = function (scope, row) {
        scope.gridApi.rowEdit.setRowsDirty([row.entity]);
    }

    tablesFactory.setAllRowsClean = setAllRowsClean
    tablesFactory.calculateTableHeight = calculateTableHeight;
    tablesFactory.persistAllChanges = persistAllChanges;
    tablesFactory.initialize = initialize;
    tablesFactory.deleteRow = deleteRow;
    tablesFactory.addRow = addRow;
    tablesFactory.markAsDirty = markAsDirty;
    tablesFactory.addColumnDefinition = addColumnDefinition;

    return tablesFactory;
});
