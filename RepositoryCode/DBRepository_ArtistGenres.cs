using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {

        public ArtistGenre AddArtistGenre(string artistName, string genreName,
            long? dateOfDisband = null, Action<string> errorAction = null
        )
        {
            try
            {
                if (genreName == null || genreName == string.Empty)
                {
                    if (errorAction != null) errorAction.Invoke("AddArtistGenre exception : genreName == null");
                    return null;
                }
                var firstArtist = GetFirstArtistIfExists(artistName);

                if (firstArtist == null)
                {
                    if (errorAction != null) errorAction.Invoke("AddArtistGenre exception : no matching artist exist <" + artistName + ">.");
                    return null;
                }

                var newAGenre = new ArtistGenre();
                firstArtist = DB.GetArtists().FirstOrDefault(x => x.ArtistID == firstArtist.ArtistID);
                newAGenre.Artist = firstArtist;
                newAGenre.ArtistID = firstArtist.ArtistID;
                //todo: check for changes //newAGenre.Genre = DB.Genres.Find(genreName);
                newAGenre.Genre = GetFirstGenreIfExists(genreName);

                if (newAGenre.Genre == null)
                    newAGenre.Genre = new Genre { GenreName = genreName };
                else
                    newAGenre.GenreID = newAGenre.GenreID;

                //GetFirstArtistIfExists(firstArtist.ArtistName).ArtistGenres.Add(newAGenre);
                DB.AddEntity(newAGenre);
                DB.SaveChanges();
                return newAGenre;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }
        public bool ArtistHasGenre(Artist artist, string possibleGenre)
        {
            var genres = from aGen in artist.ArtistGenres
                         join gen in DB.GetGenres() on aGen.GenreID equals gen.GenreID
                         where gen.GenreName == possibleGenre
                         select gen;

            if (genres.Count() == 0)
                return false;
            return true;
        }

        public ArtistGenre FindArtistGenreOrReturnNull(Guid artistID, string genreName)
        {
            var query = from ag in DB.GetArtistGenres()
                        join gen in DB.GetGenres() on ag.GenreID equals gen.GenreID
                        where ag.ArtistID == artistID &&
                        gen.GenreName == genreName
                        select ag;
            if (query.Any())
                return query.FirstOrDefault();
            else
                return null;
        }

        public void AddNewArtistGenre(Artist artist, Genre genre, Action<string> errorAction = null)
        {
            var foundGenreByArtist = GetFirstArtistGenreIfExists(artist.ArtistID, genre.GenreID);
            if (foundGenreByArtist == null) // if Genre not contains Artist 
            {
                //genre = new Genre();
                //genre.GenreName = newGenre;

                var artG = new ArtistGenre()
                {
                    Artist = artist,
                    ArtistID = artist.ArtistID,
                    //DateOfApplication = DateTime.Now,
                    Genre = genre,
                    //GenreName = genre.GenreName,
                    GenreID = genre.GenreID
                };

                var existingAG = FindArtistGenreOrReturnNull(artist.ArtistID, genre.GenreName);
                if (existingAG == null)
                {
                    try {
                        DB.AddEntity(artG);
                        DB.SaveChanges();
                    }
                    catch (Exception ex) {
                        if (errorAction != null) errorAction.Invoke("AddNewArtistGenre(): " + ex.Message);
                    }
                }
            }
        }
        public ArtistGenre GetFirstArtistGenreIfExists(Guid artistID, Guid genreID)
        {
            var ags = DB.GetArtistGenres().Where(ag => ag.ArtistID == artistID && ag.GenreID == genreID);

            if (ags.Count() == 0)
                return null;
            return ags.FirstOrDefault();
        }
    }
}
