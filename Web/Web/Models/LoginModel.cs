using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    /// <summary>
    /// 用户登录模型
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "帐号不能为空")]
        [StringLength(50)]
        public string Account { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(50)]
        public string Password { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码不能为空")]
        public string ValidCode { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
       
    }
}