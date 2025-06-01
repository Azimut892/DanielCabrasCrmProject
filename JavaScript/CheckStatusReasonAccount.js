"use strict";

var DanielCabrasCrmProject = DanielCabrasCrmProject || {};

DanielCabrasCrmProject.CheckAccount = {

    validateAccount: function (executionContext) {

        const formContext = executionContext.getFormContext();

        const statusCodeHeaderValue = formContext.getAttribute("statuscode").getValue();

        const accountValue = formContext.getAttribute("daca_account_id").getValue();
        const accountControl = formContext.getControl("daca_account_id");

        if (accountValue != null) {

            formContext.ui.setFormNotification("Account ID is: " + accountValue[0].id, "INFO", "account_Id");

        }

        if (statusCodeHeaderValue === 303950003 && accountValue === null) {


            accountControl.setNotification("Please fill the Account field.", "accountError");
            formContext.ui.clearFormNotification("account_Id");

        } else {
            accountControl.clearNotification("accountError");
        }
    },

    checkChanges: function (executionContext) {

        const formContext = executionContext.getFormContext();

        const accountAttribute = formContext.getAttribute("daca_account_id");
        const statusCodeAttribute = formContext.getAttribute("statuscode");

        if (accountAttribute) {

            accountAttribute.addOnChange(DanielCabrasCrmProject.CheckAccount.validateAccount);
        }

        if (statusCodeAttribute) {

            statusCodeAttribute.addOnChange(DanielCabrasCrmProject.CheckAccount.validateAccount);
        }
    }
};
