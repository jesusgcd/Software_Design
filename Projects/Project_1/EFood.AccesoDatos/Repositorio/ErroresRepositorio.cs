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
    public class ErroresRepositorio : Repositorio<Errores>, IErroresRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ErroresRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Errores error)
        {
           var erroresBD = _db.Errores.FirstOrDefault(b => b.Id == error.Id);
            if(erroresBD !=null)
            {
                erroresBD.CodigoError = error.CodigoError;
                erroresBD.Descripcion = error.Descripcion;
                erroresBD.FechaHora= error.FechaHora;
                _db.SaveChanges();
            }
        }
    }
}
