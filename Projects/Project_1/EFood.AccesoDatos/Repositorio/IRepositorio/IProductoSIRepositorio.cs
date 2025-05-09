﻿using Microsoft.AspNetCore.Mvc.Rendering;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoSIRepositorio : IRepositorio<ProductoSI>
    {
        void Actualizar(ProductoSI ProductoSI);

        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

    }
}