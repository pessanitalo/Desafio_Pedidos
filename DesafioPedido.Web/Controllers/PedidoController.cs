using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Validation;
using DesafioPedido.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoInterface _pedidoInterface;
        private readonly IClienteInterface _clienteInterface;
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
                Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data,
                Produtos = (await _produtoInterface.GetProdutosDisponiveisAsync()).Data,
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
        public async Task<IActionResult> Create(CriarPedidoViewModel viewModel)
        {
            var pedidoDto = new PedidoDTO
            {
                ClienteId = viewModel.ClienteId,
                Itens = viewModel.Pedido.Itens
            };

            var result = await _pedidoInterface.AddAsync(pedidoDto);

            if (!result.Success)
            {
                var vm = new CriarPedidoViewModel
                {
                    Pedido = viewModel.Pedido,
                    ClienteId = viewModel.ClienteId,
                    Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data,
                    Produtos = (await _produtoInterface.GetAllAsync()).Data,
                };
                ModelState.AddModelError("", result.Error);
                return View(vm);
            }

            TempData["ToastMessage"] = result.Data ?? "Pedido criado com sucesso.";
            TempData["ToastType"] = "success";
            TempData.Keep();
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
                Produtos = (await _produtoInterface.GetProdutosDisponiveisAsync()).Data,

                Itens = pedido.Itens.Select(i => new ItemPedidoViewModel
                {
                    ItemId = i.ItemId,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarPedidoViewModel model)
        {
            try
            {
                var dto = new EditarPedidoDTO
                {
                    PedidoId = model.PedidoId,
                    ClienteId = model.ClienteId,
                    Itens = model.Itens.Select(i => new ItemPedidoDTO
                    {
                        ItemId = i.ItemId > 0 ? i.ItemId : 0,
                        ProdutoId = i.ProdutoId,
                        Quantidade = i.Quantidade,
                        PrecoUnitario = i.PrecoUnitario
                    }).ToList()
                };

                var result = await _pedidoInterface.UpdateAsync(dto);
                if (!result.Success)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", result.Error);
                    model.Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data;
                    model.Produtos = (await _produtoInterface.GetAllAsync()).Data;
                    return View(model);
                }

                TempData["ToastMessage"] = "Pedido atualizado com sucesso.";
                TempData["ToastType"] = "success";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                model.Clientes = (await _clienteInterface.GetAllAsync(null, null)).Data;
                model.Produtos = (await _produtoInterface.GetAllAsync()).Data;
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _pedidoInterface.GetByIdAsync(id);

            if (!result.Success)
            {
                TempData["ToastMessage"] = result.Error ?? "Erro ao atualizar o pedido.";
                TempData["ToastType"] = "error";
                TempData.Keep();
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pedidoInterface.DeleteAsync(id);
            if (!result.Success)
            {
                TempData["ToastMessage"] = result.Error ?? "Erro ao excluir o pedido.";
                TempData["ToastType"] = "error";
                TempData.Keep();
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = result.Data ?? "Pedido excluido com sucesso.";
            TempData["ToastType"] = "success";
            TempData.Keep();
            return RedirectToAction("Index");
        }

    }
}
