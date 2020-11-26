using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuceneNetDemo.Models
{
    /// <summary>
    /// 大客户工单管理实体类
    /// </summary>
    public class Bpo_JobEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否可用 0否，1不可用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 工单名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 工单名称拼音
        /// </summary>
        public string TitlePinYin { get; set; }
        /// <summary>
        /// 公司ID（发单方）
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// 公司名称（发单方）
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string JobOrderCode { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string JobPassword { get; set; }
        /// <summary>
        /// 服务描述
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Intro { get; set; }
        /// <summary>
        /// 工单状态-1级
        /// </summary>
        public int JobStatus { get; set; }
        /// <summary>
        /// 最小预算范围
        /// </summary>
        public decimal MinPrice { get; set; }
        /// <summary>
        /// 最大预算范围
        /// </summary>
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// 添加金额???
        /// </summary>
        public decimal AddPrice { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal GkWard { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal GkPropor { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsAllowGoAdd { get; set; }
        /// <summary>
        /// 发票类型
        /// </summary>
        public int InvoiceType { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public int TradeType { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string CycleDate { get; set; }
        /// <summary>
        /// 投标结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 发单时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 发布时间（项目型工单审核通过后时间重置）
        /// </summary>
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public int Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public int City { get; set; }
        /// <summary>
        /// 县
        /// </summary>
        public int Xian { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 查看工程师详情次数
        /// </summary>
        public int Hit { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int XingNum { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int ServiceObject { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int KillNum { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsGkCommend { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsCusCommend { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool FromIsAgree { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool ToIsAgree { get; set; }
        /// <summary>
        /// 最终价格
        /// </summary>
        public decimal LastPrice { get; set; }
        /// <summary>
        /// 现场联系人
        /// </summary>
        public string JobLinkMan { get; set; }

        /// <summary>
        /// 现场联系人电话
        /// </summary>
        public string JobHandSet { get; set; }
        /// <summary>
        /// 执行时间-开始
        /// </summary>
        public DateTime? ExecStartDate { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public DateTime DaoDate { get; set; }

        /// <summary>
        /// 执行时间-结束
        /// </summary>
        public DateTime? ExecEndDate { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int Guarantee { get; set; }

        /// <summary>
        /// 工具备件
        /// </summary>
        public string JobTool { get; set; }
        /// <summary>
        /// 支付方式???
        /// </summary>
        public int PayModel { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int TradingPattern { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Oldid { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string FaPiaoTitle { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int Bpo_FaPiao { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int CommentNum { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 团队ID???
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int TeamType { get; set; }
        /// <summary>
        /// 是否确认支付
        /// </summary>
        public bool IsConfirmPay { get; set; }
        /// <summary>
        /// job来源
        /// </summary>
        public int JobType { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int JobAddressID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public DateTime KillTime { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public DateTime CompleteTime { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsOrder { get; set; }
        /// <summary>
        /// 服务报告
        /// </summary>
        public string ServiceReports { get; set; }
        /// <summary>
        /// 是否是受邀请工单 0否 1是
        /// </summary>
        public bool IsInvitation { get; set; }

        /// <summary>
        /// 受邀请企业ID
        /// </summary>
        public int InvitationCompanyId { get; set; }
        /// <summary>
        /// 受邀请企业name
        /// </summary>
        public string InvitationCompanyName { get; set; }
        /// <summary>
        /// 是否释放的单子
        /// </summary>
        public bool IsFree { get; set; }
        /// <summary>
        /// 工单状态-2级
        /// </summary>
        public int InvitationJobStatus { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsUrgent { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string JobLinkMail { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsNeedProtocol { get; set; }
        /// <summary>
        /// 发票状态
        /// </summary>
        public int BpoFaPiaoStatus { get; set; }
        /// <summary>
        /// 流标原因(1找不到承接人：时间紧；2找不到承接人：预算太低；3找不到承接人：技术要求高，现有资源不足；4找不到承接人：地区偏远，现有资源不足；5发活方取消： 时间紧，自己找到人；6发活方取消： 报价太高；7发活方取消： 项目取消；8发活方取消： 线下对接；9发活方发活信息模糊，无法对接；10速派未及时跟进；11不会使用速派平台；12签合同或开票方式有问题；13未知;14即将/已过期投标;)
        /// </summary>
        public int FailureKillType { get; set; }
        /// <summary>
        /// 流标关闭时间
        /// </summary>
        public DateTime FailureKillDate { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsNeedProtocol_SendJob { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsFromPc { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int EnterpriseJobStatus { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int EnterpriseUserJobStatus { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string FaPiaoNumber { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string ExpressNum { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string ExpressName { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string OperationPerson { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsNeedInvoice { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int OperateCompanyId { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal Lng { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int ES_JobStatus { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int ImportantGrade { get; set; }
        /// <summary>
        /// 工程师资质要求
        /// </summary>
        public string Aptitude { get; set; }
        /// <summary>
        /// 故障现象
        /// </summary>
        public string Symptom { get; set; }
        /// <summary>
        /// 预判故障
        /// </summary>
        public string PrejudgeFault { get; set; }
        /// <summary>
        /// 解决方案
        /// </summary>
        public string Solution { get; set; }
        /// <summary>
        /// 是否西门子
        /// </summary>
        public bool IsSiemens { get; set; }
        /// <summary>
        /// 内部工单号
        /// </summary>
        public string SiemensTicketNumber { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int SiemensExpertsId { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string LastUserCompany { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string PostCode { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public string SiemensProductCatetgory { get; set; }
        /// <summary>
        /// 接单方给速派开票id
        /// </summary>
        public int DistributionInvoiceID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool Vistrues { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal TaxPoint { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal ReceivableMoney { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int BrowseCount { get; set; }
        /// <summary>
        /// 辅料费用
        /// </summary>
        public decimal MaterialAmount { get; set; }
        /// <summary>
        /// 所需工程师数量
        /// </summary>
        public int? EngineerCount { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public int DistributionMaterialInvoiceID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal MaterialTaxPoint { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public decimal ReceivableMaterialMoney { get; set; }
        /// <summary>
        /// 其他费用
        /// </summary>
        public decimal OtherAmount { get; set; }
        /// <summary>
        /// 第二联系人
        /// </summary>
        public string SecondLinkMan { get; set; }

        /// <summary>
        /// 第二联系人电话
        /// </summary>
        public string SecondHandSet { get; set; }

        /// <summary>
        /// 故障设备数量
        /// </summary>
        public int? DeviceCount { get; set; }
        /// <summary>
        /// 是否项目型工单
        /// </summary>
        public int IsProjectJob { get; set; }
        /// <summary>
        /// 是否是新版大客户的活儿，用来判断新旧版活儿来对旧版大客户活儿做兼容
        /// </summary>
        public bool IsNewVersion { get; set; }
        /// <summary>
        /// 项目工单发单总价
        /// </summary>
        public decimal JobTotalAmount { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string JobNo { get; set; }
        /// <summary>
        /// 速派给发单方开票id
        /// </summary>
        public int BillToOtherInvoiceId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 配置的Id, 外键关联Dis_Dispose表
        /// </summary>
        public int DisposeID { get; set; }

        /// <summary>
        /// 接单方配置表主键，外联Dis_ReceiverDispose
        /// </summary>
        public int RDisposeID { get; set; }
        /// <summary>
        /// 是否导入的
        /// </summary>
        public bool IsImport { get; set; }
        /// <summary>
        /// 导入id
        /// </summary>
        public bool ImportID { get; set; }
        /// <summary>
        /// ???
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 时间单位：
        /// 0. 未定义
        /// 1、小时
        /// 2. 天，
        /// </summary>
        public int TimeUnit { get; set; }
        /// <summary>
        /// 产品领域id
        /// </summary>
        public int ProductCategoryLv1 { get; set; }

        /// <summary>
        ///  地图选择地址.
        /// </summary>
        public string FullAddress { get; set; }

        /// <summary>
        ///  取消原因ID
        /// </summary>
        public int CancelReasonId { get; set; }

        /// <summary>
        ///  代发工单ID
        /// </summary>
        public int ReplaceSendJobId { get; set; }
        /// <summary>
        /// 环信群id
        /// </summary>
        public string EasemobGroupId { get; set; }

    }
}