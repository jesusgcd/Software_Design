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
            "url": "/Cliente/Order/ObtenerTodos"
        },
        "columns": [
            { "data": "id", "width": "20%" },
            {
                "data": "date",
                "render": function (data) {
                    var dateObj = new Date(data);
                    var formattedDate = dateObj.toISOString().replace('T', ' ').slice(0, 19);
                    return formattedDate;
                }, "width": "20%"
            },
            {
                "data": "total", "className": "text-end",
                "render": function (data) {
                    var format = '₡' + data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return format;
                }, "width": "20%"
            },
            { "data": "paymentProcessor.type", "width": "20%" },
            { "data": "status", "width": "20%" }
        ]

    });
}



