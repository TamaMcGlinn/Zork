using System.Collections.Generic;

namespace Zork.Texts
{
    /// <summary>
    /// Each node has a text and some number of child nodes.
    /// </summary>
    public class Node
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private List<Node> _children;
        private List<string> _conditions;

        public List<Node> Children
        {
            get { return _children; }
        }

        public Node(string text, List<string> conditions)
        {
            Text = text;
            _children = new List<Node>();
            _conditions = conditions;
        }

    }
}
