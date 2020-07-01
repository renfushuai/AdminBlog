using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Blog.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDto
    {
        [Required(ErrorMessage = "姓名不能为空"), MaxLength(10, ErrorMessage = "名字太长啦"), MinLength(0, ErrorMessage = "名字太短")]
        [RegularExpression(@"^[\u4e00-\u9fa5]+$", ErrorMessage = "姓名必须是中文")]
        public string Name { get; set; }

        [Required(ErrorMessage = "手机号码不能为空"), RegularExpression(@"^\+?\d{0,4}?[1][3-8]\d{9}$", ErrorMessage = "手机号码格式错误")]
        public string Phone { get; set; }

        [Range(1, 99, ErrorMessage = "年纪必须在0-99之间")]
        public int Age { get; set; }
        [Required(ErrorMessage ="配送地址不能为空")]
        public List<AddressItem> AdressList { get; set; }
    }
    public class AddressItem {
        [MinLength(1, ErrorMessage = "地址不能为空")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "配送人电话不能为空"), RegularExpression(@"^\+?\d{0,4}?[1][3-8]\d{9}$", ErrorMessage = "配送人电话格式错误")]
        public string Mobile { get; set; }
    }

}
