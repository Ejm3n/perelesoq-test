using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Domain
{
    public static class Logic
    {
        public static readonly Func<IEnumerable<IElectricNode>, bool> And = nodes => nodes.All(n => n.HasCurrent);
        public static readonly Func<IEnumerable<IElectricNode>, bool> Or = nodes => nodes.Any(n => n.HasCurrent);
    }

}
