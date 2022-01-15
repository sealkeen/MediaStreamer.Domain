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
#if !NET40
    using System.ComponentModel.DataAnnotations;
#endif

    public partial class ListenedGenre
    {
        public Nullable<long> CountOfPlays { get; set; }
        public long UserID { get; set; }
#if !NET40
        [StringLength(50)]
#endif

        public long GenreID { get; set; }
        public string GenreName { get; set; }
    
        public virtual Genre Genre { get; set; }
        public virtual User User { get; set; }
    }
}
