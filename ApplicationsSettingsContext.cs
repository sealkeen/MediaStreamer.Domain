using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaStreamer.Domain
{
    public interface IApplicationsSettingsContext
    {
        IQueryable<string> GetDataSources();
        double? GetCachedVolumeLevel();
        void EnsureCreated();
        void ClearPlayerStates();
        void Add(PlayerState ps);
        void Add(DBPath path);
    }
}