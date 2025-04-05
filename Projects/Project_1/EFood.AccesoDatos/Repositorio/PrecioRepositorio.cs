using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class PrecioRepositorio : Repositorio<Precio>, IPrecioRepositorio
	{

        private readonly ApplicationDbContext _db;

        public PrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Precio precio)
        {
            var precioBD = _db.Precio.FirstOrDefault(p => p.Id == precio.Id);
            if (precioBD != null)
            {
                precioBD.Id = precio.Id;
                precioBD.ProductoID = precio.ProductoID;
                precioBD.TipoPrecioID = precio.TipoPrecioID;
                precioBD.Monto= precio.Monto;
                _db.SaveChanges();
            }
        }

        public async Task<List<Precio>> ObtenerPreciosPorProducto(int productoId)
        {
            return await _db.Precio
                .Include(p => p.Producto)
                .Include(p => p.TipoPrecio)
                .Where(p => p.ProductoID == productoId)
                .ToListAsync();
        }
    }
}
