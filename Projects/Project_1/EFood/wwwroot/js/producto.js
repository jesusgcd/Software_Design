let datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar page _PAGE_ de _PAGES_",
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
            "url": "/Administracion/Producto/ObtenerTodos"
        },
        "columns": [
            //llamarse igual que en el modelo
            { "data": "id"},
            { "data": "nombre" },
            { "data": "descripcion" },
            { "data": "lineaComida.nombre" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Administracion/Producto/MostrarPrecios/${data}" class="btn btn-success">
                                <i class="bi bi-tags"></i> Ver Precios
                            </a>
                           <a href="/Administracion/Producto/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                           </a>
                           <a onclick=Delete("/Administracion/Producto/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
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
        title: "Esta seguro de Eliminar el Producto?",
        text: "Este registro no se podra recuperar",
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