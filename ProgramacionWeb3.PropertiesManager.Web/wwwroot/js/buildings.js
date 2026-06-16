/**
 * buildings.js
 *
 * Contiene la lógica JS para las vistas de Buildings:
 *
 * [Create / Edit]
 *   - Geolocalización automática via Nominatim al enviar el formulario.
 *   - Manejo del hidden `redirectAction` para los botones de guardado múltiple.
 *   Requiere en el DOM: txtCalle, txtNumero, comboProvincia, hfLatitud, hfLongitud.
 *   Opcional: comboLocalidad.
 *
 * [Index]
 *   - Modal de confirmación de eliminación (deleteModal).
 *   - Mapa Leaflet con marcadores de consorcios (mapaModal).
 */

document.addEventListener('DOMContentLoaded', function () {

    // ─── CREATE / EDIT: Geolocalización ──────────────────────────────────────

    const form = document.querySelector('form');
    if (form && document.getElementById('txtCalle')) {

        // Hidden que transporta qué botón fue presionado al controller
        const hfRedirect = document.createElement('input');
        hfRedirect.type = 'hidden';
        hfRedirect.name = 'redirectAction';
        hfRedirect.value = 'index';
        form.appendChild(hfRedirect);

        // Capturamos el value de cada botón submit antes de que el evento submit se dispare
        form.querySelectorAll('button[type="submit"][name="redirectAction"]').forEach(function (btn) {
            btn.addEventListener('click', function () {
                hfRedirect.value = btn.value;
            });
        });

        form.addEventListener('submit', function (event) {
            event.preventDefault();

            const calle = document.getElementById('txtCalle').value.trim();
            const numero = document.getElementById('txtNumero').value.trim();
            const comboProv = document.getElementById('comboProvincia');
            const provincia = comboProv?.options[comboProv.selectedIndex]?.text || 'Buenos Aires';
            const comboLoc = document.getElementById('comboLocalidad');
            const localidad = comboLoc?.options[comboLoc.selectedIndex]?.text || '';

            // Si faltan datos mínimos, enviamos sin coordenadas
            if (!calle || !numero) {
                form.submit();
                return;
            }

            const localidadQuery = localidad ? `${localidad}, ` : '';
            const query = `${calle} ${numero}, ${localidadQuery}${provincia}, Argentina`;
            const url = `https://nominatim.openstreetmap.org/search?format=json&limit=1&q=${encodeURIComponent(query)}`;

            console.log('Geocodificando:', url);

            fetch(url)
                .then(response => response.json())
                .then(data => {
                    if (data && data.length > 0) {
                        document.getElementById('hfLatitud').value = data[0].lat;
                        document.getElementById('hfLongitud').value = data[0].lon;
                        console.log('Coordenadas obtenidas:', data[0].lat, data[0].lon);
                    } else {
                        console.warn('Nominatim no encontró la dirección. Asignando 0.');
                        document.getElementById('hfLatitud').value = '0.0000000000';
                        document.getElementById('hfLongitud').value = '0.0000000000';
                    }
                })
                .catch(error => {
                    console.error('Error de red al geolocalizar:', error);
                    document.getElementById('hfLatitud').value = '0.0000000000';
                    document.getElementById('hfLongitud').value = '0.0000000000';
                })
                .finally(() => {
                    form.submit();
                });
        });
    }

    // ─── INDEX: Modal de eliminación ─────────────────────────────────────────

    const deleteModal = document.getElementById('deleteModal');
    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {
            const btn = event.relatedTarget;
            const buildingId = btn.getAttribute('data-building-id');
            const address = btn.getAttribute('data-building-address');

            document.getElementById('deleteModalAddress').textContent = address;
            document.getElementById('deleteForm').action = '/Buildings/Delete/' + buildingId;
        });
    }

    // ─── INDEX: Mapa Leaflet ──────────────────────────────────────────────────

    const mapaEl = document.getElementById('mapaConsorcios');
    if (mapaEl) {
        const map = L.map('mapaConsorcios').setView([-34.6037, -58.3816], 12);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        }).addTo(map);

        document.getElementById('mapaModal').addEventListener('shown.bs.modal', function () {
            map.invalidateSize();
        });

        fetch('/api/consorcios/listado-mapa')
            .then(response => {
                if (!response.ok) throw new Error('Error fetching map data');
                return response.json();
            })
            .then(data => {
                data.forEach(consorcio => {
                    if (consorcio.latitud && consorcio.longitud) {
                        const marker = L.marker([consorcio.latitud, consorcio.longitud]).addTo(map);
                        marker.bindPopup(`
                            <strong>${consorcio.direccion}</strong><br>
                            <a href="/Buildings/Details/${consorcio.id}" class="btn btn-sm btn-outline-primary mt-2">Ver Detalles</a>
                        `);
                    }
                });
            })
            .catch(error => console.error('Map Error:', error));
    }

});
