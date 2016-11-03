using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework;

namespace AbstractPlugin
{
    public abstract class AbstractPlugin : IPlugin
    {
        public string Name => "Abstract Plugin";
    }
}
