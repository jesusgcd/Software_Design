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
    public class FoodCategoryRepositorio : Repositorio<FoodCategory>, IFoodCategoryRepositorio
    {

        private readonly ApplicationDbContext _db;

        public FoodCategoryRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(FoodCategory foodCategory)
        {
            var foodCategoryDB = _db.FoodCategories.FirstOrDefault(reg => reg.ID == foodCategory.ID);
            if(foodCategoryDB != null)
            {
                foodCategoryDB.Name = foodCategory.Name;
                _db.SaveChanges();
            }
        }
    }
}