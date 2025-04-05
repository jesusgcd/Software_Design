using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio
{
    public class ErrorRepositorio : Repositorio<Error>, IErrorRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ErrorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Error error)
        {
            var errorDB = _db.Errors.FirstOrDefault(reg => reg.ID == error.ID);
            if(errorDB != null)
            {
                errorDB.Date = error.Date;
                errorDB.Description = error.Description;
                _db.SaveChanges();
            }
        }
    }
}