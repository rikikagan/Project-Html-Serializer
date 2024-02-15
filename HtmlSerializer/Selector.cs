using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HtmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Children { get; set; }
        public Selector()
        { 
            Classes = new List<string>();
        }
        public static Selector Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("input cannot be null or empty");
            }
            var parts = input.Split(' ');
            Selector root = new Selector();
            Selector current=root;
            foreach (var part in parts)
            {
                var segments = part.Split(' ');
                foreach (var segment in segments)
                {
                    var newSelector = new Selector();
                    if (segment.StartsWith("#"))
                    {
                        newSelector.Id = part.Substring(1);
                    }
                    else if (segment.StartsWith("."))
                    {
                        newSelector.Classes.Add(segment.Substring(1));
                    }
                    else if (HtmlHelper.Instance.AllHtmlTags.Any(tag => segment.StartsWith(tag))|| HtmlHelper.Instance.SelfClosingTags.Any(tag => segment.StartsWith(tag)))
                    {
                        newSelector.TagName = part.Substring(0);
                    }
                    else
                    {
                        continue;
                    }
                    current.Children = newSelector;
                    newSelector.Parent = current;
                    current = newSelector;
                }
            }
            return root;
        }
        public void ToString()
        {
            StringBuilder sb = new StringBuilder();
            while(Children!=null)
            {
                if (!string.IsNullOrEmpty(Children.TagName))
                {
                    Console.WriteLine("TagName: "+ Children.TagName);
                }
                // Print ID
                if (!string.IsNullOrEmpty(Children.Id))
                {
                    Console.WriteLine("Id: "+ Children.Id);
                }
                if (Children.Classes.Count > 0)
                {
                    Console.WriteLine(" class=\"");
                    Console.WriteLine(string.Join(" ", Children.Classes));
                    Console.WriteLine("\"");
                }
                Children = Children.Children;
            }        
        
        }
    }
}
