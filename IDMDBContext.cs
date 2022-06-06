using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreamer.Domain
{
    public interface IDMDBContext : IDisposable
    {
        void DisableLazyLoading();
        void Clear();
        void EnsureCreated();
        //Task ClearAsync();
        void AddEntity<T>(T o) where T : class;
        void RemoveEntity<T>(T o) where T : class;
        void UpdateAndSaveChanges<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
        bool ClearTable(string tableName);
        string GetContainingFolderPath();
        IQueryable<Administrator> GetAdministrators();
        void Add(Administrator administrator);
        IQueryable<Album> GetAlbums();
        void Add(Album administrator);
        IQueryable<AlbumGenre> GetAlbumGenres();
        void Add(AlbumGenre albumGenre);
        IQueryable<Artist> GetArtists();
        void Add(Artist artist);
        IQueryable<ArtistGenre> GetArtistGenres();
        void Add(ArtistGenre artistGenre);
        IQueryable<Composition> GetCompositions();
        Task<IQueryable<Composition>> GetCompositionsAsync();
        IQueryable<IComposition> GetICompositions();
        void Add(Composition composition);
        IQueryable<CompositionVideo> GetCompositionVideos();
        void Add(CompositionVideo compositionVideo);
        IQueryable<Genre> GetGenres();
        void Add(Genre genre);
        IQueryable<ListenedComposition> GetListenedCompositions();
        void Add(ListenedComposition listenedComposition);
        IQueryable<Moderator> GetModerators();
        void Add(Moderator moderator);
        IQueryable<Picture> GetPictures();
        void Add(Picture picture);
        IQueryable<User> GetUsers();
        void Add(User user);
        IQueryable<Video> GetVideos();
        void Add(Video video);
    }
}
