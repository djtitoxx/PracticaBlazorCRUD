using BlazorCrud.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCRUD.Data.Dapper.Repositorios
{
  
    public class FilmRepository : IFIlmRepository
    {

        private string _ConnectionString;

        public FilmRepository(string connectionString)
        {
            _ConnectionString  = connectionString;
        }

        protected SqlConnection dbConnection()
        {
            return new SqlConnection(this._ConnectionString);
        }

            public async Task<bool> DeleteFilm(int id)
        {
            var db = dbConnection();
            var sql = @"DELETE FROM Films Where Id = @Id";
            var result = await db.ExecuteAsync(sql.ToString(), new { Id = id }) ;
            return result > 0;
        }

        public async Task<IEnumerable<Film>> GetAllFilms()
        {
            var db = dbConnection();
            var sql = @"SELECT Id, Title, Director, ReleaseDate FROM [dbo].[films]";
            return await db.QueryAsync<Film>(sql.ToString(), new {});
        }

        public async Task<Film> GetFilmDetails(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT Id, Title, Director, ReleaseDate FROM [dbo].[films]
                      WHERE Id = @Id";
            return await db.QueryFirstOrDefaultAsync<Film>(sql.ToString(), new { id = id });
        }

        public async Task<bool> InsertFilm(Film film)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO Films (Title, Director, ReleaseDate) Values (@Title, @Director, @ReleaseDate)";
            var result = await db.ExecuteAsync(sql.ToString(), new
            {
                film.Title,
                film.Director,
                film.ReleaseDate
            });


            return result > 0;
        }

        public async Task<bool> UpdateFilm(Film film)
        {
            var db = dbConnection();
            var sql = @"UPDATE Films 
                        SET Title = @Title, Director = @Director, ReleaseDate = @ReleaseDate 
                        WHERE Id = @Id ";
            var result = await db.ExecuteAsync(sql.ToString(),
                new { film.Title, film.Director, film.ReleaseDate, film.Id });

            return result > 0;
        }
    }
}
