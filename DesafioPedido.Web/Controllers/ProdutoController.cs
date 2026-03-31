using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoInterface  _produtoInterface;

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
            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _produtoInterface.GetByIdAsync(id);

            var produtoDTO = new ProdutoDto
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
        public async Task<IActionResult> Delete(int id)
        {
            await _produtoInterface.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
