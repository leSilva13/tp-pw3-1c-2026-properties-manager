const chkGlobal = document.getElementById('chkIsGlobal');
const divUnidades = document.getElementById('divUnidades');
const buildingIdInput = document.getElementById('SelectedBuildingId');

if (chkGlobal) {
    chkGlobal.addEventListener('change', toggleUnidades);

    if (!chkGlobal.checked && divUnidades) {
        divUnidades.classList.remove('d-none');
        cargarUnidades();
    }
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
    const buildingId = buildingIdInput ? buildingIdInput.value : null;
    if (!buildingId) return;

    const response = await fetch(`/Expenses/ObtenerUnidadesPorEdificio?buildingId=${buildingId}`);
    const unidades = await response.json();
    const lista = document.getElementById('listaUnidades');
    if (!lista) return;

    lista.innerHTML = '';

    if (unidades.length === 0) {
        const span = document.createElement('span');
        span.className = 'text-muted';
        span.textContent = 'Este consorcio no tiene unidades.';
        lista.appendChild(span);
        return;
    }

    unidades.forEach(u => {
        const div = document.createElement('div');
        div.className = 'form-check';

        const input = document.createElement('input');
        input.className = 'form-check-input';
        input.type = 'checkbox';
        input.name = 'SelectedUnitIds';
        input.value = u.id;
        input.id = `unit_${u.id}`;

        const label = document.createElement('label');
        label.className = 'form-check-label';
        label.htmlFor = `unit_${u.id}`;
        label.textContent = u.nombre;

        div.appendChild(input);
        div.appendChild(label);
        lista.appendChild(div);
    });
}
