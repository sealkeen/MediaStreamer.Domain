using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    // Media Entity class that can be represented
    // As a column data template
    public abstract class MediaEntity
    {
        public abstract string GetID();
        public abstract bool IsValid();
        public virtual string GetTitle() { return "MediaEntity"; }
        public virtual string GetDescription() { return "Description"; }

    }
}
