using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Services
{
    public class ClienteService : IClienteInterface
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Result<string>> AddAsync(ClienteDTO clienteDTO)
        {

            try
            {
                var cliente = new Cliente(clienteDTO.Nome, clienteDTO.Email, clienteDTO.Telefone, DateTime.Now);
                await _clienteRepository.CreateAsync(cliente);
                return Result<string>.Ok("Cliente salvo com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível adicionar o cliente: {ex.Message}");
            }

        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente is null)
                    return Result<string>.Fail("Cliente não encontrado.");

                await _clienteRepository.DeleteAsync(id);
                return Result<string>.Ok("Cliente excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível remover o cliente: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ClienteDTO>>> GetAllAsync(string? nome, string? email)
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync(nome, email);

                var clientesDto = clientes.Select(p => new ClienteDTO
                {
                    ClienteId = p.ClienteId,
                    Nome = p.Nome,
                    Email = p.Email,
                    Telefone = p.Telefone,
                    DataCadastro = p.DataCadastro,
                });
                return Result<IEnumerable<ClienteDTO>>.Ok(clientesDto);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ClienteDTO>>.Fail($"Não foi consultar os clientes: {ex.Message}");
            }
        }

        public async Task<Result<Cliente>> GetByIdAsync(int id)
        {         
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);

                if (cliente is null)
                    return Result<Cliente>.Fail("Cliente não encontrado.");

                return Result<Cliente>.Ok(cliente);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Fail($"Não foi possível encontrar o cliente: {ex.Message}");
            }
        }

        public async Task<Result<string>> UpdateAsync(ClienteDTO clienteDTO)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(clienteDTO.ClienteId);

                if (cliente is null)
                    return Result<string>.Fail("Cliente não encontrado.");

                cliente.Nome = clienteDTO.Nome;
                cliente.Email = clienteDTO.Email;
                cliente.Telefone = clienteDTO.Telefone;
                cliente.DataCadastro = DateTime.Now;

                await _clienteRepository.UpdateAsync(cliente);
                return Result<string>.Ok("Cliente atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Erro ao atualizar: {ex.Message}");
            }
        }
    }
}
