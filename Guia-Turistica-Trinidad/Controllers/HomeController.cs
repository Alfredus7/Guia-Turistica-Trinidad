using Guia_Turistica_Trinidad.Data;
using Guia_Turistica_Trinidad.Models;
using guia_turistico.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Guia_Turistica_Trinidad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            // Cargar tipos para el menú de categorías
            var tipos = _context.Tipos.ToList();

            // Calcular top 3 sitios mejor puntuados con su imagen principal
            var topSitios = _context.SitiosTuristicos
                .Include(s => s.Imagenes)
                .Include(s => s.Comentarios)
                .Include(s => s.Tipo)
                .Select(s => new
                {
                    Sitio = s,
                    ImagenUrl = s.Imagenes.FirstOrDefault().Url ?? "/images/default-sitio.jpg",
                    Promedio = s.Comentarios.Any() ? s.Comentarios.Average(c => c.Puntuacion) : 0
                })
                .OrderByDescending(x => x.Promedio)
                .Take(3)
                .ToList();

            ViewBag.TopSitios = topSitios;
            ViewBag.Tipos = tipos; // También pasamos los tipos al ViewBag por si los necesitas

            return View(tipos); // Seguimos pasando tipos como modelo principal
        }

        // GET: Home/Details/5 - Detalles de un sitio turístico
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitioTuristico = await _context.SitiosTuristicos
                .Include(s => s.Tipo)
                .Include(s => s.Imagenes)
                .Include(s => s.Comentarios)
                    .ThenInclude(c => c.Usuario) // Carga el usuario del comentario
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sitioTuristico == null)
            {
                return NotFound();
            }

            // Ordenar comentarios por fecha (más recientes primero)
            if (sitioTuristico.Comentarios.Any())
            {
                sitioTuristico.Comentarios = sitioTuristico.Comentarios
                    .OrderByDescending(c => c.Fecha)
                    .ToList();
            }

            return View(sitioTuristico);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Mapa interactivo
        public IActionResult Mapa()
        {
            var sitios = _context.SitiosTuristicos
                .Include(s => s.Tipo)
                .Include(s => s.Imagenes)
                .ToList();
            return View(sitios);
        }

        // Sitios por tipo/categoría
        public async Task<IActionResult> PorTipo(int tipoId)
        {
            var tipo = await _context.Tipos.FirstOrDefaultAsync(t => t.TipoId == tipoId);

            if (tipo == null)
                return NotFound();

            var sitios = await _context.SitiosTuristicos
                .Where(s => s.TipoId == tipoId)
                .Include(s => s.Tipo)
                .Include(s => s.Imagenes)
                .Include(s => s.Comentarios)
                .ToListAsync();

            ViewBag.TipoNombre = tipo.Nombre;
            ViewBag.TipoNombreIngles = tipo.NombreIngles;
            ViewBag.TipoNombrePortugues = tipo.NombrePortugues;
            ViewBag.TipoId = tipoId;

            return View(sitios);
        }

        // Agregar comentario (desde cualquier página)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarComentario(int sitioId, int puntuacion, string texto)
        {
            if (puntuacion < 1 || puntuacion > 5 || string.IsNullOrWhiteSpace(texto))
            {
                TempData["Error"] = "Debe completar todos los campos y seleccionar una puntuación válida.";
                return RedirectToAction("Details", "Home", new { id = sitioId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var comentarioExistente = await _context.Comentarios
                .FirstOrDefaultAsync(c => c.SitioTuristicoId == sitioId && c.UsuarioId == user.Id);

            if (comentarioExistente != null)
            {
                comentarioExistente.Texto = texto;
                comentarioExistente.Puntuacion = puntuacion;
                comentarioExistente.Fecha = DateTime.UtcNow;
                _context.Comentarios.Update(comentarioExistente);
                TempData["Mensaje"] = "Tu comentario ha sido actualizado.";
            }
            else
            {
                var comentario = new Comentarios
                {
                    SitioTuristicoId = sitioId,
                    Texto = texto,
                    Puntuacion = puntuacion,
                    UsuarioId = user.Id,
                    Fecha = DateTime.UtcNow
                };
                _context.Comentarios.Add(comentario);
                TempData["Mensaje"] = "Tu comentario ha sido agregado.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Home", new { id = sitioId });
        }

        //// Sitios recomendados (mejor puntuados)
        //public async Task<IActionResult> Recomendados()
        //{
        //    var sitios = await _context.SitiosTuristicos
        //        .Include(s => s.Tipo)
        //        .Include(s => s.Imagenes)
        //        .Include(s => s.Comentarios)
        //        .Where(s => s.Comentarios.Any())
        //        .OrderByDescending(s => s.Comentarios.Average(c => c.Puntuacion))
        //        .Take(10)
        //        .ToListAsync();

        //    return View(sitios);
        //}

        //// Búsqueda de sitios
        //public async Task<IActionResult> Buscar(string query)
        //{
        //    if (string.IsNullOrWhiteSpace(query))
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var sitios = await _context.SitiosTuristicos
        //        .Where(s => s.Nombre.Contains(query) ||
        //                   s.Descripcion.Contains(query) ||
        //                   s.NombreIngles.Contains(query) ||
        //                   s.NombrePortugues.Contains(query) ||
        //                   s.DescripcionIngles.Contains(query) ||
        //                   s.DescripcionPortugues.Contains(query) ||
        //                   s.Tipo.Nombre.Contains(query))
        //        .Include(s => s.Tipo)
        //        .Include(s => s.Imagenes)
        //        .Include(s => s.Comentarios)
        //        .ToListAsync();

        //    ViewBag.Query = query;
        //    return View(sitios);
        //}

        //// Estadísticas del sitio (página de admin/dashboard)
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Estadisticas()
        //{
        //    var totalSitios = await _context.SitiosTuristicos.CountAsync();
        //    var totalComentarios = await _context.Comentarios.CountAsync();
        //    var totalTipos = await _context.Tipos.CountAsync();
        //    var totalUsuarios = await _context.Users.CountAsync();

        //    var sitiosPopulares = await _context.SitiosTuristicos
        //        .Include(s => s.Comentarios)
        //        .Include(s => s.Tipo)
        //        .Where(s => s.Comentarios.Any())
        //        .OrderByDescending(s => s.Comentarios.Count)
        //        .Take(5)
        //        .ToListAsync();

        //    ViewBag.TotalSitios = totalSitios;
        //    ViewBag.TotalComentarios = totalComentarios;
        //    ViewBag.TotalTipos = totalTipos;
        //    ViewBag.TotalUsuarios = totalUsuarios;
        //    ViewBag.SitiosPopulares = sitiosPopulares;

        //    return View();
        //}

        // Vista de contacto/información
        public IActionResult Contacto()
        {
            return View();
        }

        // Acerca de la aplicación
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}