using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IPagedDBRepository : IDBRepository
    {
        new void Update<TDBContext>() where TDBContext : IPagedDMDBContext, new();
        Type GetDbContextType();
    }
}
