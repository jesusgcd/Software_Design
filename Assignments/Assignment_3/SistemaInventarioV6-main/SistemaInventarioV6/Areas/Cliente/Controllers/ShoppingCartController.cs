using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Modelos.ViewModels;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }

        public ShoppingCartController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM();
            shoppingCartVM.Order = new Modelos.Order();
            shoppingCartVM.ShoppingCartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(
                x => x.AppUserId == claim.Value,
                incluirPropiedades:"Price,Product"
            );
            shoppingCartVM.Order.Subtotal = 0;
            shoppingCartVM.Order.AppUserId = claim.Value;
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {
                item.Cost = item.Price.Cost;
                shoppingCartVM.Order.Subtotal += (item.Cost * item.Quantity);
            }
            return View(shoppingCartVM);
        }

        public async Task<IActionResult> addItems(int cartId)
        {
            var shoppingCart = await _unidadTrabajo.ShoppingCart.ObtenerPrimero(c => c.ID == cartId);
            shoppingCart.Quantity += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> substractItems(int cartId)
        {
            var shoppingCart = await _unidadTrabajo.ShoppingCart.ObtenerPrimero(c => c.ID == cartId);
            if (shoppingCart.Quantity == 1)
            {
                //Se remueve el registro del Carrito de Compras y actualizamos la sesión
                var cartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(
                                                c => c.AppUserId == shoppingCart.AppUserId);
                var productCount = cartList.Count();
                _unidadTrabajo.ShoppingCart.Remover(shoppingCart);
                await _unidadTrabajo.Guardar();
                HttpContext.Session.SetInt32(DS.ssShoppingCart, productCount - 1);
            }
            else
            {
                shoppingCart.Quantity -= 1;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> removeItem(int cartId)
        {
            var shoppingCart = await _unidadTrabajo.ShoppingCart.ObtenerPrimero(c => c.ID == cartId);
            var cartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(
                                            c => c.AppUserId == shoppingCart.AppUserId);
            var productCount = cartList.Count();
            _unidadTrabajo.ShoppingCart.Remover(shoppingCart);
            await _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.ssShoppingCart, productCount - 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Continue()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new ShoppingCartVM()
            {
                Order = new Modelos.Order(),
                ShoppingCartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(
                                            c => c.AppUserId == claim.Value, incluirPropiedades:"Product,Price"),
            };
            shoppingCartVM.Order.Subtotal = 0;
            shoppingCartVM.Order.AppUser = await _unidadTrabajo.AppUser.ObtenerPrimero(u => u.Id == claim.Value);
            shoppingCartVM.Order.AppUserId = claim.Value;
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {
                item.Cost = item.Price.Cost;
                shoppingCartVM.Order.Subtotal += (item.Cost * item.Quantity);

            }
            return View(shoppingCartVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CardDetails(ShoppingCartVM shoppingCartVM)
        {
            shoppingCartVM.CardList = _unidadTrabajo.ShoppingCart.ObtenerTodosDropDownList("Card", 0);
            shoppingCartVM.CardVM = new CardVM();
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckDetails(ShoppingCartVM shoppingCartVM)
        {
            shoppingCartVM.CheckVM = new CheckVM();
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(ShoppingCartVM shoppingCartVM)
        {
            shoppingCartVM.Order.Discount = 0;
            if (shoppingCartVM.Order.DiscountCode != null)
            {
                var ticket = await _unidadTrabajo.DiscountTicket.ObtenerPrimero(d => d.Code == shoppingCartVM.Order.DiscountCode);
                if (ticket != null && ticket.Stock > 0)
                {
                    double discountPercentage = (double)ticket.Percentage / 100.0;
                    double discountAmount = discountPercentage * shoppingCartVM.Order.Subtotal;
                    shoppingCartVM.Order.Discount = discountAmount;
                }
                    
            }
            shoppingCartVM.Order.Total = shoppingCartVM.Order.Subtotal - shoppingCartVM.Order.Discount;
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(ShoppingCartVM shoppingCartVM)
        {
            //Bajar el stock del tiquete de descuento, si se aplicó alguno
            if (shoppingCartVM.Order.Discount != 0)
            {
                var ticket = await _unidadTrabajo.DiscountTicket.ObtenerPrimero(d => d.Code == shoppingCartVM.Order.DiscountCode);
                ticket.Stock -= 1;
                await _unidadTrabajo.Guardar();
            }
            else
            {
                shoppingCartVM.Order.DiscountCode = "";
            }
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Setear los campos que faltan de la orden
            shoppingCartVM.Order.AppUserId = claim.Value;
            shoppingCartVM.Order.Date = DateTime.Now;
            shoppingCartVM.Order.Status = "En curso";
            var processor = await _unidadTrabajo.PaymentProcessor.ObtenerPrimero(x => x.Type == shoppingCartVM.Order.PaymentMethod && x.Status == true);
            shoppingCartVM.Order.PaymentProcessorId = processor.ID;
            //Agregar la orden
            await _unidadTrabajo.Order.Agregar(shoppingCartVM.Order);
            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Orden creada con el código: " + shoppingCartVM.Order.ID;
            //Generar el detalle de la orden
            shoppingCartVM.ShoppingCartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(
                                            c => c.AppUserId == claim.Value, incluirPropiedades: "Product,Price");
            foreach (var item in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = shoppingCartVM.Order.ID,
                    ProductId = item.ProductId,
                    PriceId = item.PriceId,
                    Quantity = item.Quantity,
                    Cost = item.Price.Cost
                };
                await _unidadTrabajo.OrderDetail.Agregar(orderDetail);
                await _unidadTrabajo.Guardar();

            }
            //Eliminar el carrito y la sesión
            List<ShoppingCart> cartList = shoppingCartVM.ShoppingCartList.ToList();
            _unidadTrabajo.ShoppingCart.RemoverRango(cartList);
            await _unidadTrabajo.Guardar();
            HttpContext.Session.SetInt32(DS.ssShoppingCart, 0);
            return RedirectToAction("Index", "Home", new { area = "Cliente" });
        }


        #region API
        [ActionName("ValidarTarjeta")]
        public async Task<IActionResult> ValidarTarjeta(int cardId)
        {
            var processorCardList = await _unidadTrabajo.ProcessorCard.ObtenerTodos(incluirPropiedades: "PaymentProcessor");
            //Si no hay un procesador asociado a la tarjeta que esté activo, no es válida
            bool issue = !processorCardList.Any(x => x.CardId == cardId && x.PaymentProcessor.Status == true);
            return Json(new { data = issue });

        }

        [ActionName("ValidarProcesadorCheque")]
        public async Task<IActionResult> ValidarProcesadorCheque()
        {
            var checkProcessorList = await _unidadTrabajo.PaymentProcessor.ObtenerTodos(x=>x.Type == "Cheque");
            //Si no hay un procesador activo de cheques, hay un problema
            bool issue = !checkProcessorList.Any(x => x.Status == true);
            return Json(new { data = issue });

        }
        #endregion
    }
}
