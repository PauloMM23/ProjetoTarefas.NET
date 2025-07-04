using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoTarefas.Data;
using ProjetoTarefas.Models;

namespace ProjetoTarefas.Controllers;

public class TarefaController : Controller
{
    private readonly AppDbContext _context;

    public TarefaController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int projetoId)
    {
        var tarefa = new Tarefa { ProjetoId = projetoId };
        return View(tarefa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tarefa tarefa)
    {
        if (ModelState.IsValid)
        {
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projeto", new { id = tarefa.ProjetoId });
        }
        return View(tarefa);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var tarefa = await _context.Tarefas.FindAsync(id);
        return tarefa == null ? NotFound() : View(tarefa);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projeto", new { id = tarefa.ProjetoId });
        }
        return View(tarefa);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id);
        return tarefa == null ? NotFound() : View(tarefa);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa != null)
        {
            int projetoId = tarefa.ProjetoId;
            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projeto", new { id = projetoId });
        }
        return RedirectToAction("Index", "Projeto");
    }
}