// Logica de la pantalla Crear Usuario: muestra el campo "Nombre Completo"
// (dueño) solo cuando el rol seleccionado es Owner.

document.addEventListener('DOMContentLoaded', function () {
    const roleSelect = document.getElementById('roleSelect');
    const ownerNameContainer = document.getElementById('ownerNameContainer');
    const ownerNameInput = document.getElementById('OwnerFullName');

    // El id del rol Owner se expone desde la vista en data-owner-role-id.
    const targetRoleId = ownerNameContainer
        ? ownerNameContainer.getAttribute('data-owner-role-id')
        : null;

    function toggleOwnerName() {
        if (!roleSelect || !ownerNameContainer || !ownerNameInput) {
            return;
        }

        if (roleSelect.value === targetRoleId) {
            ownerNameContainer.classList.remove('d-none');
            ownerNameInput.setAttribute('required', 'required');
        } else {
            ownerNameContainer.classList.add('d-none');
            ownerNameInput.removeAttribute('required');
            ownerNameInput.value = '';
        }
    }

    if (roleSelect) {
        roleSelect.addEventListener('change', toggleOwnerName);
        toggleOwnerName();
    }
});
