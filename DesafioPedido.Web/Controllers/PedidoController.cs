using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult CriarPedido()
        {
            return View();
        }
    }
}
