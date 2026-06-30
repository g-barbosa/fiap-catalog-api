using FiapCloudGames.Catalogs.Application.Bibliotecas.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Events;

namespace FiapCloudGames.Catalogs.Application.Pedidos
{
    public class ProcessaPagamentoService
    {
        private readonly IBibliotecaService _bibliotecaService;
        public ProcessaPagamentoService(IBibliotecaService bibliotecaService)
        {
            _bibliotecaService = bibliotecaService;
        }
        public async Task ProcessarPagamento(PagamentoProcessadoEvent evento)
        {
            Console.WriteLine($"Processando pagamento do pedido {evento.PedidoId} para o usuário {evento.NomeUsuario} com status {evento.Status} e valor {evento.Valor}");

            if (evento.Status == "Aprovado")
            {
                Console.WriteLine($"Pagamento aprovado para o pedido {evento.PedidoId}. Adicionando jogo {evento.IdJogo} à biblioteca {evento.IdBiblioteca} do usuário {evento.NomeUsuario}");
                await _bibliotecaService.AdicionarJogoAsync(evento.IdBiblioteca, evento.IdJogo);
                Console.WriteLine($"Jogo {evento.IdJogo} adicionado à biblioteca {evento.IdBiblioteca} do usuário {evento.NomeUsuario}");
            }
            else
            {
                Console.WriteLine($"Pagamento não aprovado para o pedido {evento.PedidoId}. Status: {evento.Status}");
            }
        }
    }
}
