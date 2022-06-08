using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaStreamer.Domain;

namespace MediaStreamer.Domain
{
    public interface IDBRepository
    {
        IDMDBContext DB { get; set; }
        void OnStartup();
        Guid GetNewCompositionID();
        Guid GetNewArtistID();
        Guid GetNewAlbumID();
        Guid GetNewModeratorID();
        Guid GetNewAdministratorID();
        void PopulateDataBase(Action<string> errorAction = null);

        /// <returns>Returns null if not successful.</returns>
        Album AddAlbum( string artistName, string albumName,
            //Guid artistID = -1, long groupFormationDate = -1,
            long? year = null, string label = null, string type = null, 
            Action<string> errorAction = null 
            );

        /// <returns>Returns null if not successful.</returns>
        ArtistGenre AddArtistGenre(string artistName, string genreName, long? dateOfDisband = null, Action<string> errorAction = null);

        /// <returns>Returns null if not successful.</returns>
        Artist AddArtist(string artistFileName, Action<string> errorAction = null);

        /// <returns>Returns null if not successful.</returns>
        Genre AddGenreToArtist(Artist artist, string newGenre, Action<string> errorAction = null);

        /// <returns>Returns null if not successful.</returns>
        Album AddAlbum(
            Artist artist, Genre genre, string albumFromFile,
            string label = null, string type = null, long? year = null);

        /// <summary>
        /// This method changes the existing composition
        /// </summary>
        /// <returns></returns>
        Composition AddComposition(Artist artist, Album album,
            string title, TimeSpan duration,
            string fileName, long? yearFromFile = null,
            bool onlyReturnNoAppend = false, Action<string> errorAction = null);

        /// <summary>
        /// Bad function, don't use it
        /// </summary>
        void AddComposition(
            string artistName,
            string compositionName,
            string albumName,
            long? duration = null,
            string filePath = null
        );
        void AddNewListenedComposition(Composition composition, User user, Action<string> errorAction = null);

        /// <returns>Returns false if does not exist.</returns>
        bool ArtistExists(string artistName);

        /// <returns>null if does not exist.</returns>
        Artist GetFirstArtistIfExists(string artistName);

        /// <returns>null if does not exist.</returns>
        Genre GetFirstGenreIfExistsByArtist(string artistName);

        /// <returns>null if does not exist.</returns>
        Album GetFirstAlbumIfExists(string artistName, string albumName);

        IQueryable<Artist> GetPossibleArtists(string name);
        IQueryable<Genre> GetPossibleGenres(string name);
        IQueryable<Album> GetPossibleAlbums(Guid artistID, string albumName);
        IQueryable<Album> GetPossibleAlbums(string artistName, string albumName);

        bool ContainsArtist(string artistName, List<Artist> artists);

        bool ArtistHasGenre(Artist artist, string possibleGenre);

        void Update<TDBContext>() where TDBContext : IDMDBContext, new();

        string ToMD5(string source);

        void ChangeExistingComposition(Artist artist,
            Album album, string title, TimeSpan duration, string fileName,
            bool onlyReturnNoAppend, Composition newComposition, Composition existingComp,
            Action<string> errorAction = null);

        ListenedComposition FindFirstListenedComposition(Composition composition);

        void CopyFieldsExceptForDurationAndPath(Composition existingComp, Composition comp);

        bool HasAdminRights(User user, Action<string> errorAction = null);
        bool HasModerRights(User user, Action<string> errorAction = null);
        User AddNewUser(string login, string psswd,
            string email, string bio,
            string VKLink = "null", string FaceBookLink = "null", Action<string> errorAction = null);

        Moderator AddNewModerator(Guid userID, Action<string> errorAction = null);

        Administrator AddNewAdministrator(Guid userID, Guid moderID, Action<string> errorAction = null);

        bool DeleteComposition(Guid ID, Action<string> errorAction = null);
        bool DeleteComposition(Composition composition, Action<string> errorAction = null);
        bool DeleteListenedComposition(ListenedComposition composition, Action<string> errorAction = null);
        bool DeleteAlbum(Guid ID, Action<string> errorAction = null);
        bool DeleteAlbum(Album album, Action<string> errorAction = null);
        bool ClearListenedCompositions();

        IQueryable<ListenedComposition> GetCurrentUsersListenedCompositions(User user);
    }
}

