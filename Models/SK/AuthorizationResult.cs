using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models.SK
{
    public class AuthorizationResult
    {
        public int TimeToLive { get; set; }

        public string Token { get; set; }
    }
}
