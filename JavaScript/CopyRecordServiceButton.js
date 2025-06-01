"use strict";

var DanielCabrasCrmProject = DanielCabrasCrmProject || {};

DanielCabrasCrmProject.CopyRecordTrigger = {

    callHSOCopyRecordService: function (selectedControl) {

        const grid = selectedControl.getGrid();
        const selectedRows = grid.getSelectedRows();
        const row = selectedRows.get(0);
        const rowData = row.getData();

        const entityReference = rowData.getEntity().getEntityReference();
        const entityId = entityReference.id.replace('{', '').replace('}', '');
        const entityName = entityReference.entityType;

        const actionName = "hso_copy_record_service";
        const requestUrl = Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/" + actionName;

        const data = {
            entityid: entityId,
            entityname: entityName
        };

        let req = new XMLHttpRequest();
        req.open("POST", requestUrl, true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        Xrm.Utility.showProgressIndicator("Copying record...");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                Xrm.Utility.closeProgressIndicator();
                req.onreadystatechange = null;
                if (this.status === 200 || this.status === 204) { 
                    selectedControl.refresh();
                    Xrm.Navigation.openAlertDialog({ text: "Record copied successfully." });
                } else {
                    Xrm.Navigation.openAlertDialog({ text: "Could not copy the record." });
                }
            }
        };
        req.send(JSON.stringify(data));
    }
};
