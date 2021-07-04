using System.Threading.Tasks;
using api.Entities;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateTokenAsync(User user);
    }
}