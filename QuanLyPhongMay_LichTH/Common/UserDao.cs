using QuanLyPhongMay_LichTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyPhongMay_LichTH.Common
{
    public class UserDao
    {
        Do_An_Phan_MemEntities db = null;
        public UserDao()                            
        {
            db = new Do_An_Phan_MemEntities();
            
        }
        public UserLogin GetById(string userName)
        {
            return db.UserLogins.SingleOrDefault(x => x.UserName == userName);
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.UserLogins.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }
        public int Login(string userName, string passWord/*, bool isLoginAdmin = false*/)
        {
            var result = db.UserLogins.SingleOrDefault(x => x.UserName.Trim() == userName.Trim());
            if (result == null)
            {
                return 0;
            }
            else
            {
                //if (isLoginAdmin == true)
                //{
                //if (result.GroupID == CommonConstants1.ADMIN_GROUP || result.GroupID == CommonConstants1.MOD_GROUP)
                //{
                //if (result. Status == false)
                //{
                //    return -1;
                //}
                //else
                //{
                if (result.PassWord.Trim() == Encryptor.MD5Hash(passWord).Trim())
                {
                    return 1;
                }
                else
                {
                    return -2;
                }
                //}
            }
            //else
            //{
            //    return -3;
            //}
            //}
            //else
            //{
            //if (result.Status == false)
            //{
            //    return -1;
            //}
            //else
            //{
            //if (result.PassWord.Trim() == passWord.Trim())
            //    return 1;
            //else
            //    return -2;
            //        //}
            //    }
            //}
        }

    }

}