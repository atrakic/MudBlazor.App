using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using app.Core.Model;
using app.Infrastructure;
using app.Core.Interfaces;

namespace app.Services;

public class ProductService : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task<Product> GetProductAsync(int id)
    {
        return await _context.Products.FindAsync(id) ?? throw new Exception("Product not found");
    }
    public async Task<Product> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return product;
    }
    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException("Product not found");
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
