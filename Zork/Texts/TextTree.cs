using System.Collections.Generic;
using System.Linq;

namespace Zork.Texts
{
    /// <summary>
    /// A tree of Nodes with one root
    /// </summary>
    public class TextTree
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Node _rootNode;

        public Node RootNode
        {
            get { return _rootNode; }
        }


        public TextTree(string filename)
        {
            string text = System.IO.File.ReadAllText("../../../data/story/" + filename);
            var lines = text.Split('\n');
            Name = lines[0];
            _rootNode = readNode(lines, 1);
        }

        private int countInitialTabs(string line)
        {
            int i = 0;
            while (line[i++] == '\t') ;
            return i-1;
        }

        private Node readNode(string[] lines, int line)
        {
            if(line >= lines.Count())
            {
                return null;
            }
            int tabs = countInitialTabs(lines[line]);
            Node currentNode = new Node(lines[line].Substring(tabs + 1));
            List<int> childBeginLines = new List<int>();
            for (int endline = line + 1; endline != lines.Count(); ++endline)
            {
                int childTabs = countInitialTabs(lines[endline]);
                if( childTabs == tabs+1)
                {
                    childBeginLines.Add(endline);
                }
                if( childTabs < tabs+1)
                {
                    break;
                }
            }
            foreach(int beginLine in childBeginLines)
            {
                currentNode.Children.Add(readNode(lines, beginLine));
            }
            return currentNode;
        }
    }
}
