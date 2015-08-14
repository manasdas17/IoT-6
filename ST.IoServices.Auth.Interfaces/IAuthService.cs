using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.IoServices.Auth.Interfaces
{
    public interface IAuthService
    {
        string VerifyAuthToken(string token);
    }
}
