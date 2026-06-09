const chkGlobal = document.getElementById('chkIsGlobal');
const divUnidades = document.getElementById('divUnidades');
const buildingIdInput = document.getElementById('SelectedBuildingId');

if (chkGlobal) {
    chkGlobal.addEventListener('change', toggleUnidades);

    // Solo muestra/oculta el panel en carga inicial.
    // No llama a cargarUnidades() para no pisar las unidades pre-marcadas en Edit.
    if (!chkGlobal.checked && divUnidades) {
        divUnidades.classList.remove('d-none');
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

    const lista = document.getElementById('listaUnidades');
    if (!lista) return;

    try {
        const response = await fetch(`/Expenses/ObtenerUnidadesPorEdificio?buildingId=${buildingId}`);
        if (!response.ok) throw new Error();
        const unidades = await response.json();

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
    } catch {
        lista.innerHTML = '';
        const span = document.createElement('span');
        span.className = 'text-danger';
        span.textContent = 'No se pudieron cargar las unidades. Intentá nuevamente.';
        lista.appendChild(span);
    }
}
