using System;

namespace Blog.Common
{
    /// <summary>
    /// 工作单元 只能作用在方法上
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}
