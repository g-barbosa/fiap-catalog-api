namespace FiapCloudGames.Catalogs.Application.Pedidos.Interfaces
{
    public interface IPedidoService
    {
        Task ProcessarPedido(string nomeUsuario, string email, Guid idBiblioteca, Guid idJogo, decimal preco);
    }
}
