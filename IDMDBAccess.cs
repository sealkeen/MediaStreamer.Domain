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
        //Task<IDBRepository> LoadingTask { get; set; }
        void OnStartup();
        long GetNewCompositionID();
        long GetNewArtistID();
        long GetNewAlbumID();
        long GetNewModeratorID();
        long GetNewAdministratorID();
        void PopulateDataBase(Action<string> errorAction = null);
        void RemoveGroupMember(string artistName, long formationDate,
            long? dateOfDisband = null);

        GroupMember AddGroupMember(string artistName, DateTime formationDate,
            long? dateOfDisband = null
        //, FirstFMEntities DB = null
        );
        Album AddAlbum(
            string artistName,
            string albumName,
            //long artistID = -1,
            //long groupFormationDate = -1,
            long? year = null,
            string label = null,
            string type = null,
            Action<string> errorAction = null
        );
        /// <summary>
        /// Returns false if does not exist.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        bool ArtistExists(string artistName);

        /// <summary>
        /// Returns null if does not exist.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns></returns>
        Artist GetFirstArtistIfExists(string artistName);
        Genre GetFirstGenreIfExistsByArtist(string artistName);
        /// <summary>
        /// Returns null if does not exist.
        /// </summary>
        /// <param name="artistName">Possible artist name.</param>
        /// <param name="albumName">Possible album name.</param>
        /// <returns></returns>
        Album GetFirstAlbumIfExists(string artistName, string albumName);


        IQueryable<Artist> GetPossibleArtists(string name);

        IQueryable<Genre> GetPossibleGenres(string name);

        IQueryable<Album> GetPossibleAlbums(long artistID, string albumName);

        IQueryable<Album> GetPossibleAlbums(string artistName, string albumName);

        IQueryable<GroupMember> GetPossibleGroupMembers(long artistID);

        IQueryable<GroupMember> GetPossibleGroupMembers(string artistName);

        bool ContainsArtist(string artistName, List<Artist> artists);

        void AddArtistGenre(string artistName, string genreName,
                    long? dateOfDisband = null, Action<string> errorAction = null);
        
        bool ArtistHasGenre(Artist artist, string possibleGenre);

        void Update<TDBContext>() where TDBContext : IDMDBContext, new();

        string ToMD5(string source);

        Artist AddArtist(string artistFileName, Action<string> errorAction = null);

        Genre AddGenreToArtist(Artist artist, string newGenre, Action<string> errorAction = null);

        Album AddAlbum(
            Artist artist, Genre genre, string albumFromFile,
            string label = null, DateTime? gFD = null,
            string type = null, long? year = null);

        /// <summary>
        /// This method removes the existing composition and add a new one for its place
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="album"></param>
        /// <param name="title"></param>
        /// <param name="duration"></param>
        /// <param name="fileName"></param>
        /// <param name="yearFromFile"></param>
        /// <param name="onlyReturnNoAppend"></param>
        /// <returns></returns>
        Composition AddComposition(Artist artist, Album album,
            string title, TimeSpan duration,
            string fileName, long? yearFromFile = null,
            bool onlyReturnNoAppend = false, Action<string> errorAction = null);
        /// <summary>
        /// Bad function, don't use it
        /// </summary>
        /// <param name="artistName"></param>
        /// <param name="compositionName"></param>
        /// <param name="albumName"></param>
        /// <param name="duration"></param>
        /// <param name="filePath"></param>
        void AddComposition(
            string artistName,
            string compositionName,
            string albumName,
            long? duration = null,
            string filePath = null
        );
        void ChangeExistingComposition(Artist artist,
            Album album, string title, TimeSpan duration, string fileName,
            bool onlyReturnNoAppend, Composition newComposition, Composition existingComp,
            Action<string> errorAction = null);

        ListenedComposition FindFirstListenedComposition(Composition composition);

        void AddNewListenedComposition(Composition composition, User user, Action<string> errorAction = null);

        void CopyFieldsExceptForDurationAndPath(Composition existingComp,
            Composition comp);
        bool HasAdminRights(User user, Action<string> errorAction = null);
        bool HasModerRights(User user, Action<string> errorAction = null);

        User AddNewUser(string login, string psswd,
            string email, string bio,
            string VKLink = "null", string FaceBookLink = "null", Action<string> errorAction = null);

        Moderator AddNewModerator(long userID, Action<string> errorAction = null);

        Administrator AddNewAdministrator(long userID, long moderID, Action<string> errorAction = null);

        bool DeleteComposition(long ID, Action<string> errorAction = null);

        bool DeleteComposition(Composition composition, Action<string> errorAction = null);

        bool DeleteListenedComposition(ListenedComposition composition, Action<string> errorAction = null);
        bool DeleteAlbum(long ID, Action<string> errorAction = null);
        bool DeleteAlbum(Album album, Action<string> errorAction = null);

        IQueryable<ListenedComposition> GetCurrentUsersListenedCompositions(User user);

        IQueryable<ListenedComposition> GetCurrentUsersListenedGenres(User user);

        IQueryable<ListenedComposition> GetCurrentUsersListenedArtist(User user);
    }
}

