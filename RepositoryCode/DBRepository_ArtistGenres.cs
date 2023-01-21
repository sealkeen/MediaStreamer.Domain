using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public ArtistGenre GetFirstArtistGenreIfExists(Guid artistID, Guid genreID)
        {
            var ags = DB.GetArtistGenres().Where(ag => ag.ArtistID == artistID && ag.GenreID == genreID);

            if (ags.Count() == 0)
                return null;
            return ags.FirstOrDefault();
        }
    }
}
