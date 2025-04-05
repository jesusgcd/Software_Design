using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IOrdenRepositorio : IRepositorio<Orden>
    {
        void Actualizar(Orden orden);
        Task<List<Orden>> ObtenerTodosLista();
		Task<List<Orden>> ObtenerTodosLista(DateTime fechaInicio, DateTime fechaFin, String EstadoSeleccionado);


	}
}
