using Core.DTOs;
using Core.Generator;
using Core.Security;
using Core.Service.Interfaces;
using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _db;
        public UserService(MyDbContext db )
        {
            _db = db;
        }

        public bool ActiveAccount(string id)
        {
            var user = _db.Users.SingleOrDefault(u => u.ActiveCode == id);
            if(user == null)
            {
                return false;
            }

            // Now User Is Active 
            user.IsActive = true;
            user.ActiveCode = ActiveCodeGen.GenerateCode();

            // Update User And Save That
            Update(user);
            _db.SaveChanges();
            return true;

        }

        public void Add(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public User LoginUser( SignInViewModel login)
        {
            var HashPassword = PasswordHashC.EncodePasswordMd5(login.Password);
            var user = _db.Users.SingleOrDefault( u => u.Email == login.EmailOrUsername || u.Username == login.EmailOrUsername);
            if(user != null)
            {
                if(user.Password == HashPassword)
                {
                    return user;
                }
            }

            return null;
        }

        public bool IsEmail(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public bool IsUsername(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
        }

        public User GetUserByEmail(ForgotPasswordVm forgotPassword)
        {
            return _db.Users.SingleOrDefault(u => u.Email == forgotPassword.Email);
        }

        public User GetUserByActiveCode(ResetPasswordVm reset)
        {
            return _db.Users.SingleOrDefault(u => u.ActiveCode == reset.ActiveCode);
        }

        public UserInformationsVm GetUserInformations(string username)
        {
            User user = GetuserByUserName(username);
            UserInformationsVm userInfo = new UserInformationsVm()
            {
                UserName = user.Username,
                Email = user.Email,
                RegisterDate = user.Created,
                Wallet = 0 ,
            };
            return userInfo;
        }

        public User GetuserByUserName(string username)
        {
            return _db.Users.SingleOrDefault( u => u.Username == username );
        }
    }
}
