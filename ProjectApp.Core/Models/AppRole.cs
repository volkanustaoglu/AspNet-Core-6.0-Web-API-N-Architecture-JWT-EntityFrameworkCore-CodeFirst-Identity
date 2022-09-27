using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Core.Models
{
    public class AppRole: IdentityRole
    {
        public int RowOptions { get; set; }
    }
}
