using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace guia_turistico.Models
{
    public class SitiosTuristicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID Sitio Turístico")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200)]
        [Display(Name = "Nombre del Sitio")]
        public string? Nombre { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre (Inglés)")]
        public string? NombreIngles { get; set; }

        [StringLength(100)]
        [Display(Name = "Nombre (Portugués)")]
        public string? NombrePortugues { get; set; }

        [StringLength(1000)]
        [Display(Name = "Descripción (Español)")]
        public string? Descripcion { get; set; }

        [StringLength(1000)]
        [Display(Name = "Descripción (Inglés)")]
        public string? DescripcionIngles { get; set; }

        [StringLength(1000)]
        [Display(Name = "Descripción (Portugués)")]
        public string? DescripcionPortugues { get; set; }

        [Display(Name = "Dirección o Referencia")]
        [StringLength(250)]
        public string? Direccion { get; set; }

        // 🌍 Coordenadas para mapa
        [Required]
        [Display(Name = "Latitud")]
        public double Latitud { get; set; }

        [Required]
        [Display(Name = "Longitud")]
        public double Longitud { get; set; }

        // Tipo o categoría (tour) - singular para propiedad de navegación
        [Required]
        [Display(Name = "Tipo / Tour")]
        public int TipoId { get; set; }

        [ForeignKey("TipoId")]
        public virtual Tipos? Tipo { get; set; }

        // Galería de imágenes (plural para colección)
        public virtual ICollection<ImagenesSitios> Imagenes { get; set; } = new List<ImagenesSitios>();

        // Comentarios de usuarios (plural para colección)
        public virtual ICollection<Comentarios> Comentarios { get; set; } = new List<Comentarios>();

        // ⭐ Puntuación promedio (calculada)
        [NotMapped]
        [Display(Name = "Puntuación Promedio")]
        public double PuntuacionPromedio
        {
            get
            {
                if (Comentarios == null || !Comentarios.Any())
                    return 0;

                return Math.Round(Comentarios.Average(c => c.Puntuacion), 1);
            }
        }
    }
}