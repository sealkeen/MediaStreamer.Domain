using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public static class ListICompositionExtension
    {
        public static List<Composition> FromListOfInterfaces(this List<IComposition> list)
        {
            List<Composition> result = new List<Composition>();
            foreach (var cInterface in list)
            {
                result.Add(cInterface.GetInstance());
            }
            return result;
        }
    }
}