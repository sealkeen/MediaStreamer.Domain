using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using StringExtensions;
using System.Threading.Tasks;
using LinqExtensions;
using MediaStreamer.Domain;

namespace MediaStreamer.Domain
{
    public class DBRepository : IDBRepository
    {
        public IDMDBContext DB { get; set; }

        //private Task<IDBRepository> _loadingTask;
        //public Task<IDBRepository> LoadingTask { get { return _loadingTask; } set { _loadingTask = value; } }

        public void OnStartup()
        {
            
        }

        public void EnsureCreated()
        {
            DB.EnsureCreated();
        }

        public Guid GetNewCompositionID()
        {
            return Guid.NewGuid();
        }

        public Guid GetNewArtistID()
        {
            return Guid.NewGuid();
        }

        public Guid GetNewAlbumID()
        {
            return Guid.NewGuid();
        }

        public Guid GetNewModeratorID()
        {
            return Guid.NewGuid();
        }

        public Guid GetNewAdministratorID()
        {
            return Guid.NewGuid();
        }

        public void PopulateDataBase( Action<string> errorAction = null )
        {
            try
            {
                //AddArtist("August Burns Red", new DateTime(2003, 1, 1), null);//, DB);
                //AddArtist("Being As An Ocean", new DateTime(2011, 1, 1), null);//, DB);
                //AddArtist("Delain", new DateTime(2002, 1, 1), null);//, DB);
                //AddArtist("Fifth Dawn", new DateTime(2014, 1, 1), null);//, DB);
                //AddArtist("Saviour", new DateTime(2009, 1, 1), null);//, DB);

                //AddArtistGenre("August Burns Red", "metalcore");//, DB);
                //AddArtistGenre("Being As An Ocean", "melodic hardcore");//, DB);
                //AddArtistGenre("Being As An Ocean", "post-hardcore");//, DB);
                //AddArtistGenre("Delain", "symphonic metal");//, DB);
                //AddArtistGenre("Fifth Dawn", "alternative rock");//, DB);
                //AddArtistGenre("Saviour", "melodic hardcore");//, DB);

                //AddAlbum("August Burns Red", "Found In Far Away Places", 2015, "Fearless", "Studio");
                //AddAlbum("Being As An Ocean", "Waiting For Morning To Come (Deluxe Edition)", 2018, "SharpTone", "Studio");
                //AddAlbum("Being As An Ocean", "Waiting For Morning To Come", 2017, "SharpTone", "Studio");
                //AddAlbum("Delain", "April Rain", 2009, "Sensory", "Studio");
                //AddAlbum("Fifth Dawn", "Identity", 2018, "Dreambound", "Studio");
                //AddAlbum("Saviour", "Empty Skies", 2018, "Dreambound", "Studio");

                //AddComposition("August Burns Red", "Identity", "Found In Far Away Places", 259);
                //AddComposition("August Burns Red", "Marathon", "Found In Far Away Places", 286);
                //AddComposition("August Burns Red", "Martyr", "Found In Far Away Places", 275);
                //AddComposition("Being As An Ocean", "Alone", "Waiting For Morning To Come (Deluxe Edition)", 264);
                //AddComposition("Being As An Ocean", "Black & Blue", "Waiting For Morning To Come", 256);
                //AddComposition("Being As An Ocean", "Blacktop", "Waiting For Morning To Come", 296);
                //AddComposition("Being As An Ocean", "Dissolve", "Waiting For Morning To Come", 288);
                //AddComposition("Being As An Ocean", "Glow", "Waiting For Morning To Come", 314);
                //AddComposition("Being As An Ocean", "OK", "Waiting For Morning To Come", 256);
                //AddComposition("Being As An Ocean", "Thorns", "Waiting For Morning To Come", 230);
                //AddComposition("Being As An Ocean", "Waiting for Morning to Come", "Waiting For Morning To Come", 295);
                //AddComposition("Delain", "April Rain (Album Version)", "April Rain", 276);
                //AddComposition("Fifth Dawn", "Allure", "Identity", 288);
                //AddComposition("Fifth Dawn", "Element", "Identity", 288);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
        }

        public Genre AddNewUnknownGenreToArtist(string artistName)
        {
            //var genre = GetFirstGenreIfExistsByArtist(artistName);

            var artist = GetFirstArtistIfExists(artistName);
            if (artist == null)
            {
                artist = AddArtist(artistName);
            }
            return AddGenreToArtist(artist, "unknown");
        }

        public Album AddNewUnknownAlbumToExistingArtistAndGenre(Artist artist, Genre genre, 
            string label = null, string type = null, long? year = null)
        {
            Album albumToAdd;
            // var albumFound = DB.GetAlbums().Find("unknown");

            if (artist.ArtistName == null || artist.ArtistName == string.Empty)
            {
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

            if (unknownAlbums.Count() == 0)
            {
                albumToAdd = new Album()
                {
                    AlbumName = "unknown",
                    AlbumID = GetNewAlbumID(),
                    Artist = artist,
                    ArtistID = artist.ArtistID,
                    Genre = genre,
                    GenreID = genre.GenreID,
                    Label = label,
                    Type = type,
                    Year = year
                };
                DB.AddEntity(albumToAdd);
                DB.SaveChanges();
            }
            else
            {
                albumToAdd = unknownAlbums.FirstOrDefault();
            }
            return albumToAdd;
        }

        public Album AddAlbum(
            Artist artist, Genre genre, string albumFromFile,
            string label = null, 
            string type = null, long? year = null)
        {
            Album albumToAdd;

            if (albumFromFile == null || albumFromFile == string.Empty) 
            {
                // album tag/name is not recognized
                return AddNewUnknownAlbumToExistingArtistAndGenre(artist, genre, label, type, year);
            } else {
                // album tag/name is recognized 
                var foundAlbum = GetFirstAlbumIfExists(artist.ArtistName, albumFromFile);
                if ((foundAlbum == null))
                {
                    // the album is new
                    albumToAdd = new Album()
                    { //ArtistName = artistFileName,
                        Artist = artist, AlbumName = albumFromFile,
                        Genre = genre,
                        GenreID = genre.GenreID,
                        ArtistID = (artist.ArtistID), AlbumID = GetNewAlbumID()
                    };
                    try {
                        DB.AddEntity(albumToAdd);
                        DB.SaveChanges();
                    } catch (Exception ex) {
                        Debug.WriteLine(ex.Message);
                    }
                }
                else
                {
                    // album is found in DB
                    albumToAdd = foundAlbum;
                }
            }
            return albumToAdd;
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
            try
            {
                var foundArtist = GetFirstArtistIfExists(artistName);
                if (foundArtist == null)
                {
                    foundArtist = AddArtist(artistName);
                    DB.SaveChanges();
                }

                var foundAlbum = GetFirstAlbumIfExists(artistName, albumName);
                var genre = GetLastGenreIfExistsByArtist(artistName);
                if (genre == null)
                    AddNewUnknownGenreToArtist(artistName);
                
                // No similar
                if (foundAlbum == null /*&& artAlbs.Count() == 0*/)
                {
                    // No similar, No Album name retrieved
                    if ( string.IsNullOrEmpty(albumName) || albumName.ToLower().Trim() == "unknown" )
                        newAlbum = AddNewUnknownAlbumToExistingArtistAndGenre(foundArtist, genre);
                    // No similar, But Album name retrieved, create new
                    else
                    {
                        newAlbum = AddNewAlbumToExistingArtistAndGenre(albumName, year, label, type, foundArtist, genre);
                    }
                    // Has similar, return it
                } else if ( foundAlbum.AlbumName.ToLower().Trim() == albumName.ToLower().Trim() ) {
                    return foundAlbum;
                } 

                return newAlbum;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke("AddAlbum(): " + ex.Message);
                return null;
            }
        }

        private Album AddNewAlbumToExistingArtistAndGenre(string albumName, long? year, string label, string type, Artist foundArtist, Genre genre)
        {
            Album newAlbum;
            newAlbum = new Album()
            {
                AlbumID = GetNewAlbumID(),
                ArtistID = foundArtist?.ArtistID,
                AlbumName = albumName,
                GenreID = genre.GenreID,
                //ArtistName = targetArtist.ArtistName,
                Label = label,
                Type = type,
                Year = year
            };
            DB.AddEntity(newAlbum);
            DB.SaveChanges();
            return newAlbum;
        }

        public AlbumGenre AddAlbumGenre(Artist artist, Album album, Genre genre)
        {
            var ag = GetFirstAlbumGenreIfExists(artist, album);

            var albG = new AlbumGenre()
            {
                AlbumID = album.AlbumID,
                GenreID = genre.GenreID
            };

            if (album.AlbumGenres == null)
                album.AlbumGenres = new HashSet<AlbumGenre>();
            DB.AddEntity(albG);
            DB.UpdateAndSaveChanges(album);
            return albG;
        }

        /// <summary>
        /// Returns false if does not exist.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public bool ArtistExists(string artistName)
        {
            var artistMatches = GetPossibleArtists(artistName);

            if (artistMatches.Count() == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Returns null if does not exist.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public Artist GetFirstArtistIfExists(string artistName)
        {
            var artistMatches = GetPossibleArtists(artistName);

            if (artistMatches.Count() == 0)
                return null;
            return artistMatches.FirstOrDefault();
        }

        /// <summary>
        /// Lazy pick first genre.
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        public Genre GetFirstGenreIfExists(string genreName)
        {
            var genreMatches = GetPossibleGenres(genreName);

            if (genreMatches.Count() == 0)
                return null;
            return genreMatches.FirstOrDefault();
        }

        /// <summary>
        /// Lazy pick last genre.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        public Genre GetLastGenreIfExistsByArtist(string artistName)
        {
            try
            {
                var genreMatches = GetPossibleGenresByArtistName(artistName);

                if (genreMatches.Count() == 0)
                    return null;

                return genreMatches.OrderBy(g => g.GenreID).Last();
            } catch (NullReferenceException) {
                return null;
            }
        }

        public ArtistGenre GetFirstArtistGenreIfExists(Guid artistID, Guid genreID)
        {
            var ags = DB.GetArtistGenres().Where(ag => ag.ArtistID == artistID && ag.GenreID == genreID);

            if (ags.Count() == 0)
                return null;
            return ags.FirstOrDefault();
        }

        public Genre GetFirstGenreIfExistsByArtist(string artistName)
        {
            var genreMatches = GetPossibleGenresByArtistName(artistName);

            if (genreMatches.Count() == 0)
                return null;
            return genreMatches.FirstOrDefault();
        }
        /// <summary>
        /// Returns null if does not exist.
        /// </summary>
        /// <param name="artistName">Possible artist name.</param>
        /// <param name="albumName">Possible album name.</param>
        /// <returns></returns>
        public Album GetFirstAlbumIfExists(string artistName, string albumName)
        {
            var albumMatches = GetPossibleAlbums(artistName, albumName);

            if (albumMatches.Count() == 0)
                return null;
            return albumMatches.FirstOrDefault();
        }

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



            //Album alb = new Album() { ArtistID = targetArtist.ArtistID, AlbumName = albumName,
            //    ArtistName = artistName, GroupFormationDate = groupFormationDate, Label = label,
            //    Type = type, Year = year};

            var alb = new Album()
            {
                ArtistID = targetArtist.ArtistID,
                AlbumName = albumName,
            };

            var cmp = new Composition()
            {
                ArtistID = targetArtist.ArtistID,
                CompositionName = compositionName,
                Duration = duration,
                FilePath = filePath,
            };

            DB.AddEntity(cmp);
        }

        public IQueryable<Artist> GetPossibleArtists(string name)
        {
            var matches = from match in DB.GetArtists() where match.ArtistName == name select match;

            return matches;
        }

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

        public IQueryable<Album> GetPossibleAlbums(Guid artistID, string albumName)
        {
            var matches = from match in DB.GetAlbums()
                          where (match.AlbumName == albumName &&
                          match.ArtistID == artistID)
                          select match;

            return matches;
        }

        public IQueryable<Album> GetPossibleAlbums(string artistName, string albumName)
        {
            var result = from album in DB.GetAlbums()
                         join artist in DB.GetArtists()
                        on album.ArtistID equals artist.ArtistID
                         where ((artist.ArtistName == artistName) &&
                     (album.AlbumName == albumName))
                         select album;

            //var debugAlbumGenres = (from aG in DB.AlbumGenres select aG).ToList();

            return result;
        }


        public bool ContainsArtist(string artistName, List<Artist> artists)
        {
            foreach (var artist in artists)
                if (artist.ArtistName == artistName)
                    return true;
            return false;
        }

        public ArtistGenre AddArtistGenre(string artistName, string genreName,
            long? dateOfDisband = null, Action<string> errorAction = null
        )
        {
            try
            {
                if (genreName == null || genreName == string.Empty)
                {
                    if(errorAction != null) errorAction.Invoke("AddArtistGenre exception : genreName == null");
                    return null;
                }
                var firstArtist = GetFirstArtistIfExists(artistName);

                if (firstArtist == null)
                {
                    if(errorAction != null) errorAction.Invoke("AddArtistGenre exception : no matching artist exist <"+artistName+">.");
                    return null;
                }

                var newAGenre = new ArtistGenre() { GenreName = genreName };
                firstArtist = DB.GetArtists().FirstOrDefault(x => x.ArtistID == firstArtist.ArtistID);
                //todo: check for changes
                //newAGenre.Genre = DB.Genres.Find(genreName);
                newAGenre.Genre = GetFirstGenreIfExists(genreName);

                if (newAGenre.Genre == null)
                    newAGenre.Genre = new Genre { GenreName = genreName };

                //GetFirstArtistIfExists(firstArtist.ArtistName).ArtistGenres.Add(newAGenre);
                DB.AddEntity(newAGenre); 
                DB.SaveChanges();
                return newAGenre;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public bool ArtistHasGenre(Artist artist, string possibleGenre)
        {
            var genres = from gen in artist.ArtistGenres where gen.GenreName == possibleGenre select gen;

            if (genres.Count() == 0)
                return false;
            return true;
        }

        public void Update<TDBContext>() where TDBContext : IDMDBContext, new()
        {
            if (DB == null)
            {
                //DB.Dispose();
                //DB = null;
                DB = new TDBContext();
            }
        }

        public string ToMD5(string source)
        {
            var buffer = Encoding.Default.GetBytes(source);

            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(buffer);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public Artist AddArtist(string artistFileName, Action<string> errorAction = null)
        {
            try
            {
                Artist artistToAdd;
                if (artistFileName == null)
                {
                    artistToAdd = GetFirstArtistIfExists("unknown");
                    if (artistToAdd == null)
                    {
                        DB.AddEntity(artistToAdd = new Artist() { ArtistName = "unknown", ArtistID = GetNewArtistID() });
                    }
                    return artistToAdd;
                }
                artistToAdd = GetFirstArtistIfExists(artistFileName);
                if (artistToAdd == null)
                {
                    artistToAdd = new Artist() { ArtistName = artistFileName };
                    try
                    {
                        artistToAdd.ArtistID = GetNewArtistID();
                    }
                    catch
                    {
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
                var countOfArtists = DB.GetArtists().Count();
                if(errorAction != null) errorAction.Invoke("AddArtist(): " + ex.Message);
                return null;
            }
        }

        public ArtistGenre FindArtistGenreOrReturnNull(Guid artistID, string genreName)
        {
            var query = from ag in DB.GetArtistGenres()
                        where ag.ArtistID == artistID &&
                        ag.GenreName == genreName
                        select ag;
            if (query.Any())
                return query.FirstOrDefault();
            else
                return null;
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
                    DateOfApplication = DateTime.Now,
                    Genre = genre,
                    GenreName = genre.GenreName,
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

        public Genre AddGenreToArtist(Artist artist, string newGenre, Action<string> errorAction = null)
        {
            try
            {
                Genre genre;
                AddNewGenreOrSetToUnknown(out genre, newGenre);
                AddNewArtistGenre(artist, genre);
                return genre;
            } catch (Exception ex) {
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

        /// <summary>
        /// This method changes the existing composition if it exists
        /// </summary>
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

                if (title != null)
                {
                    try
                    {
                        newComposition.CompositionID = GetNewCompositionID();
                        newComposition.CompositionName = title;
                        newComposition.ArtistID = artist?.ArtistID;
                        newComposition.AlbumID = album?.AlbumID;
                        newComposition.Album = album;
                        newComposition.Artist = artist;

                        try
                        {
                            newComposition.Duration = (long?)duration.TotalSeconds;
                            newComposition.FilePath = fileName;
                        }
                        catch
                        {
                            //leave them null
                        }

                        DB.AddEntity(newComposition);
                        DB.SaveChanges();
                        return newComposition;
                    }
                    catch (Exception ex)
                    {
                        if(errorAction != null) errorAction.Invoke(ex.Message);
                        return null;
                    }
                }
                else
                {
                    if(errorAction != null) errorAction.Invoke("title is null");
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

        public ListenedComposition FindFirstListenedComposition(Composition composition)
        {
            var matches = from lC in DB.GetListenedCompositions()
                          where lC.CompositionID == composition.CompositionID
                          select lC;
            if (matches.Count() == 0)
                return null;
            return matches.FirstOrDefault();
        }

        public void AddNewListenedComposition(Composition newC, User user,
            Action<string> errorAction = null)
        {
            try
            {
                var existingComps = (
                    from lc in DB.GetListenedCompositions()
                    join c in DB.GetCompositions()
                        on lc.CompositionID equals c.CompositionID
                    join u in DB.GetUsers()
                        on lc.UserID equals u.UserID
                    where
                        lc.UserID == user.UserID &&
                        c.CompositionName == newC.CompositionName &&
                        c.AlbumID == newC.AlbumID &&
                        c.ArtistID == newC.ArtistID
                    select lc

                    )
                ;
                if (existingComps != null &&
                    existingComps.Any())
                {
                    var last = existingComps.FirstOrDefault();
                    last.CountOfPlays += 1;
                    last.ListenDate = DateTime.Now;
                    DB.SaveChanges();
                    return;
                }
                /*public long*/
                var UserID = user.UserID;
                AddDefaultUserIfNotExists(user);
                var CompositionID = newC.CompositionID;
                var lC = new ListenedComposition()
                {
                    ListenedCompositionID = Guid.NewGuid(),
                    ListenDate = DateTime.Now,
                    CompositionID = newC.CompositionID,
                    CountOfPlays = 1,
                    UserID = user.UserID
                };

                DB.AddEntity(lC);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
        }

        private void AddDefaultUserIfNotExists(User user)
        {
            if (user.UserID == Guid.Empty)
            {
                if (DB.GetUsers().Count() <= 0 ||
                    DB.GetUsers().Where(u => u.UserID == Guid.Empty).Count() <= 0
                )
                {
                    DB.Add(new User() { UserID = Guid.Empty, UserName = "__Default", Email = "de@fau.lt", DateOfSignUp = DateTime.Now, Password = "password" });
                    DB.SaveChanges();
                }
            }
        }

        public void CopyFieldsExceptForDurationAndPath(Composition existingComp, Composition comp)
        {
            comp.About = existingComp.About; comp.Album = existingComp.Album;
            comp.AlbumID = existingComp.AlbumID; comp.Artist = existingComp.Artist;
            comp.ArtistID = existingComp.ArtistID; comp.CompositionName = existingComp.CompositionName;
        }

        public bool HasAdminRights(User user,
            Action<string> errorAction = null)
        {
            //var matches = from user in DB.Administrators join 
            try
            {
                var adminQuery = from u in DB.GetUsers()
                                 join a in DB.GetAdministrators()
                                 on u.UserID equals a.UserID
                                 where a.UserID == user.UserID
                                 select u;

                if (adminQuery.Any())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
            return false;
        }

        public bool HasModerRights(User user,
            Action<string> errorAction = null)
        {
            //var matches = from user in DB.Administrators join 
            try
            {
                var moderQuery = from u in DB.GetUsers()
                                 join m in DB.GetModerators()
                                 on u.UserID equals m.UserID
                                 where m.UserID == user.UserID
                                 select u;

                if (moderQuery.Any())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
            return false;
        }

        public User AddNewUser(string login, string psswd,
            string email, string bio,
            string VKLink = "null", string FaceBookLink = "null",
            Action<string> errorAction = null)
        {
            DateTime lastListenedDataModificationDate = DateTime.MinValue;
            //DateTime 

            try
            {
                var user = new User();
                Guid id = Guid.NewGuid();

                user.UserName = login;
                user.Email = email;
                user.Password = ToMD5(psswd);
                user.DateOfSignUp = DateTime.Now;
                user.Bio = bio;

                user.VKLink = "null";
                user.FaceBookLink = "null";
                user.UserID = id;

                DB.AddEntity(user);
                DB.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public Moderator AddNewModerator(Guid userID,
            Action<string> errorAction = null)
        {
            try
            {
                var user = DB.GetUsers().FirstOrDefault(u => u.UserID == userID);
                if (user == null)
                    return null;
                var moders = DB.GetModerators().Where(m => m.UserID == userID);
                if (moders.Any())
                {
                    return moders.FirstOrDefault();
                }

                var moderator = new Moderator();
                moderator.UserID = userID;
                moderator.ModeratorID = GetNewModeratorID();

                DB.AddEntity(moderator);
                DB.SaveChanges();
                return moderator;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public Administrator AddNewAdministrator(Guid userID, Guid moderID,
            Action<string> errorAction = null)
        {
            try
            {
                var user = DB.GetUsers().FirstOrDefault( x => x.UserID == userID);
                if (user == null)
                    return null;

                var moders = DB.GetModerators().FirstOrDefault(x => x.ModeratorID == moderID);
                if (moders == null)
                    return null;

                var admins = DB.GetAdministrators().Where(a => a.UserID == userID);
                if (admins.Any())
                {
                    return admins.FirstOrDefault();
                }

                var administrator = new Administrator();
                administrator.UserID = userID;
                administrator.ModeratorID = moderID;
                administrator.AdministratorID = GetNewAdministratorID();

                DB.AddEntity(administrator);
                DB.SaveChanges();
                return administrator;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public bool DeleteComposition(Guid ID,
            Action<string> errorAction = null)
        {
            try
            {
                DB.RemoveEntity(DB.GetCompositions().FirstOrDefault(x => x.CompositionID == ID));
                if(DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }

        public bool DeleteAlbum(Guid ID, Action<string> errorAction = null)
        {
            try
            {
                DB.RemoveEntity(DB.GetAlbums().FirstOrDefault(x => x.AlbumID == ID));
                if(DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }
        public bool DeleteAlbum(Album album, Action<string> errorAction = null)
        {
            try
            {
                if(DB != null) DB.RemoveEntity(DB.GetAlbums().FirstOrDefault(x => x.AlbumID == album.AlbumID));
                if(DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }

        public bool DeleteComposition(Composition composition,
            Action<string> errorAction = null)
        {
            try
            {
                if(DB != null) DB.RemoveEntity(DB.GetCompositions().FirstOrDefault(x => x.CompositionID == composition.CompositionID));
                if(DB != null) DB.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }

        public bool DeleteListenedComposition(ListenedComposition composition,
            Action<string> errorAction = null)
        {
            try
            {
                var matches = DB.GetListenedCompositions().Where(c => c.ListenDate == composition.ListenDate && c.UserID == composition.UserID);
                if (matches.Any())
                {
                    var countOfPlays = matches.FirstOrDefault().CountOfPlays;
                    DB.RemoveEntity(matches.FirstOrDefault());
                    DB.SaveChanges();
                    var newMatches = DB.GetListenedCompositions().Where(c => c.ListenDate == composition.ListenDate &&
                    c.UserID == composition.UserID &&
                    composition.CompositionID == c.CompositionID
                    );
                    if (newMatches.Any())
                    {
                        newMatches.FirstOrDefault().CountOfPlays += countOfPlays;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }


        public IQueryable<ListenedComposition> GetCurrentUsersListenedCompositions(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   select comp;
        }

        public IQueryable<ListenedComposition> GetCurrentUsersListenedGenres(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   select comp;
        }

        public IQueryable<ListenedComposition> GetCurrentUsersListenedArtist(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   select comp;
        }

        public bool ClearListenedCompositions()
        {
            return DB.ClearTable("ListenedCompositions");
        }
    }
}
