using System;
namespace Blog.Model.Dto
{
    public class JwtTokenDto
    {
                public string token { get; set; }
                public double expires_in { get; set; }
                public string token_type { get; set; }
    }
}
