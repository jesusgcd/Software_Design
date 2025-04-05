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
    public class CuponRepositorio : Repositorio<Cupon>, ICuponRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CuponRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Cupon cupon)
        {
           var cuponesBD = _db.Cupones.FirstOrDefault(b => b.Id == cupon.Id);
            if(cuponesBD !=null)
            {
                cuponesBD.Codigo= cupon.Codigo;
                cuponesBD.Descripcion = cupon.Descripcion;
                cuponesBD.CantidadDisponible= cupon.CantidadDisponible;
                cuponesBD.Descuento= cupon.Descuento;
                _db.SaveChanges();
            }
        }
    }
}
