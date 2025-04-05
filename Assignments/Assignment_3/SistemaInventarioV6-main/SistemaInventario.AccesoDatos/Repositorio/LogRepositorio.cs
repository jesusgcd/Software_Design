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
    public class LogRepositorio : Repositorio<Log>, ILogRepositorio
    {

        private readonly ApplicationDbContext _db;

        public LogRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


    }
}