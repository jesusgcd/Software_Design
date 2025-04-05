using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio
{
    public class ProcessorCardRepositorio : Repositorio<ProcessorCard>, IProcessorCardRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProcessorCardRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == "PaymentProcessor")
            {   //Solo se retornan los procesadores que sean de tipo tarjeta
                return _db.PaymentProcessors.Where(c => c.Type == "Tarjeta").Select(c => new SelectListItem
                {
                    Text = c.Code,
                    Value = c.ID.ToString()
                }); ;
            }
            if (obj == "Card")
            {
                return _db.Cards.Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.ID.ToString()
                }); ;
            }
            return null;
        }
    }
}