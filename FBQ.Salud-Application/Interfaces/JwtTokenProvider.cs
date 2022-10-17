using FBQ.Salud_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Application.Interfaces
{
    public interface IJwtTokenProvider
    {
        public Task<string> CreateJwtToken(User user);
    }
}
