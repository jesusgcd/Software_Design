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
    public class ProcesadorTarjetaRepositorio : Repositorio<ProcesadorTarjeta>, IProcesadorTarjetaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProcesadorTarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProcesadorTarjeta procesadortarjeta)
        {
            var procesadortarjetaDB = _db.ProcesadorTarjeta.FirstOrDefault(t => t.Id == procesadortarjeta.Id);
            if (procesadortarjetaDB != null)
            {
                procesadortarjetaDB.Id = procesadortarjeta.Id;
                procesadortarjetaDB.ProcesadorPago = procesadortarjeta.ProcesadorPago;
                procesadortarjetaDB.TipoTarjeta = procesadortarjeta.TipoTarjeta;
                _db.SaveChanges();
            }
        }

        public async Task<List<ProcesadorTarjeta>> ObtenerTarjetasPorProcesadorPago(int procesadorpagoId)
        {
            return await _db.ProcesadorTarjeta
                .Include(t => t.TipoTarjeta)
                .Where(t => t.ProcesadorPagoId == procesadorpagoId)
                .ToListAsync();
        }


    }
}
