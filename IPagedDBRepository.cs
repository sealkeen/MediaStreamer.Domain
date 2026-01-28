using System;

namespace MediaStreamer.Domain
{
    public interface IPagedDBRepository : IDBRepository
    {
        new void Update<TDBContext>() where TDBContext : IPagedDMDBContext, new();
        Type GetDbContextType();
    }
}
