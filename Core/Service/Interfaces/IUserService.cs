using Core.DTOs;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Interfaces
{
    public interface IUserService
    {
        bool IsEmail(string email);
        bool IsUsername(string username);
        void Add(User user);
        bool ActiveAccount(string id);
        void Update(User user);
        User LoginUser(SignInViewModel login);
        User GetUserByEmail(ForgotPasswordVm forgotPassword);
        User GetUserByActiveCode(ResetPasswordVm reset);
        UserInformationsVm GetUserInformations(string username);
        User GetuserByUserName(string username);
    }
}
