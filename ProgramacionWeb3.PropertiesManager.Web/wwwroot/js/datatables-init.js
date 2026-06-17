// Inicializa DataTables sobre cualquier tabla marcada con la clase .js-datatable.
// Requiere que la libreria DataTables ya este cargada en la pagina.

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.js-datatable').forEach(function (table) {
        new DataTable(table);
    });
});
