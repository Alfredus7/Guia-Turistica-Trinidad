using Guia_Turistica_Trinidad.Data;
using guia_turistico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Guia_Turistica_Trinidad.Controllers
{
    public class ImagenesSitiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagenesSitiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImagenesSitios
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ImagenesSitios.Include(i => i.SitioTuristico);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ImagenesSitios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesSitios = await _context.ImagenesSitios
                .Include(i => i.SitioTuristico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagenesSitios == null)
            {
                return NotFound();
            }

            return View(imagenesSitios);
        }

        // GET: ImagenesSitios/Create
        public IActionResult Create()
        {
            ViewData["SitioTuristicoId"] = new SelectList(_context.SitiosTuristicos, "Id", "Nombre");
            return View();
        }

        // POST: ImagenesSitios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int sitioTuristicoId, string allImageUrls)
        {
            if (string.IsNullOrEmpty(allImageUrls) || sitioTuristicoId == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar un sitio y agregar al menos una imagen.");
                ViewBag.SitioTuristicoId = new SelectList(_context.SitiosTuristicos, "Id", "Nombre");
                return View();
            }

            try
            {
                var imageUrls = JsonConvert.DeserializeObject<List<string>>(allImageUrls);

                foreach (var url in imageUrls)
                {
                    var imagenSitio = new ImagenesSitios
                    {
                        Url = url,
                        SitioTuristicoId = sitioTuristicoId
                    };

                    _context.Add(imagenSitio);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar las imágenes: " + ex.Message);
                ViewBag.SitioTuristicoId = new SelectList(_context.SitiosTuristicos, "Id", "Nombre");
                return View();
            }
        }

        // GET: ImagenesSitios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesSitios = await _context.ImagenesSitios.FindAsync(id);
            if (imagenesSitios == null)
            {
                return NotFound();
            }
            ViewData["SitioTuristicoId"] = new SelectList(_context.SitiosTuristicos, "Id", "Nombre", imagenesSitios.SitioTuristicoId);
            return View(imagenesSitios);
        }

        // POST: ImagenesSitios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url,SitioTuristicoId")] ImagenesSitios imagenesSitios)
        {
            if (id != imagenesSitios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagenesSitios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagenesSitiosExists(imagenesSitios.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SitioTuristicoId"] = new SelectList(_context.SitiosTuristicos, "Id", "Nombre", imagenesSitios.SitioTuristicoId);
            return View(imagenesSitios);
        }

        // GET: ImagenesSitios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagenesSitios = await _context.ImagenesSitios
                .Include(i => i.SitioTuristico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagenesSitios == null)
            {
                return NotFound();
            }

            return View(imagenesSitios);
        }

        // POST: ImagenesSitios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagenesSitios = await _context.ImagenesSitios.FindAsync(id);
            if (imagenesSitios != null)
            {
                _context.ImagenesSitios.Remove(imagenesSitios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagenesSitiosExists(int id)
        {
            return _context.ImagenesSitios.Any(e => e.Id == id);
        }
    }
}
