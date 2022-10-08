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
    
    public partial class ListenedComposition : ICompositionInstance
    {
        public ListenedComposition()
        {
            UserID = Guid.Empty;
            ListenDate = DateTime.Now;
            CompositionID = Guid.Empty;
        }

        public System.DateTime ListenDate { get; set; }
        public Nullable<long> CountOfPlays { get; set; }
        public Guid UserID { get; set; }
        public Guid CompositionID { get; set; }
        public double StoppedAt { get; set; }
    
        public virtual Composition Composition { get; set; }
        public virtual User User { get; set; }
        
        public string GetPath() {
            return Composition.FilePath;
        }
        public Composition GetInstance()
        {
            return Composition;
        }
    }
}
