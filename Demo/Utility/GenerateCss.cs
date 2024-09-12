using Demo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Demo.Utility
{
    public class GenerateCss
    {
        public static void GenerateCssFile(int domain)
        {
            string jsonData = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/Mock/{domain}.json");
            var data = JsonConvert.DeserializeObject<List<ColorCustomization>>(jsonData);
            var cssTree = new TreeBuilder().BuildTree(data);

            var cssBuilder = new StringBuilder();

            foreach (var item in cssTree)
            {
                cssBuilder.AppendLine($".{item.CssName} {{");
                foreach (var cssProp in item.Children)
                {
                    cssBuilder.AppendLine($"    {cssProp.CssName}: {cssProp.CssValue};");
                }
                cssBuilder.AppendLine("}");
            }

            var css = cssBuilder.ToString();
            using (var writer = new StreamWriter(Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}/Styles/CSS", $"Style_{domain}.css")))
            {
                writer.Write(css);
            }
        }

        public static void GenerateLessFile(int domain)
        {
            var data = new List<ColorCustomization>()
            {
                new ColorCustomization()
                {
                    Id = 1,
                    CssName = "sa-login-panel:before",
                    CssType = (int)CssType.Class,
                },
                new ColorCustomization()
                {
                    Id = 2,
                    CssName = "background",
                    CssType= (int)CssType.Property,
                    CssValue = "linear-gradient(to right, #e669a2 0%, #db2e75 34%, #99ca3f 78%, #93eb3e 100%)",
                    ValueType = (int)CssValueType.Gradient,
                    ParentId = 1
                },
                new ColorCustomization()
                {
                    Id=3,
                    CssName = "backgrounColor",
                    CssType = (int)CssType.Variable,
                    CssValue = "#007dbc",
                    ValueType = (int)CssValueType.Gradient,
                    ParentId = 4,
                },
                new ColorCustomization()
                {
                    Id = 4,
                    CssName = "sa-btn",
                    CssType= (int)CssType.Class,
                },
                new ColorCustomization()
                {
                    Id = 5,
                    CssName = "border-color",
                    CssType= (int)CssType.Property,
                    ParentId = 4
                }
            };

            var lessCssBuilder = new StringBuilder();
            var lessCssTree = new TreeBuilder().BuildTree(data);

            var variableDictionary = new Dictionary<int, string>();

            foreach (var variable in data.Where(x => x.CssType == (int)CssType.Variable))
            {
                lessCssBuilder.AppendLine($"@{variable.CssName}: {variable.CssValue};");
                variableDictionary.Add(variable.ParentId.Value, variable.CssName);
            }

            foreach (var item in lessCssTree.Where(x => x.CssType != (int)CssType.Variable))
            {
                lessCssBuilder.AppendLine($".{item.CssName} {{");
                foreach (var property in item.Children)
                {
                    if (property.CssType == (int)CssType.Variable)
                    {
                        continue;
                    }
                    lessCssBuilder.AppendLine($"    {property.CssName}: {(!string.IsNullOrWhiteSpace(property.CssValue) ? property.CssValue : $"@{variableDictionary.FirstOrDefault(x => x.Key == item.Id).Value}")};");
                }
                lessCssBuilder.AppendLine("}");
            }

            var lessCss = lessCssBuilder.ToString();
            using (var writer = new StreamWriter(Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}/Styles/LESS", $"LessStyle_{domain}.less")))
            {
                writer.Write(lessCss);
            }
        }
    }

    public class TreeBuilder
    {
        public List<ColorCustomization> BuildTree(List<ColorCustomization> nodes, int? parentId = null)
        {
            return nodes
           .Where(n => n.ParentId == parentId)
           .Select(n => new ColorCustomization
           {
               Id = n.Id,
               ParentId = n.ParentId,
               FieldName = n.FieldName,
               CssName = n.CssName,
               CssType = n.CssType,
               CssValue = n.CssValue,
               CompanyId = n.CompanyId,
               DomainName = n.DomainName,
               NumberOfCssValue = n.NumberOfCssValue,
               ValueType = n.ValueType,
               Children = BuildTree(nodes, n.Id)
           })
           .ToList();
        }
    }
}