using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoInterface _pedidoInterface;
        private readonly IClienteInterface  _clienteInterface;
        private readonly IProdutoInterface _produtoInterface;

        public PedidoController(IPedidoInterface pedidoInterface, IClienteInterface clienteInterface, IProdutoInterface produtoInterface)
        {
            _pedidoInterface = pedidoInterface;
            _clienteInterface = clienteInterface;   
            _produtoInterface = produtoInterface;
        }

        public async Task<IActionResult> Create()
        {
            var vm = new CriarPedidoViewModel
            {
                Clientes = (await _clienteInterface.GetAllAsync()).Data,
                Produtos = (await _produtoInterface.GetAllAsync()).Data
            };

            return View(vm);
        }

        public async Task<IActionResult> Index(int? clienteId, string? status)
        {
            var vm = new ListarPedidosViewModel
            {
                Clientes = (await _clienteInterface.GetAllAsync()).Data,
                ClienteIdFiltro = clienteId,
                StatusFiltro = status
            };

            var result = await _pedidoInterface.GetAllAsync(clienteId, status);
            if (result.Success)
                vm.Pedidos = result.Data;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoDTO pedido)
        {
            var result = await _pedidoInterface.AddAsync(pedido);

            if (!result.Success)
            {
                // recarrega as listas caso dê erro de estoque
                var vm = new CriarPedidoViewModel
                {
                    Pedido = pedido,
                    Clientes = (await _clienteInterface.GetAllAsync()).Data,
                    Produtos = (await _produtoInterface.GetAllAsync()).Data
                };
                ModelState.AddModelError("", result.Error);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _pedidoInterface.GetByIdAsync(id);

            if (!result.Success)
                return NotFound();

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _pedidoInterface.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
