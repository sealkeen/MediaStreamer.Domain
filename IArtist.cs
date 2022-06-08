using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IArtist
    {
        Guid ArtistID { get; set; }
        string ArtistName { get; set; }
    }
}
