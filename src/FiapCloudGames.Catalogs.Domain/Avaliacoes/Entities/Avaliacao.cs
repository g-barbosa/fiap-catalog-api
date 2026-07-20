namespace FiapCloudGames.Catalogs.Domain.Avaliacoes.Entities
{
    public class Avaliacao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid JogoId { get; set; }
        public Guid UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public int Nota { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        protected Avaliacao() { }

        public Avaliacao(Guid jogoId, Guid usuarioId, string nomeUsuario, int nota, string comentario)
        {
            JogoId = jogoId;
            UsuarioId = usuarioId;
            NomeUsuario = nomeUsuario;
            Nota = nota;
            Comentario = comentario;
        }
    }
}