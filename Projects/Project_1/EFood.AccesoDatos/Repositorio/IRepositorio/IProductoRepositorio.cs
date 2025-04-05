﻿using Microsoft.AspNetCore.Mvc.Rendering;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto Producto);

        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
        public IEnumerable<Producto> ObtenerProductosPorLineaComida(int lineaComidaId);
    }
}