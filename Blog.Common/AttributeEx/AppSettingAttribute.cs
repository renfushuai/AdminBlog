using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Common.AttributeEx
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AppSettingAttribute : Attribute
    {
        /// <summary>
        /// 如果没有值，映射根节点
        /// </summary>
        public string SectionKey { get; set; }

        public AppSettingAttribute(string sectionKey)
        {
            SectionKey = sectionKey;
        }

        public AppSettingAttribute()
        {
        }
    }
}
