// Logica del formulario de gasto (alta/edicion).
// Las unidades del consorcio se renderizan server-side; aca solo mostramos u
// ocultamos el panel segun el check "Gasto Global". La visibilidad inicial ya
// la resuelve el server con la clase d-none, asi que solo escuchamos el cambio.

const chkGlobal = document.getElementById('chkIsGlobal');
const divUnidades = document.getElementById('divUnidades');

if (chkGlobal && divUnidades) {
    chkGlobal.addEventListener('change', function () {
        divUnidades.classList.toggle('d-none', chkGlobal.checked);
    });
}

// Validacion client-side del comprobante.
// El input file no es propiedad del modelo, asi que jQuery Validate no lo
// valida por data-val. Agregamos la regla "required" manualmente, pero solo
// cuando el campo esta marcado como requerido (alta de gasto, no edicion).
(function () {
    if (typeof window.jQuery === 'undefined') return;
    const $ = window.jQuery;

    $(function () {
        const $file = $('#receiptFile');
        if ($file.length === 0 || $file.data('comprobante-requerido') !== true) return;

        const $form = $file.closest('form');
        if (!$form.data('validator')) {
            $form.validate();
        }

        $file.rules('add', {
            required: true,
            messages: { required: 'El comprobante es requerido.' }
        });
    });
})();
