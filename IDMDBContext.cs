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
        int SaveChanges();
        void AddEntity<T>(T o) where T : class;
        void RemoveEntity<T>(T o) where T : class;
        void UpdateAndSaveChanges<TEntity>(TEntity entity) where TEntity : class;
        bool ClearTable(string tableName);
        string GetContainingFolderPath();
        IQueryable<Administrator> GetAdministrators();
        IQueryable<Album> GetAlbums();
        IQueryable<AlbumGenre> GetAlbumGenres();
        IQueryable<Artist> GetArtists();
        IQueryable<ArtistGenre> GetArtistGenres();
        IQueryable<Composition> GetCompositions();
        IQueryable<IComposition> GetICompositions();
        IQueryable<CompositionVideo> GetCompositionVideos();
        IQueryable<Genre> GetGenres();
        IQueryable<ListenedComposition> GetListenedCompositions();
        IQueryable<Moderator> GetModerators();
        IQueryable<Picture> GetPictures();
        IQueryable<User> GetUsers();
        IQueryable<Video> GetVideos();
        Task<List<Composition>> GetCompositionsAsync();
        Task<List<IComposition>> GetICompositionsAsync();
        void Add(ArtistGenre artistGenre);
        void Add(Composition composition);
        void Add(CompositionVideo compositionVideo);
        void Add(Genre genre);
        void Add(ListenedComposition listenedComposition);
        void Add(Moderator moderator);
        void Add(Picture picture);
        void Add(User user);
        void Add(Video video);
        void Add(Artist artist);
        void Add(AlbumGenre albumGenre);
        void Add(Album administrator);
        void Add(Administrator administrator);
    }
}
