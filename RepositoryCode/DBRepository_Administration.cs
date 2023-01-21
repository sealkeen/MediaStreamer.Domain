using System;

namespace MediaStreamer.Domain
{
    public partial class DBRepository 
    {
        public Guid GetNewModeratorID()
        {
            return Guid.NewGuid();
        }

        public Guid GetNewAdministratorID()
        {
            return Guid.NewGuid();
        }

    }
}
