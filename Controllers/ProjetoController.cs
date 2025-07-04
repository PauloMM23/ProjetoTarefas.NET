using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers;

public class ProjetoController : Controller
{
    private readonly AppDbContext _context;

    public ProjetoController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Projetos.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var projeto = await _context.Projetos
            .Include(p => p.Tarefas)
            .FirstOrDefaultAsync(p => p.Id == id);

        return projeto == null ? NotFound() : View(projeto);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Projeto projeto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(projeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(projeto);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var projeto = await _context.Projetos.FindAsync(id);
        return projeto == null ? NotFound() : View(projeto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Projeto projeto)
    {
        if (id != projeto.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(projeto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Projetos.Any(p => p.Id == id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        return View(projeto);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var projeto = await _context.Projetos.FindAsync(id);
        return projeto == null ? NotFound() : View(projeto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var projeto = await _context.Projetos.FindAsync(id);
        if (projeto != null)
        {
            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}