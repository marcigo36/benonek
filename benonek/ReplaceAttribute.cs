using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace benonek
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    class ReplaceAttribute : Attribute
    {
        public ReplaceAttribute(string Old, string New)
        {
            this.Old = Old;
            this.New = New;
        }

        public string Old { get; set; }

        public string New { get; set; }
    }
}
