using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LuceneNetDemo.Models
{
    public enum EnumUserStatus
    {
        [Description("已删除")]
        Deleted = 0,
        [Description("有效")]
        Effective = 1,
        [Description("未审核")]
        UnAudit = 2,
        [Description("已修改")]
        Updated = 3,
        [Description("无效")]
        Invalid = 4
    }

    public enum EnumCompanyDimensions
    {
        LessThen50 = 0,
        From50To99 = 1,
        From100To499 = 2,
        From500To999 = 3,
        From1000To4999 = 4,
        From5000To9999 = 5,
        From10000ToMore = 6
    }

    /// <summary>
    /// 工控站点枚举
    /// </summary>
    public enum EnumGKSite
    {
        [Description("用户站点")]
        User = 0,
        [Description("工控MRO")]
        GongkongMRO = 1,
        [Description("学工控 EDU")]
        XueGongkong = 2,
        [Description("工控TV")]
        GongkongTV = 3,
        [Description("IGongkong")]
        IGongkong = 4,
        [Description("工控人才")]
        GongkongHR = 5,
        [Description("工控网主站")]
        Gongkong = 6,
        [Description("工控网论坛")]
        BBS = 7,
        [Description("工控消息")]
        Message = 8,
        [Description("工控军团")]
        Corp = 9,
        [Description("工控APP")]
        GongkongAPP = 10,
        [Description("工控猫")]
        GongkongMall = 11,
        [Description("速派APP")]
        BPOAPP = 12,
        [Description("IAAT")]
        IAAT = 13,
        [Description("摄影大赛")]
        Sheying = 14,
        [Description("在线研讨会")]
        Online = 15,
        [Description("BPO网站")]
        GongkongBPO = 16,
        [Description("展会")]
        Exhibition = 17,
        [Description("工控社区")]
        QGongkong = 18,
        [Description("其他")]
        Other = 99
    }

    public enum EnumUserLevel
    {
        [Description("未认证")]
        UnAudit = 0,
        [Description("已认证")]
        Audited = 1,
        [Description("VIP")]
        VIP = 2
    }

    public enum EnumUserApply
    {
        [Description("未申请")]
        NoApply = 0,
        [Description("申请中")]
        GetApply = 1,
        [Description("未通过")]
        DelApply = 2,
        [Description("通过申请")]
        PostApply = 3
    }

    public enum BpoUserType
    {
        /// <summary>
        /// </summary>
        [Description("--")]
        Defalut = 0,

        /// <summary>
        ///  普通个人
        /// </summary>
        [Description("普通个人")]
        BpoUser = 1,

        /// <summary>
        ///  工程师
        /// </summary>
        [Description("工程师")]
        UserEngineer = 2

    }

    /// <summary>
    /// 认证用户表
    /// </summary>
    public class RegUserCurrentEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否绑定邮箱
        /// </summary>
        public virtual bool? IsEMailValidation { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Handset { get; set; }

        /// <summary>
        /// 是否绑定手机
        /// </summary>
        public virtual bool? IsHandsetValidation { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// 公司规模
        /// </summary>
        public int CompanyDimensions { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 地址全称
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        /// 工作年限
        /// </summary>
        public int? WorkDate { get; set; }

        /// <summary>
        /// 单位性质
        /// </summary>
        public int? CompanyTypeId { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public int? JobTypeId { get; set; }
        /// <summary>
        /// 职位名
        /// </summary>
        public string JobTypeName { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 是否被禁言
        /// </summary>
        public bool? IsBanedTalk { get; set; }

        /// <summary>
        /// 是否实名
        /// </summary>
        public bool? IsAutonym { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string CardID { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// 最后登录日期
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int? LoginCount { get; set; }

        /// <summary>
        /// 注册终端
        /// </summary>
        public EnumGKSite RegFrom { get; set; }

        /// <summary>
        /// 老编号
        /// </summary>
        public string OldId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否验证名称
        /// </summary>
        public bool? IsTrueNameValidation { get; set; }

        /// <summary>
        /// 是否验证电话
        /// </summary>
        public bool? IsPhoneValidation { get; set; }

        /// <summary>
        /// 是否验证传真
        /// </summary>
        public bool? IsFaxValidation { get; set; }

        /// <summary>
        /// 是否验证地址
        /// </summary>
        public bool? IsAddressValidation { get; set; }

        /// <summary>
        /// 是否验证身份证
        /// </summary>
        public bool? IsCardIdValidation { get; set; }

        public EnumUserLevel Level { get; set; }

        /// <summary>
        /// 禁言结束时间
        /// </summary>
        public DateTime? BanedTime { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool? IsLogOn { get; set; }

        /// <summary>
        /// 申请VIP状态
        /// </summary>
        public EnumUserApply ApplyVIP { get; set; }

        /// <summary>
        /// VIP申请时间
        /// </summary>
        public DateTime? VIPUpdateTime { get; set; }

        /// <summary>
        /// 是否提醒
        /// </summary>
        public bool? IsRemind { get; set; }

        /// <summary>
        /// 提醒时间
        /// </summary>
        public DateTime? RemindTime { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string ShowPassword { get; set; }
        /// <summary>
        /// ????
        /// </summary>
        public string Corps { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public virtual int CountyId { get; set; }
        /// <summary>
        /// 是否专家
        /// </summary>
        public bool IsExpert { get; set; }
        /// <summary>
        /// 申请专家状态
        /// </summary>
        public EnumUserApply ApplyExpert { get; set; }
        /// <summary>
        /// 申请专家日期
        /// </summary>
        public DateTime? ExpertUpdateTime { get; set; }
        /// <summary>
        /// 是否在线讲师
        /// </summary>
        public bool IsTeacher { get; set; }
        /// <summary>
        /// 申请讲师状态
        /// </summary>
        public EnumUserApply ApplyTeacher { get; set; }
        /// <summary>
        /// 申请讲师日期
        /// </summary>
        public DateTime? TeacherUpdateTime { get; set; }
        /// <summary>
        /// 是否工程师
        /// </summary>
        public bool IsBPOEngineer { get; set; }
        /// <summary>
        /// 申请BPO工程师状态
        /// </summary>
        public EnumUserApply ApplyBPOEngineer { get; set; }
        /// <summary>
        /// 申请BPO工程师日期
        /// </summary>
        public DateTime? BPOEngineerUpdateTime { get; set; }

        /// <summary>
        /// 微博账号
        /// </summary>
        public string BlogAccount { get; set; }

        /// <summary>
        /// 微信账号
        /// </summary>
        public string WeixinAccount { get; set; }

        /// <summary>
        /// 微博二维码路径
        /// </summary>
        public string BlogQRCodeUrl { get; set; }

        /// <summary>
        /// 微信二维码路径
        /// </summary>
        public string WeixinQRCodeUrl { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? BirthDay { get; set; }

        /// <summary>
        /// qq号
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 身份正面照片
        /// </summary>
        public string CardPic { get; set; }
        /// <summary>
        /// 身份证反面照片
        /// </summary>
        public string CardOppositePic { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string LoginFrom { get; set; }
        /// <summary>
        /// 微信Id
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        ///  纬度
        /// </summary>
        public decimal Lat { get; set; }

        /// <summary>
        ///  经度
        /// </summary>
        public decimal Lng { get; set; }

        /// <summary>
        /// 微信公众号openid
        /// </summary>
        public string WxOfficialAccOpenId { get; set; }

        /// <summary>
        /// 微信培训小程序openid
        /// </summary>
        public string SMOpenId { get; set; }

        /// <summary>
        /// 实名状态
        /// </summary>
        public bool? RealNameStatus { get; set; }

        /// <summary>
        ///  公司省ID
        /// </summary>
        public int CompanyProvinceId { get; set; }

        /// <summary>
        ///  公司市ID
        /// </summary>
        public int CompanyCityId { get; set; }

        /// <summary>
        ///  公司纬度
        /// </summary>
        public string CompanyLat { get; set; }

        /// <summary>
        ///  公司经度
        /// </summary>
        public string CompanyLng { get; set; }

        /// <summary>
        ///  公司地图选择地址.
        /// </summary>
        public string CompanyFullAddress { get; set; }

        /// <summary>
        ///  公司门牌号
        /// </summary>
        public string CompanyHouseNumber { get; set; }

        /// <summary>
        ///  苹果用户Code.
        /// </summary>
        public string AppleUserCode { get; set; }

    }

}