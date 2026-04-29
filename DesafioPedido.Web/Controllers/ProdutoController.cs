using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoInterface _produtoInterface;

        public ProdutoController(IProdutoInterface produtoInterface)
        {
            _produtoInterface = produtoInterface;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var result = await _produtoInterface.GetAllAsync();
            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProdutoDto produto)
        {
            if (!ModelState.IsValid)
                return View(produto);

            await _produtoInterface.AddAsync(produto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _produtoInterface.GetByIdAsync(id);

            var produtoViewModel = new ProdutoViewModel
            {
                ProdutoId = result.Data.ProdutoId,
                Nome = result.Data.Nome,
                Descricao = result.Data.Descricao,
                Preco = result.Data.Preco,
                QuantidadeEstoque = result.Data.QuantidadeEstoque,
            };

            return View(produtoViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _produtoInterface.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["ToastMessage"] = result.Error ?? "Erro ao pesquisar o produto.";
                TempData["ToastType"] = "error";
                TempData.Keep();
                return RedirectToAction("Index");
            }

            var produtoDTO = new ProdutoViewModel
            {
                ProdutoId = result.Data.ProdutoId,
                Nome = result.Data.Nome,
                Descricao = result.Data.Descricao,
                Preco = result.Data.Preco,
                QuantidadeEstoque = result.Data.QuantidadeEstoque
            };

            return View(produtoDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoDto dt)
        {
            var result = await _produtoInterface.UpdateAsync(dt);
            if (!result.Success)
            {
                TempData["ToastMessage"] = result.Error ?? "Não foi possível editar o produto.";
                TempData["ToastType"] = "error";
                TempData.Keep();
                return RedirectToAction("Index");
            }

            TempData["ToastMessage"] = result.Data ?? "Produto atualizado com sucesso.";
            TempData["ToastType"] = "success";
            TempData.Keep();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _produtoInterface.DeleteAsync(id);
            if (!result.Success)
            {
                TempData["ToastMessage"] = result.Error ?? "Não foi possível excluir o produto.";
                TempData["ToastType"] = "error";
                TempData.Keep();
                return RedirectToAction("Index");
            }
            TempData["ToastMessage"] = result.Data ?? "Produto ecluido com sucesso.";
            TempData["ToastType"] = "success";
            TempData.Keep();
            return RedirectToAction("Index");
        }
    }
}
