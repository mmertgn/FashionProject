using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore_BLL.Services.Abstracts;

namespace FashionStore_BLL.Services.Concretes
{
    public class CategoryPictureService : IPictureService
    {
        public string GetDefaultImage()
        {
            return "defaultcategory.png";
        }
    }
}
