using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain.Views
{
    public class CompositionsResult
    {
        public IEnumerable<Composition> Compositions { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
