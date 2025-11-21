using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Guia_Turistica_Trinidad.Data;
using guia_turistico.Models;

namespace Guia_Turistica_Trinidad.Controllers
{
    public class SitiosTuristicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SitiosTuristicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SitiosTuristicos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SitiosTuristicos.Include(s => s.Tipo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SitiosTuristicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitiosTuristicos = await _context.SitiosTuristicos
                .Include(s => s.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitiosTuristicos == null)
            {
                return NotFound();
            }

            return View(sitiosTuristicos);
        }

        // GET: SitiosTuristicos/Create
        public IActionResult Create()
        {
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "Nombre");
            return View();
        }

        // POST: SitiosTuristicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,NombreIngles,NombrePortugues,Descripcion,DescripcionIngles,DescripcionPortugues,Direccion,Latitud,Longitud,TipoId")] SitiosTuristicos sitiosTuristicos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sitiosTuristicos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "Nombre", sitiosTuristicos.TipoId);
            return View(sitiosTuristicos);
        }

        // GET: SitiosTuristicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitiosTuristicos = await _context.SitiosTuristicos.FindAsync(id);
            if (sitiosTuristicos == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "Nombre", sitiosTuristicos.TipoId);
            return View(sitiosTuristicos);
        }

        // POST: SitiosTuristicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,NombreIngles,NombrePortugues,Descripcion,DescripcionIngles,DescripcionPortugues,Direccion,Latitud,Longitud,TipoId")] SitiosTuristicos sitiosTuristicos)
        {
            if (id != sitiosTuristicos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sitiosTuristicos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SitiosTuristicosExists(sitiosTuristicos.Id))
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
            ViewData["TipoId"] = new SelectList(_context.Tipos, "TipoId", "Nombre", sitiosTuristicos.TipoId);
            return View(sitiosTuristicos);
        }

        // GET: SitiosTuristicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sitiosTuristicos = await _context.SitiosTuristicos
                .Include(s => s.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sitiosTuristicos == null)
            {
                return NotFound();
            }

            return View(sitiosTuristicos);
        }

        // POST: SitiosTuristicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sitiosTuristicos = await _context.SitiosTuristicos.FindAsync(id);
            if (sitiosTuristicos != null)
            {
                _context.SitiosTuristicos.Remove(sitiosTuristicos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SitiosTuristicosExists(int id)
        {
            return _context.SitiosTuristicos.Any(e => e.Id == id);
        }
    }
}
