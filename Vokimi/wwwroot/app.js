﻿window.blazorDialog = {
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
function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
