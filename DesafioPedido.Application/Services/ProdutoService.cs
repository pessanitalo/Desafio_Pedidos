using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Services
{
    public class ProdutoService : IProdutoInterface
    {
        private readonly IProdutoRepository  _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Result<string>> AddAsync(ProdutoDto produtoDTO)
        {

            var produto = new Produto(produtoDTO.Nome, produtoDTO.Descricao, produtoDTO.Preco, produtoDTO.QuantidadeEstoque);
            await _produtoRepository.CreateAsync(produto);
            return Result<string>.Ok("Cliente salvo com sucesso.");

        }

        public async Task<Result<string>> DeleteAsync(int id)
        {
            var cliente = await _produtoRepository.GetByIdAsync(id);

            if (cliente is null)
                return Result<string>.Fail("Cliente não encontrado.");

            await _produtoRepository.DeleteAsync(id);
            return Result<string>.Ok("Cliente excluído com sucesso.");
        }

        public async Task<Result<IEnumerable<Produto>>> GetAllAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            return Result<IEnumerable<Produto>>.Ok(produtos);
        }

        public async Task<Result<IEnumerable<Produto>>> GetProdutosDisponiveisAsync()
        {
            var produtos = await _produtoRepository.GetProdutosDisponiveisAsync();
            return Result<IEnumerable<Produto>>.Ok(produtos);
        }

        public async Task<Result<Produto>> GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto is null)
                return Result<Produto>.Fail("Cliente não encontrado.");

            return Result<Produto>.Ok(produto);
        }

        public async Task<Result<string>> UpdateAsync(ProdutoDto produtoDTO)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoDTO.ProdutoId);

            if (produto is null)
                return Result<string>.Fail("Cliente não encontrado.");

            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Preco = produtoDTO.Preco;

            await _produtoRepository.UpdateAsync(produto);
            return Result<string>.Ok("Cliente atualizado com sucesso.");
        }
    }
}
