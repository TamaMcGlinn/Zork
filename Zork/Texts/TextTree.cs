using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            string text = "";
            string path = "../../../data/story/" + filename;
            if (!File.Exists(path))
            {
                _rootNode = null;
                return;
            }
            text = System.IO.File.ReadAllText(path);
            var lines = text.Split('\n');
            Name = lines[0];
            _rootNode = readNode(lines, 1);
        }

        private static int countInitialTabs(string line)
        {
            int i = 0;
            while (line[i++] == '\t') ;
            return i-1;
        }

        private static List<string> getRequiredConditions(ref string inputLine)
        {
            int start = inputLine.IndexOf('[');
            if(start == 0)
            {
                int end = inputLine.IndexOf(']');
                var result = ReadConditions(inputLine, start, end);
                inputLine = inputLine.Substring(end + 1);
                return result;
            }
            return new List<string>();
        }

        private static List<string> getUnlockedClues(ref string inputLine)
        {
            int start = inputLine.IndexOf('[');
            if (start >= 0)
            {
                int end = inputLine.IndexOf(']');
                var result = ReadConditions(inputLine, start, end);
                inputLine = inputLine.Substring(0, start);
                return result;
            }
            return new List<string>();
        }

        private static List<string> ReadConditions(string inputLine, int start, int end)
        {
            Debug.Assert(start >= 0);
            Debug.Assert(end > start);
            string conditions = inputLine.Substring(start + 1, end - start - 1);
            return conditions.Split(',').ToList();
        }

        private static Node readNode(string[] lines, int line)
        {
            if (line >= lines.Count())
            {
                return null;
            }
            int tabs = countInitialTabs(lines[line]);
            string contents = lines[line].Substring(tabs + 1);
            var requiredConditions = getRequiredConditions(ref contents);
            var unlockedClues = getUnlockedClues(ref contents);
            Node currentNode = new Node(contents, requiredConditions, unlockedClues);
            List<int> childBeginLines = GetChildren(lines, line, tabs);
            foreach (int beginLine in childBeginLines)
            {
                currentNode.Children.Add(readNode(lines, beginLine));
            }
            return currentNode;
        }

        private static List<int> GetChildren(string[] lines, int line, int tabs)
        {
            List<int> childBeginLines = new List<int>();
            for (int endline = line + 1; endline != lines.Count(); ++endline)
            {
                int childTabs = countInitialTabs(lines[endline]);
                if (childTabs == tabs + 1)
                {
                    childBeginLines.Add(endline);
                }
                if (childTabs < tabs + 1)
                {
                    break;
                }
            }
            return childBeginLines;
        }
    }
}
