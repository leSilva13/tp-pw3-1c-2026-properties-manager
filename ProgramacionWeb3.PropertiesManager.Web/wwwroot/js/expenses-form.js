// Muestra/oculta el panel de unidades segun el check "Gasto Global".
const chkGlobal = document.getElementById('chkIsGlobal');
const divUnidades = document.getElementById('divUnidades');

if (chkGlobal && divUnidades) {
    chkGlobal.addEventListener('change', function () {
        divUnidades.classList.toggle('d-none', chkGlobal.checked);
    });
}

// El comprobante es un IFormFile (no propiedad del modelo), por eso jQuery
// Validate no lo valida solo: agregamos la regla "required" a mano en el alta.
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
