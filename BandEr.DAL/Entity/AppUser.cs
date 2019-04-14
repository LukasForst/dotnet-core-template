using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BandEr.DAL.Entity
{
    public class AppUser : IdentityUser<int>
    {
        public List<ValueEntity> Values { get; set; } = new List<ValueEntity>();
    }
}
