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
    public class AppUserRepositorio : Repositorio<AppUser>, IAppUserRepositorio
    {

        private readonly ApplicationDbContext _db;

        public AppUserRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


    }
}