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
#if NOT_DEFINED_
    using System.ComponentModel.DataAnnotations;
#endif

    public partial class Picture
    {
        public long PictureID { get; set; }
        public Nullable<long> XResolution { get; set; }
        public Nullable<long> YResolution { get; set; }
        public Nullable<long> SizeKb { get; set; }
#if NOT_DEFINED_
        [MaxLength(4000)]
#endif
        public string FilePath { get; set; }
    }
}
