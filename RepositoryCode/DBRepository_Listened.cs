using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreamer.Domain
{
    public partial class DBRepository
    {
        public IQueryable<ListenedComposition> GetCurrentUsersListenedCompositions(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   orderby comp.CountOfPlays descending
                   select comp;
        }

        public IQueryable<ListenedComposition> GetCurrentUsersListenedGenres(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   select comp;
        }

        public IQueryable<ListenedComposition> GetCurrentUsersListenedArtist(User user)
        {
            return from comp in DB.GetListenedCompositions()
                   where comp.UserID == user.UserID
                   select comp;
        }

        public ListenedComposition FindFirstListenedComposition(Composition composition)
        {
            var matches = from lC in DB.GetListenedCompositions()
                          where lC.CompositionID == composition.CompositionID
                          select lC;
            if (matches.Count() == 0)
                return null;
            return matches.FirstOrDefault();
        }

        public async Task AddNewListenedCompositionAsync(Composition newC, User user,
            Action<string> errorAction = null)
        {
#if !NET40
            await Task.Factory.StartNew(() => AddNewListenedComposition(newC, user, errorAction));
#else
            Task.Factory.StartNew(() => AddNewListenedComposition(newC, user, errorAction));
#endif
        }

        public void AddNewListenedComposition(Composition newC, User user,
            Action<string> errorAction = null)
        {
            try
            {
                var existingComps = (
                    from lc in DB.GetListenedCompositions() where
                        lc.UserID == user.UserID &&
                        lc.CompositionID == newC.CompositionID
                    select lc
                    ).OrderBy(c => c.CompositionID)
                ;
                if (existingComps != null &&
                    existingComps.Any())
                {
                    var last = existingComps.FirstOrDefault();
                    last.CountOfPlays += 1;
                    last.ListenDate = DateTime.Now;

                    DB.UpdateAndSaveChanges(last);
                    return;
                }
                /*public long*/
                var UserID = user.UserID;
                AddDefaultUserIfNotExists(user);
                var CompositionID = newC.CompositionID;
                var lC = new ListenedComposition()
                {
                    ListenDate = DateTime.Now,
                    CompositionID = newC.CompositionID,
                    CountOfPlays = 1,
                    UserID = user.UserID
                };

                DB.AddEntity(lC);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                if(errorAction != null) errorAction.Invoke(ex.Message);
            }
        }

        public bool ClearListenedCompositions()
        {
            return DB.ClearTable("ListenedCompositions");
        }

        public bool DeleteListenedComposition(ListenedComposition composition,
            Action<string> errorAction = null)
        {
            try {
                var matches = DB.GetListenedCompositions().Where(c => c.ListenDate == composition.ListenDate && c.UserID == composition.UserID);
                if (matches.Any()) {
                    var countOfPlays = matches.FirstOrDefault().CountOfPlays;
                    DB.RemoveEntity(matches.FirstOrDefault());
                    DB.SaveChanges();
                    var newMatches = DB.GetListenedCompositions()
                     .Where(
                        c => c.ListenDate == composition.ListenDate && 
                        c.UserID == composition.UserID && 
                        composition.CompositionID == c.CompositionID
                    );
                    if (newMatches.Any()) {
                        newMatches.FirstOrDefault().CountOfPlays += countOfPlays;
                    }
                }
                return true;
            } catch (Exception ex) {
                if(errorAction != null) errorAction.Invoke(ex.Message);
                return false;
            }
        }
    }
}
