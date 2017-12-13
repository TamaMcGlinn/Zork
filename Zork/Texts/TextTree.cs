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

        public List<Node> RootNodes;

        public TextTree(string filename)
        {
            string text = "";
            string path = "../../../data/story/" + filename;
            if (!File.Exists(path))
            {
                RootNodes = null;
                return;
            }
            text = System.IO.File.ReadAllText(path);
            var lines = text.Split('\n');
            Name = lines[0];
            RootNodes = ReadNodes(lines, 1);
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

        /// <summary>
        /// Returns a list of Nodes read from lines starting at the given line
        /// with child Nodes below each all the way to the leafnodes
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private static List<Node> ReadNodes(string[] lines, int line)
        {
            if (line >= lines.Count())
            {
                return null;
            }
            List<Node> results = new List<Node>();
            int tabs = countInitialTabs(lines[line]);
            List<int> beginLines = GetChildren(lines, line, tabs);
            foreach(int beginLine in beginLines)
            {
                string contents = lines[beginLine].Substring(tabs + 1);
                var requiredConditions = getRequiredConditions(ref contents);
                var unlockedClues = getUnlockedClues(ref contents);
                Node currentNode = new Node(contents, requiredConditions, unlockedClues);
                int nextLine = beginLine + 1;
                if (nextLine < lines.Count() && countInitialTabs(lines[nextLine]) == tabs+1)
                {
                    currentNode.Children.AddRange(ReadNodes(lines, nextLine));
                }
                results.Add(currentNode);
            }
            return results;
        }

        /// <summary>
        /// Returns a list of line numbers which start with the specified number of tabs.
        /// Starts at the given line, ends when a line is encountered with less tabs than given.
        /// </summary>
        /// <param name="lines">All text lines</param>
        /// <param name="line">Line number to start at</param>
        /// <param name="tabs">Number of tabs to search for</param>
        /// <returns></returns>
        private static List<int> GetChildren(string[] lines, int line, int tabs)
        {
            List<int> childBeginLines = new List<int>();
            for (; line != lines.Count(); ++line)
            {
                int childTabs = countInitialTabs(lines[line]);
                if (childTabs == tabs)
                {
                    childBeginLines.Add(line);
                }
                if (childTabs < tabs)
                {
                    break;
                }
            }
            return childBeginLines;
        }
    }
}
