using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Intefaces;

namespace TWatchSKDesigner.Services
{
    public static class Bootstrap
    {
        internal static void RegisterAll(IMutableDependencyResolver services)
        {
            services.Register<IEsp32ToolService>(() => new Esp32ToolService());
        }
    }
}
