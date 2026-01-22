using System.Collections.Generic;
using System.Threading.Tasks;
using TeluqMovieForm.Models;

namespace TeluqMovieForm.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieResult>> GetMoviesAsync();
}