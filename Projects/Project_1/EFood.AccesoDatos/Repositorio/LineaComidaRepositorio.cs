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
    public class LineaComidaRepositorio : Repositorio<LineaComida>, ILineaComidaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public LineaComidaRepositorio(ApplicationDbContext db) : base(db)
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

            if (obj == "LineaComida")
            {
                return _db.LineaComida.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }

            return null;
        }
        public void Actualizar(LineaComida lineaComida)
        {
            var lineaComidasBD = _db.LineaComida.FirstOrDefault(b => b.Id == lineaComida.Id);
            if (lineaComidasBD != null)
            {
                lineaComidasBD.Nombre = lineaComida.Nombre;
                _db.SaveChanges();
            }
        }

    }
}