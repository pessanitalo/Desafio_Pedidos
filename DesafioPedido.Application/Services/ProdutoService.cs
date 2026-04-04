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

        public async Task<Result<IEnumerable<ProdutoDto>>> GetAllAsync()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            var produtosDto = produtos.Select(p => new ProdutoDto
            {
                ProdutoId = p.ProdutoId,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque
            });
            return Result<IEnumerable<ProdutoDto>>.Ok(produtosDto);
        }

        public async Task<Result<IEnumerable<ProdutoDto>>> GetProdutosDisponiveisAsync()
        {
            var produtos = await _produtoRepository.GetProdutosDisponiveisAsync();
            var produtosDto = produtos.Select(p => new ProdutoDto
            {
                ProdutoId = p.ProdutoId,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                QuantidadeEstoque = p.QuantidadeEstoque
            });
            return Result<IEnumerable<ProdutoDto>>.Ok(produtosDto);
        }

        public async Task<Result<ProdutoDto>> GetByIdAsync(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto is null)
                return Result<ProdutoDto>.Fail("Produto não encontrado.");

            var produtoDto = new ProdutoDto
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                QuantidadeEstoque = produto.QuantidadeEstoque
            };

            return Result<ProdutoDto>.Ok(produtoDto);
        }

        public async Task<Result<string>> UpdateAsync(ProdutoDto produtoDTO)
        {
            var produto = await _produtoRepository.GetByIdAsync(produtoDTO.ProdutoId);

            if (produto is null)
                return Result<string>.Fail("Cliente não encontrado.");

            produto.Nome = produtoDTO.Nome;
            produto.Descricao = produtoDTO.Descricao;
            produto.Preco = produtoDTO.Preco;
            produto.QuantidadeEstoque = produtoDTO.QuantidadeEstoque;

            await _produtoRepository.UpdateAsync(produto);
            return Result<string>.Ok("Cliente atualizado com sucesso.");
        }
    }
}
