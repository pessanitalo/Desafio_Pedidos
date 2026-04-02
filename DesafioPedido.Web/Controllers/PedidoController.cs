using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
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

        public async Task<IActionResult> Create(string? nome, string? email)
        {
            var vm = new CriarPedidoViewModel
            {
                Clientes = (await _clienteInterface.GetAllAsync(nome,email)).Data,
                Produtos = (await _produtoInterface.GetProdutosDisponiveisAsync()).Data
            };

            return View(vm);
        }

        public async Task<IActionResult> Index(int? clienteId, string? status)
        {
            var vm = new ListarPedidosViewModel
            {
                Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data,
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
                var vm = new CriarPedidoViewModel
                {
                    Pedido = pedido,
                    Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data,
                    Produtos = (await _produtoInterface.GetAllAsync()).Data
                };
                ModelState.AddModelError("", result.Error);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _pedidoInterface.GetByIdAsync(id);

            if (!result.Success)
                return NotFound();

            var pedido = result.Data;

            var viewModel = new EditarPedidoViewModel
            {
                PedidoId = pedido.PedidoId,
                ClienteId = pedido.ClienteId,

                Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data,
                Produtos = (await _produtoInterface.GetAllAsync()).Data,

                Itens = pedido.Itens.Select(i => new ItemPedidoViewModel
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };

            return View(viewModel);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarPedidoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data;
                model.Produtos = (await _produtoInterface.GetAllAsync()).Data;

                return View(model);
            }

            try
            {
                var pedido = new Pedido
                {
                    PedidoId = model.PedidoId,
                    ClienteId = model.ClienteId,
                    DataPedido = DateTime.Now,
                    Status = "Atualizado",     
                    ValorTotal = model.Itens.Sum(i => i.Quantidade * i.PrecoUnitario)
                };

                var itens = model.Itens.Select(i => new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList();

                await _pedidoInterface.UpdateAsync(pedido, itens);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar o pedido.");

                model.Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data;
                model.Produtos = (await _produtoInterface.GetAllAsync()).Data;

                return View(model);
            }
        }
    }
}
