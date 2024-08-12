using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Models
{
    public class ColorCustomization
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string CssName { get; set; }
        public int CssType { get; set; }
        public string CssValue { get; set; }
        public int? ValueType { get; set; }
        public int NumberOfCssValue { get; set; }
        public int? ParentId { get; set; }
        public int CompanyId { get; set; }
        public string DomainName { get; set; }
        public List<ColorCustomization> Children { get; set; } = new List<ColorCustomization>();
    }

    public enum CssType
    {
        Class = 1,
        Property = 2,
        Id = 3,
        Variable = 4
    }

    public enum CssValueType
    {
        Gradient = 1,
        Solid = 2
    }
}