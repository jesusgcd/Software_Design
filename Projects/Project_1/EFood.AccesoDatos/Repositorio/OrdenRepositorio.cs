using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class OrdenRepositorio : Repositorio<Orden>, IOrdenRepositorio
    {

        private readonly ApplicationDbContext _db;

        public OrdenRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Orden orden)
        {
            var ordenBD = _db.Orden.FirstOrDefault(b => b.Id == orden.Id);
            if (ordenBD != null)
            {
                ordenBD.ProcesadorPagoId = orden.ProcesadorPagoId;
                ordenBD.NombreCliente = orden.NombreCliente;
                ordenBD.ApellidosCliente = orden.ApellidosCliente;
                ordenBD.TelefonoCliente = orden.TelefonoCliente;
                ordenBD.Direccion = orden.Direccion;
                ordenBD.Estado = orden.Estado;
                ordenBD.CodigoCupon = orden.CodigoCupon;
                ordenBD.MontoTotal = orden.MontoTotal;
                ordenBD.FechaHora = orden.FechaHora;
                _db.SaveChanges();
            }
        }


        public async Task<List<Orden>> ObtenerTodosLista()
        {
            return await _db.Orden.ToListAsync();
        }

        public async Task<List<Orden>> ObtenerTodosLista(DateTime fechaInicio, DateTime fechaFin, String EstadoSeleccionado)
        {
            return await _db.Orden
                                  .Where(o => o.FechaHora >= fechaInicio &&
                                         o.FechaHora <= fechaFin &&
                                         o.Estado == EstadoSeleccionado)
                                   .ToListAsync();

        }
    }
}
