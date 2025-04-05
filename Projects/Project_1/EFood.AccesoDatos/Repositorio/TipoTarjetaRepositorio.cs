using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class TipoTarjetaRepositorio : Repositorio<TipoTarjeta>, ITipoTarjetaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TipoTarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TipoTarjeta tipoTarjeta)
        {
           var tipotarjetasBD = _db.TipoTarjeta.FirstOrDefault(b => b.Id == tipoTarjeta.Id);
            if(tipotarjetasBD !=null)
            {
                tipotarjetasBD.Nombre = tipoTarjeta.Nombre;
                _db.SaveChanges();
            }
        }


        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            // Validar la entrada
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj), "El parámetro 'obj' no puede ser nulo o vacío.");
            }

            if (obj == "TipoTarjeta")
            {
                return _db.TipoTarjeta.Select(t => new SelectListItem
                {
                    Text = t.Nombre,
                    Value = t.Id.ToString()
                });
            }

            return null;
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerTodosDropdownListaAsync(string obj, List<ProcesadorTarjeta> listaProcesadorTarjeta)
        {
            // Validar la entrada
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj), "El parámetro 'obj' no puede ser nulo o vacío.");
            }

            if (obj == "TipoTarjeta")
            {
                // Obtener los IDs de TipoTarjeta desde la lista de ProcesadorTarjeta
                var tipoTarjetaIds = listaProcesadorTarjeta.Select(pt => pt.TipoTarjetaId).ToList();

                // Filtrar TipoTarjeta por los IDs obtenidos
                var tipoTarjetas = await _db.TipoTarjeta
                                            .Where(tt => tipoTarjetaIds.Contains(tt.Id))
                                            .Select(tt => new SelectListItem
                                            {
                                                Text = tt.Nombre,
                                                Value = tt.Id.ToString()
                                            })
                                            .ToListAsync();

                return tipoTarjetas;
            }

            return null;
        }

    }
}
