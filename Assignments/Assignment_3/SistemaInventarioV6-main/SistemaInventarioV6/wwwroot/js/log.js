let datatable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No hay registros",
            "info": "Mostrar página _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Log/ObtenerTodos"
        },
        "columns": [
            { "data": "id"},
            { "data": "appUser.myUsername"},
            { "data": "timeStamp" },
            { "data": "description" }
        ]

    });
}