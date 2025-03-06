using MA.Data;
using MA.Data.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace MA.Service.MovieService;

public class GenericService<T> : IGenericService<T> where T : class, IBaseEntity
{   
    private readonly MADBContext _context;
    private readonly DbSet<T> _dbSet;
    public GenericService(MADBContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        
        
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var all = await _dbSet.ToListAsync();
        return all;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = _dbSet.Find(id);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return;
    }

    public async Task<bool> UpdateMoviePartialAsync(Guid id, Movie updatedMovie)
    {
        var existingMovie = await _context.movies.FindAsync(id);
        if (existingMovie == null)
        {
            return false; // Film bulunamadı
        }
        if (!string.IsNullOrEmpty(updatedMovie.Title))
            existingMovie.Title = updatedMovie.Title;
        
        if (updatedMovie.MovieLength > 0)
            existingMovie.MovieLength = updatedMovie.MovieLength;
        
        if (updatedMovie.ReleaseDate != DateOnly.MinValue)
            existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
        
        if (!string.IsNullOrEmpty(updatedMovie.Category))
            existingMovie.Category = updatedMovie.Category;
        
       
        _context.Entry(existingMovie).Property(m => m.Rating).IsModified = false;

        await _context.SaveChangesAsync();
        return true;
        
    }

    /*public async Task<bool> PatchMovieAsync(Guid id, JsonPatchDocument<Movie> patchDoc)
    {
        var movie = await _context.movies.FindAsync(id);
        if (movie == null)
        {
            return false;
        }

        // 📌 Patch işlemi uygula
        patchDoc.ApplyTo(movie);

        // 🚫 **AverageRating değiştirilemez!**
        _context.Entry(movie).Property(m => m.Rating).IsModified = false;

        await _context.SaveChangesAsync();
        return true;
    }*/

    
    
    
    public async Task<object?> FilterMoviesAsync(string? title, double? duration,DateOnly? releaseDate,int? before,int? after,string? sortBy)
    {
        IQueryable<Movie> query = _context.movies;

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(m => m.Title.Contains(title));
        }

        if (duration.HasValue)
        {
            query = query.Where(m => Math.Abs(m.MovieLength - duration.Value) < 0.1);
        }

        
        // 🗓 **Vizyona giriş tarihi için tam tarih filtresi**
        if (releaseDate.HasValue)
        {
            query = query.Where(m => m.ReleaseDate == releaseDate.Value);
        }

        // ⬆️ **Belirli bir yıldan sonra çıkan filmleri getir**
        if (after.HasValue)
        {
            query = query.Where(m => m.ReleaseDate.Year > after.Value);
        }

        // ⬇️ **Belirli bir yıldan önce çıkan filmleri getir**
        if (before.HasValue)
        {
            query = query.Where(m => m.ReleaseDate.Year < before.Value);
        }

        // 🔄 **Sorting (sıralama)**
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.ToLower() == "title")
            {
                query = query.OrderBy(m => m.Title);
            }
        }

        // 📄 **Pagination (sayfalama)**
        //query = query.Skip((int)((page - 1) * size)).Take(size);

        return await query.ToListAsync();
    }
    
}