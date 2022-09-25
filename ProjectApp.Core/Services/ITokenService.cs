using ProjectApp.Core.Configuration;
using ProjectApp.Core.DTOS.TokenDtos;
using ProjectApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(AppUser appUser);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
