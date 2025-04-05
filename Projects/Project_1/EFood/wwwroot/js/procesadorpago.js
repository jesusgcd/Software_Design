let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar página _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Administracion/ProcesadorPago/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre" },
            { "data": "metodoPago.nombre" },
            { "data": "pasarelaPago" },
            {
                "data": "requiereVerificacion",
                "render": function (data) {
                    if (data == true) {
                        return "Si Requiere";
                    }
                    else {
                        return "No Requiere";
                    }
                }
            },
            { "data": "metodoVerificacion" },
            {
                "data": "estado",
                "render": function (data) {
                    if (data == true) {
                        return "Activo";
                    }
                    else {
                        return "Inactivo";
                    }
                }
            },
            {
                "data": "id",
                "render": function (data, type, row) {
                    let buttons = `
                        <div class="text-center">
                           <a href="/Administracion/ProcesadorPago/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                           <a onclick=Delete("/Administracion/ProcesadorPago/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                           </a>
                    `;
                    if (row.metodoPago.nombre === "Tarjeta") {
                        buttons += `                                  
                           <a href="/Administracion/ProcesadorTarjeta/AdministrarTarjetas/${data}" class="btn btn-primary text-white" style="cursor:pointer">
                              <i class="bi bi-credit-card"></i>
                           </a>
                        `;
                    }
                    buttons += `</div>`;
                    return buttons;
                }
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "¿Está seguro de eliminar el procesador de pago?",
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
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
