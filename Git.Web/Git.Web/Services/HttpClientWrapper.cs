using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Git.Web.Services
{
    public interface IHttpClient
    {
        void Get(object searchUrl, string v);
    }

}
