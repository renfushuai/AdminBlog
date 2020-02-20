using System;
namespace BlogAdmin.AuthHelper
{
    public class PermissionItem
    {
        /// <summary>
        /// 用户或角色
        /// </summary>
       public virtual string Role { get; set; }
        /// <summary>
        /// 请求的Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
