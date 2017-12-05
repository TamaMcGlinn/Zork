using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    interface IObject
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}
