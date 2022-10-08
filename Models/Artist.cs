//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;

namespace MediaStreamer.Domain
{
    using System;
    using System.Collections.Generic;
#if NETCOREAPP || NET45 || NETSTANDARD
    using System.ComponentModel.DataAnnotations;
#endif
    using System.Linq;

    public partial class Artist : MediaEntity
    {
        public Artist()
        {
            this.Albums = new HashSet<Album>();
            this.ArtistGenres = new HashSet<ArtistGenre>();
            this.Compositions = new HashSet<Composition>();
        }
        public Guid ArtistID { get; set; }

#if NETCOREAPP || NET45 || NETSTANDARD
        [StringLength(256)]
#endif
        public string ArtistName { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<ArtistGenre> ArtistGenres { get; set; }
        public virtual ICollection<Composition> Compositions { get; set; }

        public override string GetID()
        {
            return ArtistID.ToString();
        }
        public override string GetTitle()
        {
            return ArtistName;
        }
        public override string GetDescription()
        {
            //throw new NotImplementedException();
            if (ArtistGenres != null && ArtistGenres.Count > 0)
                return ArtistGenres.First().Genre.GenreName;
            else
                return "Genre";
        }
        public override bool IsValid()
        {
            return true;
        }
    }
}
