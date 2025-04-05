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
    public class PasarelaPagoRepositorio : Repositorio<PasarelaPago>, IPasarelaPagoRepostorio
    {

        private readonly ApplicationDbContext _db;

        public PasarelaPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PasarelaPago pasarelaPago)
        {
           var pasarelaPagoesBD = _db.PasarelaPago.FirstOrDefault(b => b.Id == pasarelaPago.Id);
            if(pasarelaPagoesBD !=null)
            {
                pasarelaPagoesBD.Nombre = pasarelaPago.Nombre;
                pasarelaPagoesBD.Descripcion = pasarelaPago.Descripcion;
                pasarelaPagoesBD.ApiKey = pasarelaPago.ApiKey;
                _db.SaveChanges();
            }
        }
    }
}
