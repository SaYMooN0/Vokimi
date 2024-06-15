window.blazorDialog = {
    openDialog: function (id) {
        var dialog = document.getElementById(id);
        if (dialog) {
            dialog.showModal();
        }
    },
    closeDialog: function (id) {
        var dialog = document.getElementById(id);
        if (dialog) {
            dialog.close();
        }
    }
};
