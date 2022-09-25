using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Repository.Configurations
{
    public class IdentityOption : IdentityOptions
    {
        public static void IdentityPasswordOption(IdentityOptions builder)
        {
            builder.Password.RequiredLength = 4;
            builder.Password.RequireNonAlphanumeric = false;
            builder.Password.RequireUppercase = false;
            builder.Password.RequireLowercase = false;
            builder.Password.RequireDigit = false;

            builder.User.RequireUniqueEmail = true;
            builder.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }

    }
}
