using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Authentication
{
    public class ApplicationRole
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string NormalizedName { get; set; }
    }
}
