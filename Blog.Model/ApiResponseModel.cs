using System;
namespace Blog.Model
{
    public class ApiResponseModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T data { get; set; }
    }
}
