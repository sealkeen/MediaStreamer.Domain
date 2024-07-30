using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaStreamer.Domain.Models;

namespace MediaStreamer.Domain
{
    public interface IDMDBContext : IDisposable
    {
        void DisableLazyLoading();
        void Clear();
        void EnsureCreated();
        int SaveChanges();
        void AddEntity<T>(T o) where T : MediaEntity;
        void RemoveEntity<T>(T o, bool saveDelayed = false) where T : MediaEntity;
        void UpdateAndSaveChanges<TEntity>(TEntity entity) where TEntity : class;
        bool ClearTable(string tableName);
        string GetContainingFolderPath();
        IQueryable<Administrator> GetAdministrators();
        IQueryable<Album> GetAlbums();
        IQueryable<AlbumGenre> GetAlbumGenres();
        IQueryable<Artist> GetArtists();
        IQueryable<ArtistGenre> GetArtistGenres();
        IQueryable<Composition> GetCompositions();
        IQueryable<Genre> GetGenres();
        IQueryable<IComposition> GetICompositions();
        IQueryable<ListenedComposition> GetListenedCompositions();
        IQueryable<Moderator> GetModerators();
        IQueryable<Picture> GetPictures();
        IQueryable<Style> GetStyles();
        IQueryable<User> GetUsers();
        Task<List<Composition>> GetCompositionsAsync();
        Task<List<IComposition>> GetICompositionsAsync();
        void Add(Administrator administrator);
        void Add(Album administrator);
        void Add(AlbumGenre albumGenre);
        void Add(Artist artist);
        void Add(ArtistGenre artistGenre);
        void Add(Composition composition);
        void Add(Genre genre);
        void Add(ListenedComposition listenedComposition);
        void Add(Moderator moderator);
        void Add(Picture picture);
        void Add(Style style);
        void Add(User user);
    }
}
