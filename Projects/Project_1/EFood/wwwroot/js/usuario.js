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
            "url": "/Administracion/Usuario/ObtenerTodos"
        },
        "columns": [
            { "data": "email" },
            { "data": "nombre" },
            { "data": "apellidos" },
            { "data": "phoneNumber" },
            { "data": "preguntaSeguridad" },
            { "data": "respuestaSeguridad" },

            { "data": "role" },
            {
                "data": {
                    id: "id"
                }, 
            
                "render": function (data) {
                    return `
                           <div class="text-center">
                           <a onclick=IrEditarRol('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                              <i class="bi bi-pencil-square"></i>  
                        </div>`;
                }
            },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    let hoy = new Date().getTime();
                    let bloqueo = new Date(data.lockoutEnd).getTime();
                    if (bloqueo > hoy) {
                        // Usuario esta Bloqueado
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-danger text-white" style="cursor:pointer", width:150px >
                                    <i class="bi bi-unlock-fill"></i> Desbloquear
                               </a> 
                            </div>
                        `;
                    }
                    else {
                        return `
                            <div class="text-center">
                               <a onclick=BloquearDesbloquear('${data.id}') class="btn btn-success text-white" style="cursor:pointer", width:150px >
                                    <i class="bi bi-lock-fill"></i> Bloquear
                               </a> 
                            </div>
                        `;
                    }

                }
            }
        ]

    });
}

function ActualizarRol(id) {
    let nuevoRol = $('#selectRole').val();
    $.ajax({
        type: "POST",
        url: '/Administracion/Usuario/ActualizarRol',
        data: { id: id, nuevoRol: nuevoRol },
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

function IrEditarRol(id) {
    $.ajax({
        type: "POST",
        url: '/Administracion/Usuario/CheckEdit',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                window.location.href = '/Administracion/Usuario/Edit/' + id;
            } else {
                toastr.error(data.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error("An error occurred: " + error);
        }
    });
}
function BloquearDesbloquear(id) {
    $.ajax({
        type: "POST",
        url: '/Administracion/Usuario/BloquearDesbloquear',
        data: JSON.stringify(id),
        contentType: "application/json",
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