﻿using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
	public interface ILineaComidaRepositorio : IRepositorio<LineaComida>
	{
        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

        void Actualizar(LineaComida lineaComida);

    }
}
