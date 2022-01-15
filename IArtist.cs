using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IArtist
    {
        long ArtistID { get; set; }
        string ArtistName { get; set; }
    }
}
