using MA.Data.Entities;
using MA.Service;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using MovieApp.Models;

namespace MovieApp.Controller;

[Route("api/movies")]
[ApiController]
public class MovieController : ControllerBase

{
    private readonly IGenericService<Movie> _movieService;

    public MovieController(IGenericService<Movie> movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("api/movies")]
    public async Task<IEnumerable<Movie>> Get()
    {
        return await _movieService.GetAllAsync();
    }

    [HttpGet("search")]
    public async Task<Movie?> Get(Guid id)
    {
        return await _movieService.GetByIdAsync(id);
    }

    [HttpPost("Add")]
    public async Task Post(Movie movie)
    {
        await _movieService.AddAsync(movie);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Put(Guid id, Movie movie)
    {
        var existingMovie = await _movieService.GetByIdAsync(id);
        if (existingMovie == null)
        {
            return NotFound("Film bulunamadı.");
        }

        // 🚫 **AverageRating değiştirilemez!**
        movie.Rating = existingMovie.Rating;
        
        await _movieService.UpdateAsync(movie);
        
        return NoContent();;
    }

    [HttpDelete("Delete")]
    public async Task Delete(Guid id)
    {
        await _movieService.DeleteAsync(id);
    }
    

    [HttpGet("filter")]
    public async Task<IActionResult> FilterMovies([FromQuery] string? title, [FromQuery] int? duration ,DateOnly? releaseDate,int? before,int? after,string? sortBy)
    {
        var filteredMovies = await _movieService.FilterMoviesAsync(title, duration,releaseDate, before, after, sortBy);
        return Ok(filteredMovies);
    }
    
    
    [HttpPut("UpdatePartial/{id}")]
    public async Task<IActionResult> UpdateMoviePartial(Guid id, [FromBody] Movie updatedMovie)
    {
        var success = await _movieService.UpdateMoviePartialAsync(id, updatedMovie);
        if (!success)
        {
            return NotFound("Film bulunamadı.");
        }

        return NoContent();
    }
    
    /*[HttpPatch("Patch")]
    public async Task<IActionResult> PatchMovie(Guid id, [FromBody] JsonPatchDocument<Movie> patchDoc)
    {
        if (patchDoc == null)
        {
            return BadRequest();
        }

        var success = await _movieService.PatchMovieAsync(id, patchDoc);
        if (!success)
        {
            return NotFound("Film bulunamadı.");
        }

        return NoContent();
    }
    */    
}