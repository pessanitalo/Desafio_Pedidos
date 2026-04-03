using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioPedido.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteInterface clienteInterface;

        public ClienteController(IClienteInterface clienteInterface)
        {
            this.clienteInterface = clienteInterface;
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Index(string? nome, string? email)
        {
            var result = await clienteInterface.GetAllAsync(nome, email);
            var vm = new ListarClientesViewModel
            {
                Clientes = result.Data,
                NomeFiltro = nome,
                EmailFiltro = email
            };

            return View(vm); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClienteDTO cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await clienteInterface.AddAsync(cliente);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await clienteInterface.GetByIdAsync(id);
            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await clienteInterface.GetByIdAsync(id);

            var clienteDTO = new ClienteDTO
            {
                ClienteId = result.Data.ClienteId,
                Nome = result.Data.Nome,
                Email = result.Data.Email,
                Telefone = result.Data.Telefone
            };

            return View(clienteDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteDTO dt)
        {
            await clienteInterface.UpdateAsync(dt);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await clienteInterface.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
