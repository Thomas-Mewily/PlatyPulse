using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlatyPulseAPI.Data;

namespace PlatyPulseWebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenericController<T> : PlatyController where T : IdentifiableData
{
    private DataBaseCtx _dbContext => Db;
    private readonly DbSet<T> _dbSet;

    public GenericController(DataBaseCtx db, IConfiguration config) : base (db, config)
    {
        _dbSet = _dbContext.Set<T>();
    }

    /// <summary>
    /// Récupère tous les éléments de type T
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var items = await _dbSet.ToListAsync();
        return Ok(items);
    }

    /// <summary>
    /// Récupère un élément spécifique par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<T>> GetById(Guid id)
    {
        var item = await _dbSet.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    /// <summary>
    /// Ajoute un nouvel élément
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<T>> Create([FromBody] T item)
    {
        if (item == null)
        {
            return BadRequest("Item cannot be null");
        }

        _dbSet.Add(item);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = GetItemId(item) }, item);
    }

    /// <summary>
    /// Supprime un élément par son ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _dbSet.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        _dbSet.Remove(item);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Met à jour un élément
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] T item)
    {
        if (GetItemId(item) != id)
        {
            return BadRequest("ID mismatch");
        }

        _dbSet.Update(item);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Obtient l'ID d'un élément
    /// </summary>
    private Guid GetItemId(T item)
    {
        var property = typeof(T).GetProperty("ID");
        if (property == null || property.PropertyType != typeof(Guid))
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not have a valid ID property");
        }
        return (Guid)property.GetValue(item);
    }
}
