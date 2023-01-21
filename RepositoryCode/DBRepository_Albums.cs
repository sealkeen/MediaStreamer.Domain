using System;
using System.Diagnostics;
using System.Linq;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        /// <summary> Returns null if does not exist. </summary>
        /// <param name="artistName">Possible artist name.</param>
        /// <param name="albumName">Possible album name.</param>
        public Album GetFirstAlbumIfExists(string artistName, string albumName)
        {
            var albumMatches = GetPossibleAlbums(artistName, albumName);

            if (albumMatches.Count() == 0)
                return null;
            return albumMatches.FirstOrDefault();
        }

        public Album AddAlbum(
            string artistName,
            string albumName,
            long? year = null,
            string label = null,
            string type = null,
            Action<string> errorAction = null
        )
        {
            Album newAlbum = null;
            try {
                var foundArtist = GetFirstArtistIfExists(artistName);
                if (foundArtist == null) {
                    foundArtist = AddArtist(artistName);
                    DB.SaveChanges();
                }

                var foundAlbum = GetFirstAlbumIfExists(artistName, albumName);
                var genre = GetLastGenreIfExistsByArtist(artistName);
                if (genre == null) AddNewUnknownGenreToArtist(artistName);
                
                // No similar
                if (foundAlbum == null /*&& artAlbs.Count() == 0*/) {
                    // No similar, No Album name retrieved
                    if ( string.IsNullOrEmpty(albumName) || albumName.ToLower().Trim() == "unknown" ) {
                        newAlbum = AddNewUnknownAlbumToExistingArtistAndGenre(foundArtist, genre);
                    } else {
                    // No similar, But Album name retrieved, create new
                        newAlbum = AddNewAlbumToExistingArtistAndGenre(albumName, year, label, type, foundArtist, genre);
                    }
                    // Has similar, return it
                } else if ( foundAlbum.AlbumName.ToLower().Trim() == albumName.ToLower().Trim() ) {
                    return foundAlbum;
                } 

                return newAlbum;
            } catch (Exception ex) {
                if (errorAction != null) errorAction.Invoke("AddAlbum(): " + ex.Message);
                return null;
            }
        }

        public Album AddAlbum(
            Artist artist, Genre genre, string albumFromFile,
            string label = null, 
            string type = null, long? year = null,
            Action<string> errorAction = null)
        {
            try {
                Album albumToAdd;

                if (albumFromFile == null || albumFromFile == string.Empty) 
                { // album tag/name is not recognized
                    return AddNewUnknownAlbumToExistingArtistAndGenre(artist, genre, label, type, year);
                } else { // album tag/name is recognized 
                    var foundAlbum = GetFirstAlbumIfExists(artist.ArtistName, albumFromFile);
                    if ((foundAlbum == null)) { // the album is new
                        albumToAdd = new Album() { 
                            Artist = artist, AlbumName = albumFromFile,
                            Genre = genre,
                            GenreID = genre.GenreID,
                            ArtistID = (artist.ArtistID), AlbumID = GetNewAlbumID()
                        };
                        try {
                            DB.AddEntity(albumToAdd);
                            DB.SaveChanges();
                        } catch (Exception ex) {
                            errorAction?.Invoke(ex.Message);
                            Debug.WriteLine(ex);
                        }
                    } else { // album is found in DB
                        albumToAdd = foundAlbum;
                    }
                }
                return albumToAdd;
            }
            catch (Exception ex)
            {
                errorAction?.Invoke("AddAlbumGenre: " + ex.ToString() + ex.Message);
                return null;
            }
        }
        public Album AddNewUnknownAlbumToExistingArtistAndGenre(Artist artist, Genre genre,
            string label = null, string type = null, long? year = null)
        {
            Album albumToAdd; // var albumFound = DB.GetAlbums().Find("unknown");

            if (artist.ArtistName == null || artist.ArtistName == string.Empty) {
                throw new /*Domain*/Exception("in AddAlbum(): Artist's name was either null or empty.");
            }

            if (genre == null)
                genre = AddNewUnknownGenreToArtist(artist.ArtistName); //TODO: gFD = null handle

            var unknownAlbums = from unknownAlbum in DB.GetAlbums()
                                join art in DB.GetArtists()
                                on unknownAlbum.ArtistID equals art.ArtistID
                                where unknownAlbum.AlbumName == "unknown" &&
                                art.ArtistID == artist.ArtistID
                                select unknownAlbum;

            if (unknownAlbums.Count() == 0) {
                albumToAdd = new Album() {
                    AlbumName = "unknown", AlbumID = GetNewAlbumID(), Artist = artist,
                    ArtistID = artist.ArtistID, Genre = genre, GenreID = genre.GenreID,
                    Label = label, Type = type, Year = year
                };
                DB.AddEntity(albumToAdd);
                DB.SaveChanges();
            } else {
                albumToAdd = unknownAlbums.FirstOrDefault();
            }
            return albumToAdd;
        }

        private Album AddNewAlbumToExistingArtistAndGenre(string albumName, long? year, string label, string type, Artist foundArtist, Genre genre)
        {
            Album newAlbum;
            newAlbum = new Album()
            {
                AlbumID = GetNewAlbumID(),
                ArtistID = foundArtist?.ArtistID,
                AlbumName = albumName,
                GenreID = genre.GenreID, //ArtistName = targetArtist.ArtistName,
                Label = label,
                Type = type,
                Year = year
            };
            DB.AddEntity(newAlbum);
            DB.SaveChanges();
            return newAlbum;
        }

        public Guid GetNewAlbumID()
        {
            return Guid.NewGuid();
        }
    }
}
