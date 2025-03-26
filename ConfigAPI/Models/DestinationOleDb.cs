using System;
using System.Collections.Generic;

namespace ConfigAPI.Models;

public partial class DestinationOleDb
{
    public int? Accountcategorycode { get; set; }

    public string? Accountcategorycodename { get; set; }

    public int? Accountclassificationcode { get; set; }

    public string? Accountclassificationcodename { get; set; }

    public Guid? Accountid { get; set; }

    public string? Accountnumber { get; set; }

    public int? Accountratingcode { get; set; }

    public string? Accountratingcodename { get; set; }

    public Guid? Address1Addressid { get; set; }

    public int? Address1Addresstypecode { get; set; }

    public string? Address1Addresstypecodename { get; set; }

    public string? Address1City { get; set; }

    public string? Address1Composite { get; set; }

    public string? Address1Country { get; set; }

    public string? Address1County { get; set; }

    public string? Address1Fax { get; set; }

    public int? Address1Freighttermscode { get; set; }

    public string? Address1Freighttermscodename { get; set; }

    public double? Address1Latitude { get; set; }

    public string? Address1Line1 { get; set; }

    public string? Address1Line2 { get; set; }

    public string? Address1Line3 { get; set; }

    public double? Address1Longitude { get; set; }

    public string? Address1Name { get; set; }

    public string? Address1Postalcode { get; set; }

    public string? Address1Postofficebox { get; set; }

    public string? Address1Primarycontactname { get; set; }

    public int? Address1Shippingmethodcode { get; set; }

    public string? Address1Shippingmethodcodename { get; set; }

    public string? Address1Stateorprovince { get; set; }

    public string? Address1Telephone1 { get; set; }

    public string? Address1Telephone2 { get; set; }

    public string? Address1Telephone3 { get; set; }

    public string? Address1Upszone { get; set; }

    public int? Address1Utcoffset { get; set; }

    public Guid? Address2Addressid { get; set; }

    public int? Address2Addresstypecode { get; set; }

    public string? Address2Addresstypecodename { get; set; }

    public string? Address2City { get; set; }

    public string? Address2Composite { get; set; }

    public string? Address2Country { get; set; }

    public string? Address2County { get; set; }

    public string? Address2Fax { get; set; }

    public int? Address2Freighttermscode { get; set; }

    public string? Address2Freighttermscodename { get; set; }

    public double? Address2Latitude { get; set; }

    public string? Address2Line1 { get; set; }

    public string? Address2Line2 { get; set; }

    public string? Address2Line3 { get; set; }

    public double? Address2Longitude { get; set; }

    public string? Address2Name { get; set; }

    public string? Address2Postalcode { get; set; }

    public string? Address2Postofficebox { get; set; }

    public string? Address2Primarycontactname { get; set; }

    public int? Address2Shippingmethodcode { get; set; }

    public string? Address2Shippingmethodcodename { get; set; }

    public string? Address2Stateorprovince { get; set; }

    public string? Address2Telephone1 { get; set; }

    public string? Address2Telephone2 { get; set; }

    public string? Address2Telephone3 { get; set; }

    public string? Address2Upszone { get; set; }

    public int? Address2Utcoffset { get; set; }

    public string? AdxCreatedbyipaddress { get; set; }

    public string? AdxCreatedbyusername { get; set; }

    public string? AdxModifiedbyipaddress { get; set; }

    public string? AdxModifiedbyusername { get; set; }

    public decimal? Aging30 { get; set; }

    public decimal? Aging30Base { get; set; }

    public decimal? Aging60 { get; set; }

    public decimal? Aging60Base { get; set; }

    public decimal? Aging90 { get; set; }

    public decimal? Aging90Base { get; set; }

    public int? Businesstypecode { get; set; }

    public string? Businesstypecodename { get; set; }

    public Guid? Createdby { get; set; }

    public Guid? Createdbyexternalparty { get; set; }

    public string? Createdbyexternalpartyname { get; set; }

    public string? Createdbyexternalpartyyominame { get; set; }

