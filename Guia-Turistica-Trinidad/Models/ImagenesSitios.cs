using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace guia_turistico.Models
{
    // 🖼️ Imagen del sitio turístico
    public class ImagenesSitios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Imagen")]
        public int Id { get; set; }

        [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
        [StringLength(3000)]
        [Url(ErrorMessage = "La URL debe tener un formato válido")]
        [Display(Name = "URL de Imagen")]
        public required string Url { get; set; }

        [Required]
        [Display(Name = "Sitio Turístico")]
        public int SitioTuristicoId { get; set; }

        [ForeignKey("SitioTuristicoId")]
        public virtual SitiosTuristicos? SitioTuristico { get; set; }
    }
}