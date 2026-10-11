using CampusEats.Api.Data;
using CampusEats.Api.Dtos;
using CampusEats.Api.Models;

namespace CampusEats.Api.Services;

public class MenuService : IMenuService
{
    private readonly AppDbContext _db; // Replaced the in-memory list with DbContext
    
    public MenuService(AppDbContext db) => _db = db;

    // Helper method to map a MenuItem model to a MenuItemDto
    private static MenuItemDto ToDto(MenuItem m) =>
        new(m.Id, m.Name, m.Price, m.Category, m.Available);

    // Retrieves all menu items from the database, ordered by Id
    public IEnumerable<MenuItemDto> GetAll() =>
        _db.MenuItems.OrderBy(m => m.Id).ToList().Select(ToDto);

    // Retrieves a single menu item by its Id
    public MenuItemDto? GetById(int id)
    {
        var m = _db.MenuItems.Find(id);
        return m is null ? null : ToDto(m);
    }

    // Creates a new menu item and saves it to the database
    public MenuItemDto Create(CreateMenuItemDto dto)
    {
        var item = new MenuItem
        {
            Name = dto.Name, // Note: Id is not set here because the database auto-generates it
            Price = dto.Price,
            Category = dto.Category,
            Available = true
        };
        
        _db.MenuItems.Add(item);
        _db.SaveChanges(); // INSERT operation is executed here
        
        return ToDto(item);
    }

    // Updates an existing menu item in the database
    public bool Update(int id, CreateMenuItemDto dto)
    {
        var m = _db.MenuItems.Find(id);
        if (m is null) return false; // Guard clause: return false if item is not found
        
        m.Name = dto.Name; // Replaces the editable fields
        m.Price = dto.Price;
        m.Category = dto.Category;
        
        _db.SaveChanges(); // UPDATE operation is executed here
        
        return true;
    }

    // Deletes a menu item from the database
    public bool Delete(int id)
    {
        var m = _db.MenuItems.Find(id);
        if (m is null) return false; // Guard clause: return false if item is not found
        
        _db.MenuItems.Remove(m);
        _db.SaveChanges(); // DELETE operation is executed here
        
        return true;
    }
}