using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zork.Characters;

namespace Zork.Extensions
{
    public static class ListNPCExtensions
    {
        public static NPC FindNPC(this List<NPC> list, string name)
        {
            return list.Find(item => item.Name == name);
        }
    }
}
