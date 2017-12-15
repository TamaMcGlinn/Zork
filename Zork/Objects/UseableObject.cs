using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Objects
{
    public abstract class UseableObject : BaseObject
    {
        public UseableObject(string name, string description) : base(name, description)
        {
        }

        public abstract void UseObject(Character c);
    }
}
