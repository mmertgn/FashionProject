using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class Md5HashProvider : IEncryptor
    {
        public string Hash(string plainText)
        {
            var myprovider = new MD5CryptoServiceProvider();

            var data = myprovider.ComputeHash(Encoding.UTF8.GetBytes(plainText));

            var s = new StringBuilder();

            foreach (var item in data)
            {
                s.Append(item.ToString("X2"));
            }

            return s.ToString();
        }
    }
}
