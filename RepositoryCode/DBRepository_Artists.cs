using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public IQueryable<Artist> GetPossibleArtists(string name)
        {
            var matches = from match in DB.GetArtists() where match.ArtistName == name select match;

            return matches;
        }

        public Artist AddArtist(string artistFileName, Action<string> errorAction = null)
        {
            try
            {
                Artist artistToAdd;
                if (artistFileName == null) {
                    artistToAdd = GetFirstArtistIfExists("unknown");
                    if (artistToAdd == null) {
                        DB.AddEntity(artistToAdd = new Artist() { ArtistName = "unknown", ArtistID = GetNewArtistID() });
                    }
                    return artistToAdd;
                }
                artistToAdd = GetFirstArtistIfExists(artistFileName);
                if (artistToAdd == null) {
                    artistToAdd = new Artist() { ArtistName = artistFileName };
                    try {
                        artistToAdd.ArtistID = GetNewArtistID();
                    } catch {
                        if(errorAction != null) errorAction.Invoke("Aquiring new artist ID Exception raised.");
                        //return null;
                    }

                    DB.AddEntity(artistToAdd);
                    DB.SaveChanges();
                }

                return artistToAdd;
            }
            catch (Exception ex)
            { //TODO: remove the countOfArtists (or not)
                //var countOfArtists = DB.GetArtists().Count();
                if(errorAction != null) errorAction.Invoke("AddArtist(): " + ex.Message);
                return null;
            }
        }

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
