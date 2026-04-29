using DesafioPedido.Application.DTOs;

namespace DesafioPedido.Tests.ClienteTests
{
    public class CreateClienteTests
    {
        [Fact]
        public void Deve_retornar_erro_quando_nome_for_vazio()
        {
            var dto = new ClienteDTO
            {
                Nome = "",
                Email = "teste@email.com",
                Telefone = "5511988914149"
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Nome"));
        }

        [Fact]
        public void Deve_retornar_o_tamanho_do_nome()
        {
            var dto = new ClienteDTO
            {
                Nome = "It",
                Email = "teste@email.com",
                Telefone = "(55) 11 99999-9999"
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Contains(results, r => r.MemberNames.Contains("Nome"));
        }

        [Fact]
        public void Deve_retornar_erro_quando_o_email_for_invalido()
        {
            var dto = new ClienteDTO
            {
                Nome = "It",
                Email = "testeemail.com",
                Telefone = "(55) 11 99999-9999"
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.True(results.Any(r =>
                r.MemberNames.Contains("Email") ||
                r.ErrorMessage.Contains("Email inválido")));
        }

        [Fact]
        public void Deve_ser_valido_quando_dados_estao_corretos()
        {
            var dto = new ClienteDTO
            {
                Nome = "Italo Pessan",
                Email = "teste@email.com",
                Telefone = "(55) 11 99999-9999"
            };

            var results = ValidationHelper.ValidarTests(dto);

            Assert.Empty(results);
        }
    }
}
