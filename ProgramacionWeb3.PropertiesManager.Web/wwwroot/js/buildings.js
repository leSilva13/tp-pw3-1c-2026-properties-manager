document.addEventListener("DOMContentLoaded", function () {
    // 1. Coordenadas por defecto (Centro de Argentina o San Justo por ejemplo)
    const defaultLat = -34.6686;
    const defaultLng = -58.5678;

    // 2. Inicializar el mapa de Leaflet apuntando al div 'mapaConsorcio'
    const map = L.map('mapaConsorcio').setView([defaultLat, defaultLng], 13);

    // 3. Cargar las imágenes del mapa (OpenStreetMap)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(map);

    // 4. Crear un marcador arrastrable (Marker)
    let marker = L.marker([defaultLat, defaultLng], { draggable: true }).addTo(map);

    // Función auxiliar para actualizar los inputs ocultos de C#
    function actualizarCoordenadas(lat, lng) {
        document.getElementById("hfLatitude").value = lat.toFixed(6);
        document.getElementById("hfLongitude").value = lng.toFixed(6);
    }

    // Inicializamos los inputs con el valor por defecto
    actualizarCoordenadas(defaultLat, defaultLng);

    // 5. Evento: Capturar cuando el usuario arrastra el marcador
    marker.on('dragend', function (event) {
        const position = marker.getLatLng();
        actualizarCoordenadas(position.lat, position.lng);
    });

    // 6. Evento: Capturar cuando el usuario hace clic en cualquier punto del mapa
    map.on('click', function (event) {
        const lat = event.latlng.lat;
        const lng = event.latlng.lng;

        marker.setLatLng([lat, lng]); // Mueve el pin al lugar del click
        actualizarCoordenadas(lat, lng);
    });

    // 7. Opcional/Requisito: Geocodificación simple si hacen clic en "Ubicar por Dirección"
    document.getElementById("btnBuscarDireccion").addEventListener("click", function () {
        const calle = document.getElementById("txtCalle").value;
        const numero = document.getElementById("txtNumero").value;
        const comboLoc = document.getElementById("comboLocalidad");
        const localidad = comboLoc.options[comboLoc.selectedIndex]?.text || "";

        if (!calle || !numero) {
            alert("Por favor, ingrese calle y número para buscar en el mapa.");
            return;
        }

        const query = `${calle} ${numero}, ${localidad}, Argentina`;

        // Consumimos la API pública y gratuita de Nominatim (OpenStreetMap) para buscar la lat/long
        fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(query)}`)
            .then(response => response.json())
            .then(data => {
                if (data && data.length > 0) {
                    const lat = parseFloat(data[0].lat);
                    const lon = parseFloat(data[0].lon);

                    map.setView([lat, lon], 16);
                    marker.setLatLng([lat, lon]);
                    actualizarCoordenadas(lat, lon);
                } else {
                    alert("No se pudo encontrar la dirección exacta en el mapa. Por favor, márquela manualmente.");
                }
            })
            .catch(error => console.error("Error en geocodificación:", error));
    });
});