using FiapCloudGames.Catalogs.Domain.Core;

namespace FiapCloudGames.Catalogs.Domain.Jogos.Entities
{
    public class Jogo : EntityBase
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }

        protected Jogo() { }

        public Jogo(string titulo, string descricao, decimal preco)
        {
            Titulo = titulo;
            Descricao = descricao;
            Preco = preco;
        }
    }
}
