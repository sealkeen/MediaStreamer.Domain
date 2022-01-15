using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    // For ListenedComposition, Composition and IComposition (same syntax to get the actual instance of the class)
    public interface ICompositionInstance
    {
        Composition GetInstance();
    }
}
