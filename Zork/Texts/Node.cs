using System.Collections.Generic;
using System.Linq;
using Zork.Characters;

namespace Zork.Texts
{
    /// <summary>
    /// Each node has a text and some number of child nodes.
    /// It also has a set of conditions to be satisfied in order for the entry to be visible.
    /// </summary>
    public class Node
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private List<string> _conditions;

        public List<string> UnlockedClues;

        private List<Node> _children;

        public List<Node> Children
        {
            get { return _children; }
        }

        public Node(string text, List<string> conditions, List<string> unlockedClues)
        {
            Text = text;
            _children = new List<Node>();
            _conditions = conditions;
            UnlockedClues = unlockedClues;
        }

        private bool IsAvailable(Player player)
        {
            foreach(string condition in _conditions)
            {
                if (player.Clues.Contains(condition))
                {
                    return false;
                }
            }
            return true;
        }

        public List<Node> AvailableChildren(Player player)
        {
            return Children.Where((Node n) => { return n.IsAvailable(player); }).ToList();
        }
    }
}
