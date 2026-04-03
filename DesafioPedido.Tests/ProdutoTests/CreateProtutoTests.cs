using DesafioPedido.Application.DTOs;

namespace DesafioPedido.Tests.ProdutoTests
{
    public class CreateProtutoTests
    {
        [Fact]
        public void Deve_retornar_erro_quando_nome_for_vazio()
        {
            var dto = new ProdutoDto
            {
                Nome = "",
                Descricao = "teste@Descricao.com",
                Preco = 3.50m,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Nome"));
        }

        [Fact]
        public void Deve_retornar_o_tamanho_do_nome()
        {
            var dto = new ProdutoDto
            {
                Nome = "Pl",
                Descricao = "teste",
                Preco = 3.50m,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Nome"));
        }

        [Fact]
        public void Deve_retornar_erro_quando_o_Descricao_for_invalido()
        {
            var dto = new ProdutoDto
            {
                Nome = "It",
                Descricao = "",
                Preco = 3.50m,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Descricao"));
        }

        [Fact]
        public void Deve_validar_o_tamanho_minimo_da_descricao()
        {
            var dto = new ProdutoDto
            {
                Nome = "It",
                Descricao = "te",
                Preco = 3.50m,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Descricao"));
        }

        [Fact]
        public void Deve_validar_o_preco()
        {
            var dto = new ProdutoDto
            {
                Nome = "Lapis",
                Descricao = "lapis para desenho",
                Preco = 0,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Preco"));
        }

        [Fact]
        public void Deve_validar_a_quantidade_minima_do_produto()
        {
            var dto = new ProdutoDto
            {
                Nome = "Caderno",
                Descricao = "caderno de desenho",
                Preco = 6.50m,
                QuantidadeEstoque = 0
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("QuantidadeEstoque"));
        }

        [Fact]
        public void Deve_ser_valido_quando_dados_estao_corretos()
        {
            var dto = new ProdutoDto
            {
                Nome = "Caneta",
                Descricao = "caneta bic azul",
                Preco = 3.50m,
                QuantidadeEstoque = 150
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Empty(results);
        }
    }
}
