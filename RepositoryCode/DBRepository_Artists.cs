using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        /// <param name="artistName"></param>
        /// <returns>Returns null if does not exist.</returns>
        public Artist GetFirstArtistIfExists(string artistName)
        {
            var artistMatches = GetPossibleArtists(artistName);

            if (artistMatches.Count() == 0)
                return null;
            return artistMatches.FirstOrDefault();
        }
        
        /// <param name="artistName"></param>
        /// <returns>Returns false if does not exist.</returns> /// <summary></summary>
        public bool ArtistExists(string artistName)
        {
            var artistMatches = GetPossibleArtists(artistName);

            if (artistMatches.Count() == 0)
                return false;
            return true;
        }
        public Guid GetNewArtistID()
        {
            return Guid.NewGuid();
        }
    }
}
