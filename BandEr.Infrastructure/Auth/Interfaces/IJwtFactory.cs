using System.Collections.Generic;
using System.Threading.Tasks;
using BandEr.Infrastructure.Auth.Models;

namespace BandEr.Infrastructure.Auth.Interfaces
{
    public interface IJwtFactory
    {
        /// <summary>
        /// Generate JWT with given user id and username
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns></returns>
        Task<AccessToken> GenerateEncodedToken(string id, string userName, IEnumerable<string> roles);
    }
}
