using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public Guid GetNewCompositionID()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Bad function, don't use it
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="compositionName"></param>
        /// <param name="albumName"></param>
        /// <param name="duration"></param>
        /// <param name="filePath"></param>
        public void AddComposition(
            string artistName,
            string compositionName,
            string albumName,
            long? duration = null,
            string filePath = null
        )
        {
            var art = new Artist();
            var artistMatches = GetPossibleArtists(artistName);

            if (artistMatches.Count() == 0)
            {
                try
                {
                    art.ArtistName = artistName;
                    DB.AddEntity(art);
                    DB.SaveChanges();
                    artistMatches = GetPossibleArtists(artistName);
                }
                catch
                {
                    return;
                }
            }
            var targetArtist = artistMatches.FirstOrDefault();
            var alb = new Album() { ArtistID = targetArtist.ArtistID, AlbumName = albumName, };
            var cmp = new Composition()
            {
                ArtistID = targetArtist.ArtistID,
                CompositionName = compositionName,
                Duration = duration,
                FilePath = filePath,
            };

            DB.AddEntity(cmp);
        }
    }
}
