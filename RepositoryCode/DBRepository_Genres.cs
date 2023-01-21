using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public IQueryable<Genre> GetPossibleGenres(string genreName)
        {
            var matches = from match in DB.GetGenres()
                          where (match.GenreName == genreName)
                          select match;

            return matches;
        }
        public IQueryable<Genre> GetPossibleGenresByArtistName(string artistName)
        {
            var matches = from match in DB.GetArtistGenres()
                          join art in DB.GetArtists() on match.ArtistID equals art.ArtistID
                          join gen in DB.GetGenres() on match.GenreID equals gen.GenreID
                          where (art.ArtistName == artistName)
                          select gen;

            return matches;
        }
        public void AddNewGenreOrSetToUnknown(out Genre genre, string newGenre)
        {
            if ((genre = GetFirstGenreIfExists(newGenre)) != null)
                return;
            if (newGenre != null)
            {   // genre tag is valid, creating new entity
                genre = new Genre() { GenreID = Guid.NewGuid(), GenreName = newGenre };
                try
                {
                    DB.AddEntity(genre);
                    DB.SaveChanges();
                    genre = DB.GetGenres().Where(g => g.GenreName == newGenre).FirstOrDefault();
                    return;
                } catch { 
                
                }
            }
            genre = AddNewUnknownGenreIfNotExists();
        }

        public Genre AddGenreToArtist(Artist artist, string newGenre, Action<string> errorAction = null)
        {
            try
            {
                Genre genre;
                AddNewGenreOrSetToUnknown(out genre, newGenre);
                AddNewArtistGenre(artist, genre);
                return genre;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke("AddGenreToArtist(): " + ex.Message);
                return null;
            }
        }

        private Genre AddNewUnknownGenreIfNotExists()
        {
            var genre = GetFirstGenreIfExists("unknown");
            // TODO: Return and fix "find"
            if (genre == null)
            {
                genre = new Genre() { GenreName = "unknown" };
                DB.AddEntity(genre);
                DB.SaveChanges();
            }

            return GetFirstGenreIfExists("unknown");
        }
        public Genre AddNewUnknownGenreToArtist(string artistName)
        {
            var artist = GetFirstArtistIfExists(artistName);
            if (artist == null)
            {
                artist = AddArtist(artistName);
            }
            return AddGenreToArtist(artist, "unknown");
        }

        /// <summary> Lazy pick first genre. </summary>
        /// <param name="genreName"></param> /// <returns></returns>
        public Genre GetFirstGenreIfExists(string genreName)
        {
            var genreMatches = GetPossibleGenres(genreName);

            if (genreMatches.Count() == 0)
                return null;
            return genreMatches.FirstOrDefault();
        }

        /// <summary> Lazy pick last genre. </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public Genre GetLastGenreIfExistsByArtist(string artistName)
        {
            try {
                var genreMatches = GetPossibleGenresByArtistName(artistName);

                if (genreMatches.Count() == 0)
                    return null;

                return genreMatches.OrderBy(g => g.GenreID).Last();
            } catch (NullReferenceException) {
                return null;
            }
        }
        public Genre GetFirstGenreIfExistsByArtist(string artistName)
        {
            var genreMatches = GetPossibleGenresByArtistName(artistName);

            if (genreMatches.Count() == 0)
                return null;
            return genreMatches.FirstOrDefault();
        }
    }
}
