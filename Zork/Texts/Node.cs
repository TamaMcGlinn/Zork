using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zork.Texts
{
    class Node
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private List<Node> _children;

        public List<Node> Children
        {
            get { return _children; }
        }

        public Node(string text)
        {
            Text = text;
            _children = new List<Node>();
        }

    }
}
