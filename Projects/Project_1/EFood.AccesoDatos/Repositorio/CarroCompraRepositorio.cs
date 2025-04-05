using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class CarroCompraRepositorio : Repositorio<CarroCompra>, ICarroCompraRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CarroCompraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(CarroCompra carroCompra)
        {
           var carroCompraBD = _db.CarroCompra.FirstOrDefault(b => b.Id == carroCompra.Id);
            if(carroCompraBD != null)
            {
                carroCompraBD.PrecioId = carroCompra.PrecioId;
                carroCompraBD.ProductoId = carroCompra.ProductoId;
                carroCompraBD.Cantidad= carroCompra.Cantidad;
                carroCompraBD.SesionUsuario= carroCompra.SesionUsuario;
                _db.SaveChanges();
            }
        }
    }
}
