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
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public CategoriaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Categoria categoria)
        {
           var categoriaBD = _db.Categorias.FirstOrDefault(b => b.Id == categoria.Id);
            if(categoriaBD != null)
            {
                categoriaBD.Nombre= categoria.Nombre;
                categoriaBD.Descripcion = categoria.Descripcion;
                categoriaBD.Estado= categoria.Estado;
                _db.SaveChanges();
            }
        }
    }
}
