using Base.Model;
using System;
using System.Runtime.Serialization;
namespace JDYD.Model
{
    /// <summary>
    /// 客户
    /// </summary>
    public class New_Customer : BaseModel
    {

        /// <summary>
        /// 关联操作员
        /// </summary>
        [DataMember]
        public int AdminUserID { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [DataMember]
        public string Phone { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        [DataMember]
        public int Type { get; set; }

        /// <summary>
        /// 客户来源
        /// </summary>
        [DataMember]
        public int Quarry { get; set; }

        /// <summary>
        /// 客户来源（人员）
        /// </summary>
        [DataMember]
        public int UserID { get; set; }

        /// <summary>
        /// 录单时显示
        /// </summary>
        [DataMember]
        public int RecordingTimeDisplay { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [DataMember]
        public int ProvinceID { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [DataMember]
        public int CityID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [DataMember]
        public string PostalCode { get; set; }

        /// <summary>
        /// 合作状态
        /// </summary>
        [DataMember]
        public int StateOfCooperation { get; set; }

        /// <summary>
        /// 结算方式
        /// </summary>
        [DataMember]
        public int SettlementMethod { get; set; }

        /// <summary>
        /// 公司业务
        /// </summary>
        [DataMember]
        public string CompanyBusiness { get; set; }

        /// <summary>
        /// 人员规模
        /// </summary>
        [DataMember]
        public int ScaleOfPersonnel { get; set; }

        /// <summary>
        /// 签约状态
        /// </summary>
        [DataMember]
        public int ContractualStatus { get; set; }

        /// <summary>
        /// 签约到期日
        /// </summary>
        [DataMember]
        public DateTime? ContractExpirationDateBegin { get; set; }

        /// <summary>
        /// 签约到期日
        /// </summary>
        [DataMember]
        public DateTime? ContractExpirationDateEnd { get; set; }

        /// <summary>
        /// 电话（区号）
        /// </summary>
        [DataMember]
        public string AreaCode { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [DataMember]
        public string Tel { get; set; }

        /// <summary>
        /// 传真区号
        /// </summary>
        [DataMember]
        public string FaxAreaCode { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [DataMember]
        public string Fax { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [DataMember]
        public string Mail { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember]
        public string Contacts { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public int Sex { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [DataMember]
        public string Position { get; set; }

        /// <summary>
        /// 联系手机
        /// </summary>
        [DataMember]
        public string ContactPphone { get; set; }

        /// <summary>
        /// 合作日期
        /// </summary>
        [DataMember]
        public DateTime? DateOfCooperation { get; set; }

        /// <summary>
        /// 最后预订
        /// </summary>
        [DataMember]
        public DateTime? FinalReservation { get; set; }

        /// <summary>
        /// 预订状态
        /// </summary>
        [DataMember]
        public int ReservationStatus { get; set; }

        /// <summary>
        /// 客户来源（部门）
        /// </summary>
        [DataMember]
        public int DpMentID { get; set; }
    }
}