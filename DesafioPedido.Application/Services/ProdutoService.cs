using DesafioPedido.Application.DTOs;
using DesafioPedido.Application.Interfaces;
using DesafioPedido.Domain.Entities;
using DesafioPedido.Domain.Interfaces;
using DesafioPedido.Domain.Validation;

namespace DesafioPedido.Application.Services
{
    public class ProdutoService : IProdutoInterface
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task<Result<string>> AddAsync(ProdutoDto produtoDTO)
        {
            try
            {
                var produto = new Produto(produtoDTO.Nome, produtoDTO.Descricao, produtoDTO.Preco, produtoDTO.QuantidadeEstoque);
                await _produtoRepository.CreateAsync(produto);
                return Result<string>.Ok("Produto salvo com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível adicionar o produto: {ex.Message}");
            }

        }

        public async Task<Result<string>> DeleteAsync(int id)
        {

            try
            {
                var produtoPossuiEmUmPedido = await _produtoRepository.VerificarProdutoAoPedido(id);

                if (produtoPossuiEmUmPedido == true)
                    return Result<string>.Fail("Você não pode remover o produto, pois ele esta relacionado a um pedido.");

                var produto = await _produtoRepository.GetByIdAsync(id);

                if (produto is null)
                    return Result<string>.Fail("Produto não encontrado.");

                await _produtoRepository.DeleteAsync(id);
                return Result<string>.Ok("Produto excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível remover o produto: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ProdutoDto>>> GetAllAsync()
        {
            try
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
            catch (Exception ex)
            {
                return Result<IEnumerable<ProdutoDto>>.Fail($"Não foi possível pesquisar os produtos: {ex.Message}");
            }
        }

        public async Task<Result<IEnumerable<ProdutoDto>>> GetProdutosDisponiveisAsync()
        {
            try
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
            catch (Exception ex)
            {
                return Result<IEnumerable<ProdutoDto>>.Fail($"Não foi possível pesquisar os produtos disponíveis: {ex.Message}");
            }
        }

        public async Task<Result<ProdutoDto>> GetByIdAsync(int id)
        {

            try
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
            catch (Exception ex)
            {
                return Result<ProdutoDto>.Fail($"Não foi possível pesquisar o produto: {ex.Message}");
            }
        }

        public async Task<Result<string>> UpdateAsync(ProdutoDto produtoDTO)
        {

            try
            {
                var produto = await _produtoRepository.GetByIdAsync(produtoDTO.ProdutoId);

                if (produto is null)
                    return Result<string>.Fail("Produto não encontrado.");

                produto.Nome = produtoDTO.Nome;
                produto.Descricao = produtoDTO.Descricao;
                produto.Preco = produtoDTO.Preco;
                produto.QuantidadeEstoque = produtoDTO.QuantidadeEstoque;

                await _produtoRepository.UpdateAsync(produto);
                return Result<string>.Ok("Produto atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Não foi possível atualizar o Produto: {ex.Message}");
            }
        }
    }
}
