using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class ProcesadorPagoRepositorio : Repositorio<ProcesadorPago>, IProcesadorPagoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProcesadorPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProcesadorPago procesadorPago)
        {
            var procesadorPagoBD = _db.ProcesadorPago.FirstOrDefault(b => b.Id == procesadorPago.Id);
            if (procesadorPagoBD != null)
            {
                procesadorPagoBD.Nombre = procesadorPago.Nombre;
                procesadorPagoBD.MetodoPagoId = procesadorPago.MetodoPagoId;
                procesadorPagoBD.PasarelaPago = procesadorPago.PasarelaPago;
                procesadorPagoBD.RequiereVerificacion = procesadorPago.RequiereVerificacion;
                procesadorPagoBD.MetodoVerificacion = procesadorPago.MetodoVerificacion;
                procesadorPagoBD.Estado = procesadorPago.Estado;

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
                return _db.TipoTarjeta.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }

			if (obj == "MetodoPago")
			{
				return _db.MetodoPago.Select(c => new SelectListItem
				{
					Text = c.Nombre,
					Value = c.Id.ToString()
				});
			}
			return null;
		}

    }

}
