function DVCLOpenSPDialog(strPageURL, width, height) {
    var dialogOptions = SP.UI.$create_DialogOptions();
    dialogOptions.url = strPageURL;// URL of the Page
    dialogOptions.width = width; // Width of the Dialog
    dialogOptions.height = height; // Height of the Dialog
    dialogOptions.dialogReturnValueCallback = Function.createDelegate(null, CloseCallback); // Function to capture dialog closed event
    SP.UI.ModalDialog.showModalDialog(dialogOptions); // Open the Dialog
    return false;
}

// Dialog close event capture function
function CloseCallback(strReturnValue, target) {
    if (strReturnValue === SP.UI.DialogResult.OK) // Perform action on Ok.
    {
    }
    if (strReturnValue === SP.UI.DialogResult.cancel) // Perform action on Cancel.
    {
    }
}