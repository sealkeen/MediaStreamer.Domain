using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreamer.Domain
{
    // Implements Getting of a Compositions Range Page by Page
    public interface IPagedDMDBContext : IDMDBContext
    {
        Task<List<Composition>> GetCompositionsAsync(int skip, int take);
        Task<List<IComposition>> GetICompositionsAsync(int skip, int take);
    }
}
