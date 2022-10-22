using FBQ.Salud_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FBQ.Salud_Domain.Queries
{
    public interface IAdminQuery
    {
        User GetAdminById(string AdminId);
    }
}
