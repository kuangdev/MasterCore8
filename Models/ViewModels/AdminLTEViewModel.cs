using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MasterCore8.Models.ViewModels
{    
    public class SideItem
    {
        public string Header { get; set; }
        public string Can { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Target { get; set; }
        public string Class { get; set; }
        public string Onclick { get; set; }
        public string Align { get; set; }
        public string Active { get; set; }
        public BadgeStyle Badge { get; set; }

        public List<SideItem> SubMenu {
            get ; set;
        }
    }
    public class BadgeStyle{
        public string color { get; set; }
        public string text { get; set; }
    }

    public class Breadcrumb{
        public string Can { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Target { get; set; }
        public bool Active { get; set; }
    }

}