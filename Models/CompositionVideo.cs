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
    
    public partial class CompositionVideo
    {
        public long VideoID { get; set; }
        public long CompositionID { get; set; }
    
        public virtual Composition Composition { get; set; }
        public virtual Video Video { get; set; }
    }
}
