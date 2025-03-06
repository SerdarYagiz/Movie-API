using MA.Data.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace MA.Service;

public interface IGenericService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    //Task<bool> PatchMovieAsync(Guid id, JsonPatchDocument<Movie> patchDoc);
    Task<bool> UpdateMoviePartialAsync(Guid id, Movie updatedMovie);

    Task<object?> FilterMoviesAsync(string? title, double? duration,DateOnly? releaseDate,int? before,int? after,string? sortBy);
}