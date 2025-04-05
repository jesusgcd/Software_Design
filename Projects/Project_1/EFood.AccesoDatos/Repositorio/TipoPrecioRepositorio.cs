using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class TipoPrecioRepositorio : Repositorio<TipoPrecio>, ITipoPrecioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TipoPrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            // Validar la entrada
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj), "El parámetro 'obj' no puede ser nulo o vacío.");
            }

            if (obj == "TiposPrecio")
            {
                return _db.TipoPrecio.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                });
            }
            return null;
        }
        public void Actualizar(TipoPrecio tipoPrecio)
        {
            var tipoPreciosBD = _db.TipoPrecio.FirstOrDefault(b => b.Id == tipoPrecio.Id);
            if (tipoPreciosBD != null)
            {
                tipoPreciosBD.Nombre = tipoPrecio.Nombre;
                _db.SaveChanges();
            }
        }

    }
}
