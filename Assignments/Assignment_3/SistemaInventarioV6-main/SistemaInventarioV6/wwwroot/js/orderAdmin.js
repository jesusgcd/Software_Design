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
            "url": "/Admin/Order/ObtenerTodos"
        },
        "columns": [
            { "data": "id"},
            { "data": "appUser.myUsername" },
            {
                "data": "date",
                "render": function (data) {
                    var dateObj = new Date(data);
                    var formattedDate = dateObj.toISOString().replace('T', ' ').slice(0, 19);
                    return formattedDate;
                }
            },
            {
                "data": "total", "className": "text-end",
                "render": function (data) {
                    var format = '₡' + data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return format;
                }
            },
            { "data": "paymentProcessor.type"},
            { "data": "status" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div>
                            <a href="/Admin/Order/Update/${data}" class="btn btn-success text-white" style="cursor:pointer">
                            <i class="bi bi-pencil-square"></i>
                            </a>
                        </div>
                    `;
                }, "width": "15%"
            },
            {
                "data": "status",
                "render": function (data, type, row) {
                    if (data === "En curso") {
                        return `
                            <div>
                                <a onclick=Delete("/Admin/Order/Delete/${row.id}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                                </a>
                            </div>
                        `;
                    } else {
                        return '';
                    }
                },
                "width": "15%"
            }
        ]

    });
}

function Delete(url) {
    swal({
        title: "¿Desea eliminar la orden?",
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


