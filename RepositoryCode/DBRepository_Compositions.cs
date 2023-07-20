using StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        /// <summary> This method merges </summary>
        /// <param name="existingComp">The composition from the database</param>
        /// <param name="comp">Newly created composition</param>
        public void CopyFieldsExceptForDurationAndPath(Composition existingComp, Composition comp)
        {
            comp.About = existingComp.About; comp.Album = existingComp.Album;
            comp.AlbumID = existingComp.AlbumID; comp.Artist = existingComp.Artist;
            comp.ArtistID = existingComp.ArtistID; comp.CompositionName = existingComp.CompositionName;
        }

        /// <summary> This method changes the existing composition if it exists </summary>
        public Composition AddComposition(Artist artist, Album album,
            string title, TimeSpan duration,
            string fileName, long? yearFromFile = null,
            bool onlyReturnNoAppend = false,
            Action<string> errorAction = null)
        {
            try
            {
                var newComposition = new Composition();
                var existing = from comp in DB.GetCompositions()
                               join alb in DB.GetAlbums() on comp.AlbumID equals alb.AlbumID
                               join art in DB.GetArtists() on comp.ArtistID equals art.ArtistID
                               where (comp.CompositionName == title 
                               && alb.AlbumName == album.AlbumName 
                               && art.ArtistName == artist.ArtistName) || PathResolver.Equals(comp.FilePath, fileName)
                               select comp;

                if (existing.Any())
                {
                    var existingComp = existing.FirstOrDefault();
                    ChangeExistingComposition(artist, album, title, duration, fileName,
                        onlyReturnNoAppend, newComposition, existingComp, errorAction);
                    return existingComp;
                }

                if (title == null)
                {
                    if (errorAction != null) errorAction.Invoke("title is null");
                    return null;
                }
                try {
                    newComposition.CompositionID = GetNewCompositionID();
                    newComposition.CompositionName = title;
                    newComposition.ArtistID = artist?.ArtistID;
                    newComposition.AlbumID = album?.AlbumID;
                    newComposition.Album = album;
                    newComposition.Artist = artist;

                    try {
                        newComposition.Duration = (long?)duration.TotalSeconds;
                        newComposition.FilePath = fileName;
                    } catch {
                        //leave them null
                    }

                    DB.AddEntity(newComposition);
                    DB.SaveChanges();
                    return newComposition;
                } catch (Exception ex) {
                    if(errorAction != null) errorAction.Invoke(ex.Message);
                    return null;
                }
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public bool ChangeFilename(Composition composition, string fileName, Action<string> errorAction = null)
        {
            try
            {
                composition.FilePath = fileName;
                DB.SaveChanges();
                return true;
            }
            catch (Exception ex) {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// This is not working because we can't change the existing data inside of a DBTable.
        /// I don't know how to fix it.
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="title"></param>
        /// <param name="duration"></param>
        /// <param name="fileName"></param>
        /// <param name="onlyReturnNoAppend"></param>
        /// <param name="newComposition"></param>
        /// <param name="existingComp"></param>
        /// <returns></returns>
        public void ChangeExistingComposition(Artist artist,
            Album album, string title, TimeSpan duration, string fileName,
            bool onlyReturnNoAppend, Composition newComposition,
            Composition existingComp, Action<string> errorAction)
        {
            try
            {
                //todo: return and complete this
                //todo: figure out why we can't change en entity

                //return existingComp;

                CopyFieldsExceptForDurationAndPath(existingComp, newComposition);

                existingComp.CompositionName = title;
                existingComp.ArtistID = artist.ArtistID;
                existingComp.AlbumID = album.AlbumID;
                existingComp.Album = album;
                existingComp.Artist = artist;

                existingComp.Duration = (long?)duration.TotalSeconds;
                existingComp.FilePath = fileName;

                //todo:

                //existingComp.Duration = (long?)duration.TotalSeconds;
                //existingComp.FilePath = fileName;
                //existingComp.CompositionID = GetNewCompositionID();
                //return existingComp;
                if (!onlyReturnNoAppend)
                    DB.SaveChanges();
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
        }

        public bool DeleteComposition(Composition composition,
            Action<string> errorAction = null)
        {
            try
            {
                if (DB != null) DB.RemoveEntity(DB.GetCompositions().FirstOrDefault(x => x.CompositionID == composition.CompositionID));
                if (DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }

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

        public bool DeleteComposition(Guid ID,
            Action<string> errorAction = null)
        {
            try
            {
                DB.RemoveEntity(DB.GetCompositions().FirstOrDefault(x => x.CompositionID == ID));
                if (DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }
    }
}
