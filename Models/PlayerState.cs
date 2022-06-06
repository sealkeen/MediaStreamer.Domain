using System;
using System.Collections.Generic;
using System.Text;

namespace MediaStreamer.Domain
{
    public class PlayerState
    {
        public long StateID { get; set; }
        public DateTime StateTime { get; set; }
        public double VolumeLevel { get; set; }
    }
}