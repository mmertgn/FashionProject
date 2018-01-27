using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore_BLL.Services.Abstracts
{
    public interface ICookieService
    {
        void AddCookie(string cookieName, string cookieValue);
        void DeleteCookie(string userCookieName);
        string GetCookie(string cookieName);
    }
}
