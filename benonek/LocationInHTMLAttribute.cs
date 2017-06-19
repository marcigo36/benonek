using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benonek
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    class LocationInHTMLAttribute : Attribute
    {
        public enum ContentType { Text, HTML };
        public LocationInHTMLAttribute(string XPath, ContentType Type = ContentType.Text)
        {
            this.XPath = XPath;
            this.Type = Type;
        }

        public string XPath { get; set; }
        public ContentType Type { get; set; }
    }
}