    public string? Createdbyname { get; set; }

    public string? Createdbyyominame { get; set; }

    public DateTime? Createdon { get; set; }

    public Guid? Createdonbehalfby { get; set; }

    public string? Createdonbehalfbyname { get; set; }

    public string? Createdonbehalfbyyominame { get; set; }

    public decimal? Creditlimit { get; set; }

    public decimal? CreditlimitBase { get; set; }

    public bool? Creditonhold { get; set; }

    public string? Creditonholdname { get; set; }

    public int? Customersizecode { get; set; }

    public string? Customersizecodename { get; set; }

    public int? Customertypecode { get; set; }

    public string? Customertypecodename { get; set; }

    public Guid? Defaultpricelevelid { get; set; }

    public string? Defaultpricelevelidname { get; set; }

    public string? Description { get; set; }

    public bool? Donotbulkemail { get; set; }

    public string? Donotbulkemailname { get; set; }

    public bool? Donotbulkpostalmail { get; set; }

    public string? Donotbulkpostalmailname { get; set; }

    public bool? Donotemail { get; set; }

    public string? Donotemailname { get; set; }

    public bool? Donotfax { get; set; }

    public string? Donotfaxname { get; set; }

    public bool? Donotphone { get; set; }

    public string? Donotphonename { get; set; }

    public bool? Donotpostalmail { get; set; }

    public string? Donotpostalmailname { get; set; }

    public string? Donotsendmarketingmaterialname { get; set; }

    public bool? Donotsendmm { get; set; }

    public string? Emailaddress1 { get; set; }

    public string? Emailaddress2 { get; set; }

    public string? Emailaddress3 { get; set; }

    public byte[]? Entityimage { get; set; }

    public long? EntityimageTimestamp { get; set; }

    public string? EntityimageUrl { get; set; }

    public Guid? Entityimageid { get; set; }

    public decimal? Exchangerate { get; set; }

    public string? Fax { get; set; }

    public bool? Followemail { get; set; }

    public string? Followemailname { get; set; }

    public string? Ftpsiteurl { get; set; }

    public int? Importsequencenumber { get; set; }

    public int? Industrycode { get; set; }

    public string? Industrycodename { get; set; }

    public string? Isprivatename { get; set; }

    public DateTime? Lastonholdtime { get; set; }

    public DateTime? Lastusedincampaign { get; set; }

    public decimal? Marketcap { get; set; }

    public decimal? MarketcapBase { get; set; }

    public bool? Marketingonly { get; set; }

    public string? Marketingonlyname { get; set; }

    public string? Masteraccountidname { get; set; }

    public string? Masteraccountidyominame { get; set; }

    public Guid? Masterid { get; set; }

    public bool? Merged { get; set; }

    public string? Mergedname { get; set; }

    public Guid? Modifiedby { get; set; }

    public Guid? Modifiedbyexternalparty { get; set; }

    public string? Modifiedbyexternalpartyname { get; set; }

    public string? Modifiedbyexternalpartyyominame { get; set; }

    public string? Modifiedbyname { get; set; }

    public string? Modifiedbyyominame { get; set; }

    public DateTime? Modifiedon { get; set; }

    public Guid? Modifiedonbehalfby { get; set; }

    public string? Modifiedonbehalfbyname { get; set; }

    public string? Modifiedonbehalfbyyominame { get; set; }

    public Guid? MsaManagingpartnerid { get; set; }

    public string? MsaManagingpartneridname { get; set; }

    public string? MsaManagingpartneridyominame { get; set; }

    public Guid? MsdynAccountkpiid { get; set; }

    public string? MsdynAccountkpiidname { get; set; }

    public bool? MsdynGdproptout { get; set; }

    public string? MsdynGdproptoutname { get; set; }

    public int? MsdynPrimarytimezone { get; set; }

    public Guid? MsdynSalesaccelerationinsightid { get; set; }

    public string? MsdynSalesaccelerationinsightidname { get; set; }

    public string? Name { get; set; }

    public int? Numberofemployees { get; set; }

