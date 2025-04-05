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
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IAppUserRepositorio AppUser { get; private set; }
        public ISizeRepositorio Size { get; private set; }
        public IFoodCategoryRepositorio FoodCategory { get; private set; }

        public ICardRepositorio Card { get; private set; }

        public IErrorRepositorio Error { get; private set; }
        public IDiscountTicketRepositorio DiscountTicket{ get; private set; }

        public IProductRepositorio Product { get; private set; }

        public IPaymentProcessorRepositorio PaymentProcessor { get; private set; }

        public IProcessorCardRepositorio ProcessorCard { get; private set; }

        public IPriceRepositorio Price { get; private set; }

        public ILogRepositorio Log { get; private set; }

        public IShoppingCartRepositorio ShoppingCart { get; private set; }

        public IOrderRepositorio Order { get; private set; }

        public IOrderDetailRepositorio OrderDetail { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            AppUser = new AppUserRepositorio(_db);
            Size = new SizeRepositorio(_db);
            FoodCategory = new FoodCategoryRepositorio(_db);
            Card = new CardRepositorio(_db);
            Error = new ErrorRepositorio(_db);
            DiscountTicket = new DiscountTicketRepositorio(_db);
            Product = new ProductRepositorio(_db);
            PaymentProcessor = new PaymentProcessorRepositorio(_db);
            ProcessorCard = new ProcessorCardRepositorio(_db);
            Price = new PriceRepositorio(_db);
            Log = new LogRepositorio(_db);
            ShoppingCart = new ShoppingCartRepositorio(_db);
            Order = new OrderRepositorio(_db);
            OrderDetail = new OrderDetailRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
