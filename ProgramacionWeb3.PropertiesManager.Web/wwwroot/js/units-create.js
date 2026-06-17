// Logica de la pantalla Crear Unidad: alterna entre seleccionar un dueño
// existente o cargar los datos de un dueño nuevo segun el checkbox.

document.addEventListener('DOMContentLoaded', function () {
    const checkboxCrearNuevoDuenio = document.getElementById('crearNuevoOwner');
    const bloqueDuenioExistente = document.getElementById('ownerExistente');
    const bloqueNuevoDuenio = document.getElementById('nuevoOwner');

    if (!checkboxCrearNuevoDuenio || !bloqueDuenioExistente || !bloqueNuevoDuenio) {
        return;
    }

    function actualizarFormularioDuenio() {
        const quiereCrearNuevoDuenio = checkboxCrearNuevoDuenio.checked;
        const selectorDuenios = bloqueDuenioExistente.querySelector('select');
        const camposNuevoDuenio = bloqueNuevoDuenio.querySelectorAll('input');

        bloqueDuenioExistente.classList.toggle('d-none', quiereCrearNuevoDuenio);
        bloqueNuevoDuenio.classList.toggle('d-none', !quiereCrearNuevoDuenio);
        selectorDuenios.disabled = quiereCrearNuevoDuenio;

        camposNuevoDuenio.forEach(function (campo) {
            campo.disabled = !quiereCrearNuevoDuenio;
        });
    }

    checkboxCrearNuevoDuenio.addEventListener('change', actualizarFormularioDuenio);
    actualizarFormularioDuenio();
});
