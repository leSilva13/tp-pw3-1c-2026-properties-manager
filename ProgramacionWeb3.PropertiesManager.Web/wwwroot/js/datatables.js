$(document).ready(function () {
    $('#reservations-table').DataTable({
        language: {
            emptyTable: "No existen reservas.",
            zeroRecords: "No se encontraron resultados",
            info: "Mostrando _START_ a _END_ de _TOTAL_ reservas",
            infoEmpty: "Mostrando 0 a 0 de 0 reservas",
            search: "Buscar:",            
            paginate: {
                first: "Primero",
                last: "Último",
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });
    $('#expenses-table').DataTable({
        language: {
            emptyTable: "No existen reservas.",
            zeroRecords: "No se encontraron resultados",
            info: "Mostrando _START_ a _END_ de _TOTAL_ reservas",
            infoEmpty: "Mostrando 0 a 0 de 0 reservas",
            search: "Buscar:",
            paginate: {
                first: "Primero",
                last: "Último",
                next: "Siguiente",
                previous: "Anterior"
            }
        }
    });
});
