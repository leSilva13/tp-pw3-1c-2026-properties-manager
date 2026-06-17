document.addEventListener("DOMContentLoaded", function () {
    const alerts = document.querySelectorAll('.auto-dismiss-alert');

alerts.forEach(function (alertElement) {
    setTimeout(function () {
        const bsAlert = new bootstrap.Alert(alertElement);
        bsAlert.close();
    }, 5000);
    });
});