﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionStore_BLL.Services.Abstracts
{
    public interface ISeoUrlMaker
    {
        string MakeSlug(string input);
    }
}
