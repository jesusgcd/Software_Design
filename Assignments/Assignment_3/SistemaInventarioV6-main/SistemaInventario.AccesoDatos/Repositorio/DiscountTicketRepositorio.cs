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
    public class DiscountTicketRepositorio : Repositorio<DiscountTicket>, IDiscountTicketRepositorio
    {

        private readonly ApplicationDbContext _db;

        public DiscountTicketRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(DiscountTicket discountTicket)
        {
            var discountTicketDB = _db.DiscountTickets.FirstOrDefault(reg => reg.ID == discountTicket.ID);
            if(discountTicketDB != null)
            {
                discountTicketDB.Code = discountTicket.Code;
                discountTicketDB.Name = discountTicket.Name;
                discountTicketDB.Stock = discountTicket.Stock;
                discountTicketDB.Percentage = discountTicket.Percentage;
                _db.SaveChanges();
            }
        }
    }
}