using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IComposition
    {
        string GetPath();
        Composition GetInstance();
        Guid CompositionID { get; set; }
        string CompositionName { get; set; }
        Nullable<long> Duration { get; set; }
        string FilePath { get; set; }
        string Lyrics { get; set; }
        string About { get; set; }
        Nullable<Guid> ArtistID { get; set; }
        Nullable<Guid> AlbumID { get; set; }
        Album Album { get; set; }
        Artist Artist { get; set; }
    }
}
