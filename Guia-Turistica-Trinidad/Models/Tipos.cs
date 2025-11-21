using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace guia_turistico.Models
{
    // 🌿 Tipo o categoría de sitio (sirve también como sugerencia de tour)
    public class Tipos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Tipo")]
        public int TipoId { get; set; }

        // 🗣️ Nombre por idioma
        [Required(ErrorMessage = "El nombre del tipo es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre (Español)")]
        public string Nombre { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre (Inglés)")]
        public string? NombreIngles { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre (Portugués)")]
        public string? NombrePortugues { get; set; }

        // 📄 Descripciones por idioma
        [StringLength(500)]
        [Display(Name = "Descripción (Español)")]
        public string? Descripcion { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción (Inglés)")]
        public string? DescripcionIngles { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripción (Portugués)")]
        public string? DescripcionPortugues { get; set; }

        // 🖼️ Imagen representativa del tipo (ej. /images/tipos/aventura.jpg)
        [StringLength(250)]
        [Url(ErrorMessage = "La URL de la imagen debe ser válida")]
        [Display(Name = "Imagen del Tipo o Tour")]
        public string? ImagenUrl { get; set; }

        // 🔗 Relación con sitios turísticos (plural para colección)
        public virtual ICollection<SitiosTuristicos> Sitios { get; set; } = new List<SitiosTuristicos>();
    }
}