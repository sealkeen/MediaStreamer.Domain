using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IAlbum
    {
        Guid AlbumID { get; set; }
        string AlbumName { get; set; }
        Nullable<long> Year { get; set; }
    }
}
