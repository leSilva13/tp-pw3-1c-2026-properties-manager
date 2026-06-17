// Funciones de uso comun en toda la aplicacion (referenciado desde _BaseLayout).

document.addEventListener('DOMContentLoaded', function () {

    // Confirmacion generica para formularios de accion destructiva.
    // Uso: <form data-confirm="¿Mensaje?"> en lugar de onsubmit inline.
    document.querySelectorAll('form[data-confirm]').forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!window.confirm(form.getAttribute('data-confirm'))) {
                event.preventDefault();
            }
        });
    });

    // Boton "Regresar" que vuelve a la pagina anterior.
    // Uso: <button data-history-back> en lugar de onclick inline.
    document.querySelectorAll('[data-history-back]').forEach(function (el) {
        el.addEventListener('click', function (event) {
            event.preventDefault();
            window.history.back();
        });
    });

});
