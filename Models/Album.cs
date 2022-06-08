//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaStreamer.Domain
{
    using System;
    using System.Collections.Generic;
#if NETCOREAPP || NET45 || NETSTANDARD
    using System.ComponentModel.DataAnnotations;
#endif
    public partial class Album
    {
        public Guid AlbumID { get; set; }

#if NETCOREAPP || NET45 || NETSTANDARD
        [StringLength(50)]
#endif
        public string AlbumName { get; set; }
        public Nullable<Guid> ArtistID { get; set; }

        public Guid GenreID { get; set; }
        public Nullable<long> Year { get; set; }

#if NETCOREAPP || NET45 || NETSTANDARD
        [StringLength(50)]
#endif
        public string Label { get; set; }

#if NETCOREAPP || NET45 || NETSTANDARD
        [StringLength(50)]
#endif
        public string Type { get; set; }
        
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ICollection<AlbumGenre> AlbumGenres { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }
    }
}
