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
    public class MetodoPagoRepositorio : Repositorio<MetodoPago>, IMetodoPagoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public MetodoPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(MetodoPago metodoPago)
        {
           var metodoPagoPagoBD = _db.MetodoPago.FirstOrDefault(b => b.Id == metodoPago.Id);
            if(metodoPagoPagoBD != null)
            {
                metodoPagoPagoBD.Nombre = metodoPago.Nombre;
                _db.SaveChanges();
            }
        }
    }
}
