using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public AlbumGenre GetFirstAlbumGenreIfExists(Artist artist, Album album)
        {
            var matches = from AlbumGenre ag in DB.GetAlbumGenres()
                          join alb in DB.GetAlbums() on ag.AlbumID equals alb.AlbumID
                          where alb.AlbumID == album.AlbumID
                          select ag;
            if (matches.Any())
                return matches.FirstOrDefault();
            return null;
        }

        public AlbumGenre AddAlbumGenre(Artist artist, Album album, Genre genre,
            Action<string> errorAction = null)
        {
            try
            {
                var ag = GetFirstAlbumGenreIfExists(artist, album);
                var albG = new AlbumGenre() {
                    AlbumID = album.AlbumID, GenreID = genre.GenreID
                };

                if (album.AlbumGenres == null)
                    album.AlbumGenres = new HashSet<AlbumGenre>();
                DB.AddEntity(albG);
                DB.UpdateAndSaveChanges(album);
                return albG;
            } catch (Exception ex) {
                errorAction?.Invoke("AddAlbumGenre: " + ex.ToString() + ex.Message);
                return null;
            }
        }
    }
}
