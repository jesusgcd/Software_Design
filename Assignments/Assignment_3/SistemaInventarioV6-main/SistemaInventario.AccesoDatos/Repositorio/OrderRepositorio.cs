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
    public class OrderRepositorio : Repositorio<Order>, IOrderRepositorio
    {

        private readonly ApplicationDbContext _db;

        public OrderRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Order order)
        {
            var orderDB = _db.Orders.FirstOrDefault(reg => reg.ID == order.ID);
            if (orderDB != null)
            {
                orderDB.Status = order.Status;
                _db.SaveChanges();
            }
        }
    }
}