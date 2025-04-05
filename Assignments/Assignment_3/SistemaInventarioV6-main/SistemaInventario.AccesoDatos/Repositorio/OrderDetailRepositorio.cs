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
    public class OrderDetailRepositorio : Repositorio<OrderDetail>, IOrderDetailRepositorio
    {

        private readonly ApplicationDbContext _db;

        public OrderDetailRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(OrderDetail orderDetail)
        {
            _db.Update(orderDetail);
        }
    }
}