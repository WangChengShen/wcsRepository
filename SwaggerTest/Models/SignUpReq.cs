using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerTest.Models
{
    public class SignUpReq
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, ErrorMessage = "{0}长度不超过100字符")]
        public string Name { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        [Display(Name = "职业")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, ErrorMessage = "{0}长度不超过100字符")]
        public string Profession { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(11, ErrorMessage = "{0}格式错误", MinimumLength = 11)]
        public string Phone { get; set; }

        /// <summary>
        /// 报名类别，  1学习课程报名，2预约试听报名
        /// </summary>
        [Display(Name = "报名类别")]
        [Required(ErrorMessage = "{0}是必需的!")]
        [Range(1, 2, ErrorMessage = "{0}错误")]
        public int SignType { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        [Display(Name = "省份")]
        public string Province { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        [Display(Name = "城市")]
        public string City { get; set; }

        /// <summary>
        /// 客户端Ip
        /// </summary>
        [Display(Name = "客户端Ip")]
        public string ClientIp { get; set; }

        /// <summary>
        /// Pc端：1；移动端：2
        /// </summary>
        [Display(Name = "客户端类别")]
        [Range(1, 2, ErrorMessage = "{0}错误")]
        public int ClientType { get; set; }
    }
}
