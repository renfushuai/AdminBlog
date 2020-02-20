using System;
namespace Blog.Model
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class ApiResponse
    {
        public static ApiResponseModel<bool> Success()        {            return new ApiResponseModel<bool> { success = true, msg = ""};        }        public static ApiResponseModel<Tdata> Success<Tdata>()        {            return new ApiResponseModel<Tdata> { success=true, msg = "",data=default(Tdata) };        }        public static ApiResponseModel<Tdata> Success<Tdata>(Tdata data)        {            return new ApiResponseModel<Tdata> { success=true, msg = "", data = data };        }        public static ApiResponseModel<string> Error(string msg)        {            return new ApiResponseModel<string> { success = false, msg = msg, data = null };        }        public static ApiResponseModel<Tdata> Error<Tdata>(string msg)        {            return new ApiResponseModel<Tdata> { success=false, msg = msg,data= default(Tdata) };        }        public static ApiResponseModel<Tdata> Error<Tdata>(string msg, Tdata data)        {            return new ApiResponseModel<Tdata> { success=false, msg= msg, data = data };        }
    }
}
