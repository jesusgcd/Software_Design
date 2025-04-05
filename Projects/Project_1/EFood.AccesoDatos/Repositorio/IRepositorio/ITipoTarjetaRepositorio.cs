using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
	public interface ITipoTarjetaRepositorio : IRepositorio<TipoTarjeta>
	{
        void Actualizar(TipoTarjeta TipoTarjetas);
        
        IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj);
        //Task<IEnumerable<SelectListItem>> ObtenerTodosDropdownListaAsync(int procesadorId, string obj);
        Task<IEnumerable<SelectListItem>> ObtenerTodosDropdownListaAsync(string obj, List<ProcesadorTarjeta> listaProcesadorTarjeta);



    }
}
