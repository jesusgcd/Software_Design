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
            "url": "/Admin/ProcessorCard/ObtenerTodos"
        },
        "columns": [
            { "data": "paymentProcessor.code" },
            { "data": "card.description"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div>
                            <a onclick=Delete("/Admin/ProcessorCard/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="bi bi-dash-square-fill"></i>
                            </a>
                        </div>
                    `;
                }
            }
        ]

    });
}

function Delete(url) {
    swal({
        title: "¿Desea desasignar la tarjeta del procesador? ",
        text: "Esta acción no se puede revertir",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}



