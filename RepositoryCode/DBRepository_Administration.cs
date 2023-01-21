using System;
using System.Linq;

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
        private void AddDefaultUserIfNotExists(User user)
        {
            if (user.UserID == Guid.Empty)
            {
                if (DB.GetUsers().Count() <= 0 ||
                    DB.GetUsers().Where(u => u.UserID == Guid.Empty).Count() <= 0
                )
                {
                    DB.Add(new User() { UserID = Guid.Empty, UserName = "__Default", Email = "de@fau.lt", DateOfSignUp = DateTime.Now, Password = "password" });
                    DB.SaveChanges();
                }
            }
        }

        public Administrator AddNewAdministrator(Guid userID, Guid moderID,
            Action<string> errorAction = null)
        {
            try
            {
                var user = DB.GetUsers().FirstOrDefault(x => x.UserID == userID);
                if (user == null)
                    return null;

                var moders = DB.GetModerators().FirstOrDefault(x => x.ModeratorID == moderID);
                if (moders == null)
                    return null;

                var admins = DB.GetAdministrators().Where(a => a.UserID == userID);
                if (admins.Any())
                {
                    return admins.FirstOrDefault();
                }

                var administrator = new Administrator();
                administrator.UserID = userID;
                administrator.ModeratorID = moderID;
                administrator.AdministratorID = GetNewAdministratorID();

                DB.AddEntity(administrator);
                DB.SaveChanges();
                return administrator;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public Moderator AddNewModerator(Guid userID,
            Action<string> errorAction = null)
        {
            try
            {
                var user = DB.GetUsers().FirstOrDefault(u => u.UserID == userID);
                if (user == null)
                    return null;
                var moders = DB.GetModerators().Where(m => m.UserID == userID);
                if (moders.Any())
                {
                    return moders.FirstOrDefault();
                }

                var moderator = new Moderator();
                moderator.UserID = userID;
                moderator.ModeratorID = GetNewModeratorID();

                DB.AddEntity(moderator);
                DB.SaveChanges();
                return moderator;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public User AddNewUser(string login, string psswd,
            string email, string bio,
            string externalAccoountId = "null", string FaceBookLink = "null",
            Action<string> errorAction = null)
        {
            DateTime lastListenedDataModificationDate = DateTime.MinValue;
            //DateTime 

            try
            {
                var user = new User();
                Guid id = Guid.NewGuid();
                user.UserID = id;
                user.UserName = login;
                user.Email = email;
                user.Password = ToMD5(psswd);
                user.DateOfSignUp = DateTime.Now;
                user.Bio = bio;

                user.AspNetUserId = externalAccoountId;

                DB.Add(user);
                DB.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
                return null;
            }
        }

        public bool HasModerRights(User user,
            Action<string> errorAction = null)
        {
            //var matches = from user in DB.Administrators join 
            try
            {
                var moderQuery = from u in DB.GetUsers()
                                 join m in DB.GetModerators()
                                 on u.UserID equals m.UserID
                                 where m.UserID == user.UserID
                                 select u;

                if (moderQuery.Any())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
            }
            return false;
        }

        public bool HasAdminRights(User user,
            Action<string> errorAction = null)
        {
            //var matches = from user in DB.Administrators join 
            try
            {
                var adminQuery = from u in DB.GetUsers()
                                 join a in DB.GetAdministrators()
                                 on u.UserID equals a.UserID
                                 where a.UserID == user.UserID
                                 select u;

                if (adminQuery.Any())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
            }
            return false;
        }
    }
}
