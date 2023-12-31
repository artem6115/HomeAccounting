using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class UserContext
    {
        private readonly static AsyncLocal<UserContext> CurrentUser = new();
        private UserContext() { }
        private string _userId;
        public static string? UserId
        {
            get => CurrentUser.Value?._userId;
            set
            {
                CurrentUser.Value ??= new UserContext();
                CurrentUser.Value._userId = value;
            }
        }


    }
}
