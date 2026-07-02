using FiapCloudGames.Catalogs.Application.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces;
using FiapCloudGames.Catalogs.Domain.Pedidos.Interfaces.Messaging;

namespace FiapCloudGames.Catalogs.Application.Pedidos.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoDomainService _pedidoDomainService;
        private readonly IPedidoCriadoPublisher _pedidoCriadoPublisher;
        public PedidoService(IPedidoDomainService pedidoDomainService, IPedidoCriadoPublisher pedidoCriadoPublisher)
        {
            _pedidoDomainService = pedidoDomainService;
            _pedidoCriadoPublisher = pedidoCriadoPublisher;
        }
        public async Task ProcessarPedido(string nomeUsuario, string email, Guid idBiblioteca, Guid idJogo, decimal preco)
        {
            var pedido = await _pedidoDomainService.CriarAsync(nomeUsuario, email, idBiblioteca, idJogo);

            Console.WriteLine($"Pedido criado com sucesso! PedidoId: {pedido.Id}, NomeUsuario: {nomeUsuario}, Email: {email}, IdJogo: {idJogo}, IdBiblioteca: {idBiblioteca}, Valor: {preco}");
            await _pedidoCriadoPublisher.PublicarPedidoCriadoAsync(new Domain.Pedidos.Events.PedidoCriadoEvent
            {
                PedidoId = pedido.Id,
                NomeUsuario = nomeUsuario,
                Email = email,
                IdJogo = idJogo,
                IdBiblioteca = idBiblioteca,
                Valor = preco
            });
            Console.WriteLine($"Evento de pedido criado publicado com sucesso! PedidoId: {pedido.Id}, NomeUsuario: {nomeUsuario}, Email: {email}, IdJogo: {idJogo}, IdBiblioteca: {idBiblioteca}, Valor: {preco}");
        }
    }
}
