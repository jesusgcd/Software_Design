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
    public class OrdenDetalleRepositorio : Repositorio<OrdenDetalle>, IOrdenDetalleRepositorio
    {

        private readonly ApplicationDbContext _db;

        public OrdenDetalleRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(OrdenDetalle ordenDetalle)
        {
           var ordenDetalleBD = _db.OrdenDetalle.FirstOrDefault(b => b.Id == ordenDetalle.Id);
            if(ordenDetalleBD != null)
            {
                ordenDetalleBD.OrdenId = ordenDetalle.OrdenId;
                ordenDetalleBD.ProductoId = ordenDetalle.ProductoId;
                ordenDetalleBD.TipoPrecioId= ordenDetalle.TipoPrecioId;
                ordenDetalleBD.Cantidad= ordenDetalle.Cantidad;
                ordenDetalleBD.Monto = ordenDetalle.Monto;
                _db.SaveChanges();
            }
        }

        public async Task<List<OrdenDetalle>> ObteneOrdenDetallePorOrdenId(int id)
        {
            return await _db.OrdenDetalle
                .Include(o => o.Producto)
                .Include(o => o.TipoPrecio)
                .Where(o => o.OrdenId == id)
                .ToListAsync();
        }
    }
}
