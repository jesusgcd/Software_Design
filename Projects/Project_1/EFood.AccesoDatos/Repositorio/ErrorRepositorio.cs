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
    public class ErrorRepositorio : Repositorio<Error>, IErrorRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ErrorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<Error> ObtenerErrores()
        {
            return _db.Errores;
        }


    }
}