    public int? Onholdtime { get; set; }

    public int? Opendeals { get; set; }

    public DateTime? OpendealsDate { get; set; }

    public int? OpendealsState { get; set; }

    public decimal? Openrevenue { get; set; }

    public decimal? OpenrevenueBase { get; set; }

    public DateTime? OpenrevenueDate { get; set; }

    public int? OpenrevenueState { get; set; }

    public Guid? Originatingleadid { get; set; }

    public string? Originatingleadidname { get; set; }

    public string? Originatingleadidyominame { get; set; }

    public DateTime? Overriddencreatedon { get; set; }

    public Guid? Ownerid { get; set; }

    public string? Owneridname { get; set; }

    public string? Owneridtype { get; set; }

    public string? Owneridyominame { get; set; }

    public int? Ownershipcode { get; set; }

    public string? Ownershipcodename { get; set; }

    public Guid? Owningbusinessunit { get; set; }

    public string? Owningbusinessunitname { get; set; }

    public Guid? Owningteam { get; set; }

    public Guid? Owninguser { get; set; }

    public Guid? Parentaccountid { get; set; }

    public string? Parentaccountidname { get; set; }

    public string? Parentaccountidyominame { get; set; }

    public bool? Participatesinworkflow { get; set; }

    public string? Participatesinworkflowname { get; set; }

    public int? Paymenttermscode { get; set; }

    public string? Paymenttermscodename { get; set; }

    public int? Preferredappointmentdaycode { get; set; }

    public string? Preferredappointmentdaycodename { get; set; }

    public int? Preferredappointmenttimecode { get; set; }

    public string? Preferredappointmenttimecodename { get; set; }

    public int? Preferredcontactmethodcode { get; set; }

    public string? Preferredcontactmethodcodename { get; set; }

    public Guid? Preferredequipmentid { get; set; }

    public string? Preferredequipmentidname { get; set; }

    public Guid? Preferredserviceid { get; set; }

    public string? Preferredserviceidname { get; set; }

    public Guid? Preferredsystemuserid { get; set; }

    public string? Preferredsystemuseridname { get; set; }

    public string? Preferredsystemuseridyominame { get; set; }

    public Guid? Primarycontactid { get; set; }

    public string? Primarycontactidname { get; set; }

    public string? Primarycontactidyominame { get; set; }

    public string? Primarysatoriid { get; set; }

    public string? Primarytwitterid { get; set; }

    public Guid? Processid { get; set; }

    public decimal? Revenue { get; set; }

    public decimal? RevenueBase { get; set; }

    public int? Sharesoutstanding { get; set; }

    public int? Shippingmethodcode { get; set; }

    public string? Shippingmethodcodename { get; set; }

    public string? Sic { get; set; }

    public Guid? Slaid { get; set; }

    public Guid? Slainvokedid { get; set; }

    public string? Slainvokedidname { get; set; }

    public string? Slaname { get; set; }

    public Guid? Stageid { get; set; }

    public int? Statecode { get; set; }

    public string? Statecodename { get; set; }

    public int? Statuscode { get; set; }

    public string? Statuscodename { get; set; }

    public string? Stockexchange { get; set; }

    public int? Teamsfollowed { get; set; }

    public string? Telephone1 { get; set; }

    public string? Telephone2 { get; set; }

    public string? Telephone3 { get; set; }

    public int? Territorycode { get; set; }

    public string? Territorycodename { get; set; }

    public Guid? Territoryid { get; set; }

    public string? Territoryidname { get; set; }

    public string? Tickersymbol { get; set; }

    public string? Timespentbymeonemailandmeetings { get; set; }

    public int? Timezoneruleversionnumber { get; set; }

    public Guid? Transactioncurrencyid { get; set; }

    public string? Transactioncurrencyidname { get; set; }

    public string? Traversedpath { get; set; }

    public int? Utcconversiontimezonecode { get; set; }

    public long? Versionnumber { get; set; }

    public string? Websiteurl { get; set; }

    public string? Yominame { get; set; }
}
