using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProcesadorPagoRepositorio : IRepositorio<ProcesadorPago>
    {
        public void Actualizar(ProcesadorPago procesadorPago);

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);

    }
}
