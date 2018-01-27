using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class CookieService : ICookieService
    {
        public void AddCookie(string cookieName, string cookieValue)
        {
            // Cookie Ekleme
            //cookieName : Cookie adı
            //cookieValue : Cookie değeri
            HttpCookie cookie = new HttpCookie(cookieName, cookieValue);
            cookie.Secure = true;
            cookie.Expires = DateTime.Now.AddYears(1); // Süre(1 yıl)
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public void DeleteCookie(string userCookieName)
        {
            //Cookie Silme
            HttpCookie myCookie = new HttpCookie(userCookieName);
            myCookie.Expires = DateTime.Now.AddYears(-1);// Süreyi  1 sene azaltıyoruz
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }
        public string GetCookie(string cookieName)
        {
            // Cookie adıyla değeri çekme
            string cookie = HttpContext.Current.Request.Cookies[cookieName]?.Value;
            return cookie;
        }
    }
}
