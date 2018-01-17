using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class CustomerPictureService : ICustomerPictureService
    {
        public string GetDefaultImage()
        {
            return "defaultprofil.jpg";
        }
    }
}
