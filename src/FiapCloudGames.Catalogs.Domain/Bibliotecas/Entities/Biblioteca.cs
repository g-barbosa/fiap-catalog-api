using FiapCloudGames.Catalogs.Domain.Core;
using FiapCloudGames.Catalogs.Domain.Jogos.Entities;

namespace FiapCloudGames.Catalogs.Domain.Bibliotecas.Entities
{
    public class Biblioteca : EntityBase
    {
        public Guid UsuarioId { get; set; }
        public ICollection<Jogo> Jogos { get; set; } = new List<Jogo>();

        protected Biblioteca() { }

        public Biblioteca(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public void AdicionarJogo(Jogo jogo)
        {
            if (!Jogos.Any(j => j.Id == jogo.Id))
            {
                Jogos.Add(jogo);
                DataAtualizacao = DateTime.UtcNow;
            }
        }

        public bool RemoverJogo(Guid jogoId)
        {
            var jogo = Jogos.FirstOrDefault(j => j.Id == jogoId);
            if (jogo is null)
                return false;

            Jogos.Remove(jogo);
            DataAtualizacao = DateTime.UtcNow;
            return true;
        }
    }
}
