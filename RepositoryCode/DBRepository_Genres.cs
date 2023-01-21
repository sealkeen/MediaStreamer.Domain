using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
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
