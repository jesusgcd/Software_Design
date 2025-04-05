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
    public class SizeRepositorio : Repositorio<Size>, ISizeRepositorio
    {

        private readonly ApplicationDbContext _db;

        public SizeRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Size size)
        {
            var sizeDB = _db.Sizes.FirstOrDefault(reg => reg.ID == size.ID);
            if(sizeDB != null)
            {
                sizeDB.Description = size.Description;
                _db.SaveChanges();
            }
        }
    }
}