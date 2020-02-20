using System;
using System.Collections.Generic;

namespace Blog.Dto
{
    /// <summary>
    /// 导航菜单树
    /// </summary>
    public class NavigationBarDto
    {
        public int id { get; set; }
        public int pid { get; set; }
        public int order { get; set; }
        public string name { get; set; }
        public bool IsHide { get; set; } = false;
        public bool IsButton { get; set; } = false;
        public string path { get; set; }
        public string Func { get; set; }
        public string iconCls { get; set; }
        public NavigationBarMetaDto meta { get; set; } = new NavigationBarMetaDto();
        public List<NavigationBarDto> children { get; set; } = new List<NavigationBarDto>();
    }
    public class NavigationBarMetaDto
    {
        public string title { get; set; }
        public bool requireAuth { get; set; } = true;
        public bool NoTabPage { get; set; } = false;


    }
}
