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
            "url": "/Admin/PaymentProcessor/ObtenerTodos"
        },
        "columns": [
            { "data": "code" },
            { "data": "processor" },
            { "data": "type" },
            {
                "data": "status",
                "render": function (data) {
                    if (data == true) {
                        return "Sí";
                    }
                    else {
                        return "No";
                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div>
                            <a href="/Admin/PaymentProcessor/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/PaymentProcessor/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                            <i class="bi bi-trash3-fill"></i>
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
        title: "¿Desea eliminar el procesador?",
        text: "Este registro no se podrá recuperar",
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



