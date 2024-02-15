using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HtmlSerializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
        //מחזיר את כל הצאצאים
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                HtmlElement element = queue.Dequeue();
                yield return element;
                foreach (HtmlElement child in element.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        //מחזיר את כל האבות
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement ancestors = this;
            while(ancestors.Parent != null)
            {
                yield return ancestors.Parent;
                ancestors = ancestors.Parent;
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Id: " + Id);
            sb.AppendLine("Name: " + Name);

            sb.AppendLine("Attributes:");
            foreach (var attribute in Attributes)
            {
                sb.AppendLine(attribute);
            }

            sb.AppendLine("Classes:");
            foreach (var className in Classes)
            {
                sb.AppendLine(className);
            }

            sb.AppendLine("Inner HTML: " + InnerHtml);

            return sb.ToString();
        }
    }
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> FindElementsBySelector(this HtmlElement element, Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();
            FindElementsBySelector(element, selector, results);
            return results;
        }

        private static void FindElementsBySelector(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            if (selector.Children == null)
            {
                // סינון לפי קריטריונים של הסלקטור הנוכחי
                var filteredElements = element.Descendants().Where(e => Matches(e, selector));

                // הוספת התוצאות לאוסף
                results.UnionWith(filteredElements);
                return;
            }

            // חיפוש ברשימת הצאצאים
            foreach (var descendant in element.Descendants())
            {
                // קריאה ריקורסיבית עם הסלקטור הבא
                FindElementsBySelector(descendant, selector.Children, results);
            }
        }

        private static bool Matches(HtmlElement element, Selector selector)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && element.Name != selector.TagName)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
            {
                return false;
            }

            if (selector.Classes.Any() && !selector.Classes.All(c => element.Classes.Contains(c)))
            {
                return false;
            }

            return true;
        }

    }
}
