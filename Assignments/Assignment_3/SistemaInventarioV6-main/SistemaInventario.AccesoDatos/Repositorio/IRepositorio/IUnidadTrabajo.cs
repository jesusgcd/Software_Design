using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IAppUserRepositorio AppUser { get; }

        ISizeRepositorio Size { get; }

        IFoodCategoryRepositorio FoodCategory { get; }

        ICardRepositorio Card { get; }

        IErrorRepositorio Error { get; }

        IDiscountTicketRepositorio DiscountTicket { get; }

        IProductRepositorio Product { get; }

        IPaymentProcessorRepositorio PaymentProcessor { get; }

        IProcessorCardRepositorio ProcessorCard { get; }

        IPriceRepositorio Price { get; }

        ILogRepositorio Log { get; }

        IShoppingCartRepositorio ShoppingCart { get; }

        IOrderRepositorio Order { get; }

        IOrderDetailRepositorio OrderDetail { get; }

        Task Guardar();
    }
}
