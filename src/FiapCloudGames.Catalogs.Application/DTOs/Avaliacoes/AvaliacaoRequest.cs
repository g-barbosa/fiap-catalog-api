using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Catalogs.Application.DTOs.Avaliacoes
{
    public class AvaliacaoRequest
    {
        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeUsuario { get; set; } = string.Empty;

        [Range(1, 5)]
        public int Nota { get; set; }

        [MaxLength(1000)]
        public string Comentario { get; set; } = string.Empty;
    }
}