using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FashionStore_BLL.Services.Abstracts
{
  public  interface IUploadService
  {
      string Upload(HttpPostedFileBase file,int width,int height);
  }
}
