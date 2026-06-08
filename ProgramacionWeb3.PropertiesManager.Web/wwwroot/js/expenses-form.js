const chkGlobal = document.getElementById('chkIsGlobal');
const divUnidades = document.getElementById('divUnidades');
const buildingIdInput = document.getElementById('SelectedBuildingId');

chkGlobal.addEventListener('change', toggleUnidades);

if (!chkGlobal.checked) {
    divUnidades.classList.remove('d-none');
    cargarUnidades();
}

function toggleUnidades() {
    if (chkGlobal.checked) {
        divUnidades.classList.add('d-none');
    } else {
        divUnidades.classList.remove('d-none');
        cargarUnidades();
    }
}

async function cargarUnidades() {
    const buildingId = buildingIdInput.value;
    if (!buildingId) return;

    const response = await fetch(`/Expenses/ObtenerUnidadesPorEdificio?buildingId=${buildingId}`);
    const unidades = await response.json();
    const lista = document.getElementById('listaUnidades');

    if (unidades.length === 0) {
        lista.innerHTML = '<span class="text-muted">Este edificio no tiene unidades.</span>';
        return;
    }

    lista.innerHTML = unidades.map(u => `
        <div class="form-check">
            <input class="form-check-input" type="checkbox" name="SelectedUnitIds" value="${u.id}" id="unit_${u.id}" />
            <label class="form-check-label" for="unit_${u.id}">${u.nombre}</label>
        </div>
    `).join('');
}
