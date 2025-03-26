using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConfigAPI.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ConfigurationTable> ConfigurationTables { get; set; }

    public virtual DbSet<DestinationOleDb> DestinationOleDbs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-A8G9933C;Database=Dynamics365_Extract;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.OpenRevenue)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("openRevenue");
            entity.Property(e => e.OpenRevenueBase)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("openRevenueBase");
            entity.Property(e => e.Opendeals).HasColumnName("opendeals");
            entity.Property(e => e.OriginatingLead).HasColumnName("originatingLead");
            entity.Property(e => e.PreferredEquipement).HasColumnName("preferredEquipement");
            entity.Property(e => e.PreferredService)
                .HasMaxLength(50)
                .HasColumnName("preferredService");
            entity.Property(e => e.Territory)
                .HasMaxLength(50)
                .HasColumnName("territory");
        });

        modelBuilder.Entity<ConfigurationTable>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PK__Configur__C3BC333CC4499F4F");

            entity.ToTable("ConfigurationTable");

            entity.Property(e => e.ConfigId)
                .ValueGeneratedNever()
                .HasColumnName("ConfigID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Account).WithMany(p => p.ConfigurationTables)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Configuration_Account");

            entity.HasOne(d => d.Product).WithMany(p => p.ConfigurationTables)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Configuration_Product");
        });

        modelBuilder.Entity<DestinationOleDb>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Destination OLE DB");

            entity.Property(e => e.Accountcategorycode).HasColumnName("accountcategorycode");
            entity.Property(e => e.Accountcategorycodename)
                .HasMaxLength(255)
                .HasColumnName("accountcategorycodename");
            entity.Property(e => e.Accountclassificationcode).HasColumnName("accountclassificationcode");
            entity.Property(e => e.Accountclassificationcodename)
                .HasMaxLength(255)
                .HasColumnName("accountclassificationcodename");
            entity.Property(e => e.Accountid).HasColumnName("accountid");
            entity.Property(e => e.Accountnumber)
                .HasMaxLength(20)
                .HasColumnName("accountnumber");
            entity.Property(e => e.Accountratingcode).HasColumnName("accountratingcode");
            entity.Property(e => e.Accountratingcodename)
                .HasMaxLength(255)
                .HasColumnName("accountratingcodename");
            entity.Property(e => e.Address1Addressid).HasColumnName("address1_addressid");
            entity.Property(e => e.Address1Addresstypecode).HasColumnName("address1_addresstypecode");
            entity.Property(e => e.Address1Addresstypecodename)
                .HasMaxLength(255)
                .HasColumnName("address1_addresstypecodename");
            entity.Property(e => e.Address1City)
                .HasMaxLength(80)
                .HasColumnName("address1_city");
            entity.Property(e => e.Address1Composite).HasColumnName("address1_composite");
            entity.Property(e => e.Address1Country)
                .HasMaxLength(80)
                .HasColumnName("address1_country");
            entity.Property(e => e.Address1County)
                .HasMaxLength(50)
                .HasColumnName("address1_county");
            entity.Property(e => e.Address1Fax)
                .HasMaxLength(50)
                .HasColumnName("address1_fax");
            entity.Property(e => e.Address1Freighttermscode).HasColumnName("address1_freighttermscode");
            entity.Property(e => e.Address1Freighttermscodename)
                .HasMaxLength(255)
                .HasColumnName("address1_freighttermscodename");
            entity.Property(e => e.Address1Latitude).HasColumnName("address1_latitude");
            entity.Property(e => e.Address1Line1)
                .HasMaxLength(250)
                .HasColumnName("address1_line1");
            entity.Property(e => e.Address1Line2)
                .HasMaxLength(250)
                .HasColumnName("address1_line2");
            entity.Property(e => e.Address1Line3)
                .HasMaxLength(250)
                .HasColumnName("address1_line3");
            entity.Property(e => e.Address1Longitude).HasColumnName("address1_longitude");
            entity.Property(e => e.Address1Name)
                .HasMaxLength(200)
                .HasColumnName("address1_name");
            entity.Property(e => e.Address1Postalcode)
                .HasMaxLength(20)
                .HasColumnName("address1_postalcode");
            entity.Property(e => e.Address1Postofficebox)
                .HasMaxLength(20)
                .HasColumnName("address1_postofficebox");
            entity.Property(e => e.Address1Primarycontactname)
                .HasMaxLength(100)
                .HasColumnName("address1_primarycontactname");
            entity.Property(e => e.Address1Shippingmethodcode).HasColumnName("address1_shippingmethodcode");
            entity.Property(e => e.Address1Shippingmethodcodename)
                .HasMaxLength(255)
                .HasColumnName("address1_shippingmethodcodename");
            entity.Property(e => e.Address1Stateorprovince)
                .HasMaxLength(50)
                .HasColumnName("address1_stateorprovince");
            entity.Property(e => e.Address1Telephone1)
                .HasMaxLength(50)
                .HasColumnName("address1_telephone1");
            entity.Property(e => e.Address1Telephone2)
                .HasMaxLength(50)
                .HasColumnName("address1_telephone2");
            entity.Property(e => e.Address1Telephone3)
                .HasMaxLength(50)
                .HasColumnName("address1_telephone3");
            entity.Property(e => e.Address1Upszone)
                .HasMaxLength(4)
                .HasColumnName("address1_upszone");
            entity.Property(e => e.Address1Utcoffset).HasColumnName("address1_utcoffset");
            entity.Property(e => e.Address2Addressid).HasColumnName("address2_addressid");
            entity.Property(e => e.Address2Addresstypecode).HasColumnName("address2_addresstypecode");
            entity.Property(e => e.Address2Addresstypecodename)
                .HasMaxLength(255)
                .HasColumnName("address2_addresstypecodename");
            entity.Property(e => e.Address2City)
                .HasMaxLength(80)
                .HasColumnName("address2_city");
            entity.Property(e => e.Address2Composite).HasColumnName("address2_composite");
            entity.Property(e => e.Address2Country)
                .HasMaxLength(80)
                .HasColumnName("address2_country");
            entity.Property(e => e.Address2County)
                .HasMaxLength(50)
                .HasColumnName("address2_county");
            entity.Property(e => e.Address2Fax)
                .HasMaxLength(50)
                .HasColumnName("address2_fax");
            entity.Property(e => e.Address2Freighttermscode).HasColumnName("address2_freighttermscode");
            entity.Property(e => e.Address2Freighttermscodename)
                .HasMaxLength(255)
                .HasColumnName("address2_freighttermscodename");
            entity.Property(e => e.Address2Latitude).HasColumnName("address2_latitude");
            entity.Property(e => e.Address2Line1)
                .HasMaxLength(250)
                .HasColumnName("address2_line1");
            entity.Property(e => e.Address2Line2)
                .HasMaxLength(250)
                .HasColumnName("address2_line2");
            entity.Property(e => e.Address2Line3)
                .HasMaxLength(250)
                .HasColumnName("address2_line3");
            entity.Property(e => e.Address2Longitude).HasColumnName("address2_longitude");
            entity.Property(e => e.Address2Name)
                .HasMaxLength(200)
                .HasColumnName("address2_name");
            entity.Property(e => e.Address2Postalcode)
                .HasMaxLength(20)
                .HasColumnName("address2_postalcode");
            entity.Property(e => e.Address2Postofficebox)
                .HasMaxLength(20)
                .HasColumnName("address2_postofficebox");
            entity.Property(e => e.Address2Primarycontactname)
                .HasMaxLength(100)
                .HasColumnName("address2_primarycontactname");
            entity.Property(e => e.Address2Shippingmethodcode).HasColumnName("address2_shippingmethodcode");
            entity.Property(e => e.Address2Shippingmethodcodename)
                .HasMaxLength(255)
                .HasColumnName("address2_shippingmethodcodename");
            entity.Property(e => e.Address2Stateorprovince)
                .HasMaxLength(50)
                .HasColumnName("address2_stateorprovince");
            entity.Property(e => e.Address2Telephone1)
                .HasMaxLength(50)
                .HasColumnName("address2_telephone1");
            entity.Property(e => e.Address2Telephone2)
                .HasMaxLength(50)
                .HasColumnName("address2_telephone2");
            entity.Property(e => e.Address2Telephone3)
                .HasMaxLength(50)
                .HasColumnName("address2_telephone3");
            entity.Property(e => e.Address2Upszone)
                .HasMaxLength(4)
                .HasColumnName("address2_upszone");
            entity.Property(e => e.Address2Utcoffset).HasColumnName("address2_utcoffset");
            entity.Property(e => e.AdxCreatedbyipaddress)
                .HasMaxLength(100)
                .HasColumnName("adx_createdbyipaddress");
            entity.Property(e => e.AdxCreatedbyusername)
                .HasMaxLength(100)
                .HasColumnName("adx_createdbyusername");
            entity.Property(e => e.AdxModifiedbyipaddress)
                .HasMaxLength(100)
                .HasColumnName("adx_modifiedbyipaddress");
            entity.Property(e => e.AdxModifiedbyusername)
                .HasMaxLength(100)
                .HasColumnName("adx_modifiedbyusername");
            entity.Property(e => e.Aging30)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("aging30");
            entity.Property(e => e.Aging30Base)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("aging30_base");
            entity.Property(e => e.Aging60)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("aging60");
            entity.Property(e => e.Aging60Base)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("aging60_base");
            entity.Property(e => e.Aging90)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("aging90");
            entity.Property(e => e.Aging90Base)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("aging90_base");
            entity.Property(e => e.Businesstypecode).HasColumnName("businesstypecode");
            entity.Property(e => e.Businesstypecodename)
                .HasMaxLength(255)
                .HasColumnName("businesstypecodename");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdbyexternalparty).HasColumnName("createdbyexternalparty");
            entity.Property(e => e.Createdbyexternalpartyname)
                .HasMaxLength(300)
                .HasColumnName("createdbyexternalpartyname");
            entity.Property(e => e.Createdbyexternalpartyyominame)
                .HasMaxLength(300)
                .HasColumnName("createdbyexternalpartyyominame");
            entity.Property(e => e.Createdbyname)
                .HasMaxLength(200)
                .HasColumnName("createdbyname");
            entity.Property(e => e.Createdbyyominame)
                .HasMaxLength(200)
                .HasColumnName("createdbyyominame");
            entity.Property(e => e.Createdon)
                .HasColumnType("datetime")
                .HasColumnName("createdon");
            entity.Property(e => e.Createdonbehalfby).HasColumnName("createdonbehalfby");
            entity.Property(e => e.Createdonbehalfbyname)
                .HasMaxLength(200)
                .HasColumnName("createdonbehalfbyname");
            entity.Property(e => e.Createdonbehalfbyyominame)
                .HasMaxLength(200)
                .HasColumnName("createdonbehalfbyyominame");
            entity.Property(e => e.Creditlimit)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("creditlimit");
            entity.Property(e => e.CreditlimitBase)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("creditlimit_base");
            entity.Property(e => e.Creditonhold).HasColumnName("creditonhold");
            entity.Property(e => e.Creditonholdname)
                .HasMaxLength(255)
                .HasColumnName("creditonholdname");
            entity.Property(e => e.Customersizecode).HasColumnName("customersizecode");
            entity.Property(e => e.Customersizecodename)
                .HasMaxLength(255)
                .HasColumnName("customersizecodename");
            entity.Property(e => e.Customertypecode).HasColumnName("customertypecode");
            entity.Property(e => e.Customertypecodename)
                .HasMaxLength(255)
                .HasColumnName("customertypecodename");
            entity.Property(e => e.Defaultpricelevelid).HasColumnName("defaultpricelevelid");
            entity.Property(e => e.Defaultpricelevelidname)
                .HasMaxLength(100)
                .HasColumnName("defaultpricelevelidname");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Donotbulkemail).HasColumnName("donotbulkemail");
            entity.Property(e => e.Donotbulkemailname)
                .HasMaxLength(255)
                .HasColumnName("donotbulkemailname");
            entity.Property(e => e.Donotbulkpostalmail).HasColumnName("donotbulkpostalmail");
            entity.Property(e => e.Donotbulkpostalmailname)
                .HasMaxLength(255)
                .HasColumnName("donotbulkpostalmailname");
            entity.Property(e => e.Donotemail).HasColumnName("donotemail");
            entity.Property(e => e.Donotemailname)
                .HasMaxLength(255)
                .HasColumnName("donotemailname");
            entity.Property(e => e.Donotfax).HasColumnName("donotfax");
            entity.Property(e => e.Donotfaxname)
                .HasMaxLength(255)
                .HasColumnName("donotfaxname");
            entity.Property(e => e.Donotphone).HasColumnName("donotphone");
            entity.Property(e => e.Donotphonename)
                .HasMaxLength(255)
                .HasColumnName("donotphonename");
            entity.Property(e => e.Donotpostalmail).HasColumnName("donotpostalmail");
            entity.Property(e => e.Donotpostalmailname)
                .HasMaxLength(255)
                .HasColumnName("donotpostalmailname");
            entity.Property(e => e.Donotsendmarketingmaterialname)
                .HasMaxLength(255)
                .HasColumnName("donotsendmarketingmaterialname");
            entity.Property(e => e.Donotsendmm).HasColumnName("donotsendmm");
            entity.Property(e => e.Emailaddress1)
                .HasMaxLength(100)
                .HasColumnName("emailaddress1");
            entity.Property(e => e.Emailaddress2)
                .HasMaxLength(100)
                .HasColumnName("emailaddress2");
            entity.Property(e => e.Emailaddress3)
                .HasMaxLength(100)
                .HasColumnName("emailaddress3");
            entity.Property(e => e.Entityimage).HasColumnName("entityimage");
            entity.Property(e => e.EntityimageTimestamp).HasColumnName("entityimage_timestamp");
            entity.Property(e => e.EntityimageUrl)
                .HasMaxLength(200)
                .HasColumnName("entityimage_url");
            entity.Property(e => e.Entityimageid).HasColumnName("entityimageid");
            entity.Property(e => e.Exchangerate)
                .HasColumnType("decimal(28, 12)")
                .HasColumnName("exchangerate");
            entity.Property(e => e.Fax)
                .HasMaxLength(50)
                .HasColumnName("fax");
            entity.Property(e => e.Followemail).HasColumnName("followemail");
            entity.Property(e => e.Followemailname)
                .HasMaxLength(255)
                .HasColumnName("followemailname");
            entity.Property(e => e.Ftpsiteurl)
                .HasMaxLength(200)
                .HasColumnName("ftpsiteurl");
            entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");
            entity.Property(e => e.Industrycode).HasColumnName("industrycode");
            entity.Property(e => e.Industrycodename)
                .HasMaxLength(255)
                .HasColumnName("industrycodename");
            entity.Property(e => e.Isprivatename)
                .HasMaxLength(255)
                .HasColumnName("isprivatename");
            entity.Property(e => e.Lastonholdtime)
                .HasColumnType("datetime")
                .HasColumnName("lastonholdtime");
            entity.Property(e => e.Lastusedincampaign)
                .HasColumnType("datetime")
                .HasColumnName("lastusedincampaign");
            entity.Property(e => e.Marketcap)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("marketcap");
            entity.Property(e => e.MarketcapBase)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("marketcap_base");
            entity.Property(e => e.Marketingonly).HasColumnName("marketingonly");
            entity.Property(e => e.Marketingonlyname)
                .HasMaxLength(255)
                .HasColumnName("marketingonlyname");
            entity.Property(e => e.Masteraccountidname)
                .HasMaxLength(160)
                .HasColumnName("masteraccountidname");
            entity.Property(e => e.Masteraccountidyominame)
                .HasMaxLength(160)
                .HasColumnName("masteraccountidyominame");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Merged).HasColumnName("merged");
            entity.Property(e => e.Mergedname)
                .HasMaxLength(255)
                .HasColumnName("mergedname");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiedbyexternalparty).HasColumnName("modifiedbyexternalparty");
            entity.Property(e => e.Modifiedbyexternalpartyname)
                .HasMaxLength(300)
                .HasColumnName("modifiedbyexternalpartyname");
            entity.Property(e => e.Modifiedbyexternalpartyyominame)
                .HasMaxLength(300)
                .HasColumnName("modifiedbyexternalpartyyominame");
            entity.Property(e => e.Modifiedbyname)
                .HasMaxLength(200)
                .HasColumnName("modifiedbyname");
            entity.Property(e => e.Modifiedbyyominame)
                .HasMaxLength(200)
                .HasColumnName("modifiedbyyominame");
            entity.Property(e => e.Modifiedon)
                .HasColumnType("datetime")
                .HasColumnName("modifiedon");
            entity.Property(e => e.Modifiedonbehalfby).HasColumnName("modifiedonbehalfby");
            entity.Property(e => e.Modifiedonbehalfbyname)
                .HasMaxLength(200)
                .HasColumnName("modifiedonbehalfbyname");
            entity.Property(e => e.Modifiedonbehalfbyyominame)
                .HasMaxLength(200)
                .HasColumnName("modifiedonbehalfbyyominame");
            entity.Property(e => e.MsaManagingpartnerid).HasColumnName("msa_managingpartnerid");
            entity.Property(e => e.MsaManagingpartneridname)
                .HasMaxLength(160)
                .HasColumnName("msa_managingpartneridname");
            entity.Property(e => e.MsaManagingpartneridyominame)
                .HasMaxLength(160)
                .HasColumnName("msa_managingpartneridyominame");
            entity.Property(e => e.MsdynAccountkpiid).HasColumnName("msdyn_accountkpiid");
            entity.Property(e => e.MsdynAccountkpiidname)
                .HasMaxLength(100)
                .HasColumnName("msdyn_accountkpiidname");
            entity.Property(e => e.MsdynGdproptout).HasColumnName("msdyn_gdproptout");
            entity.Property(e => e.MsdynGdproptoutname)
                .HasMaxLength(255)
                .HasColumnName("msdyn_gdproptoutname");
            entity.Property(e => e.MsdynPrimarytimezone).HasColumnName("msdyn_primarytimezone");
            entity.Property(e => e.MsdynSalesaccelerationinsightid).HasColumnName("msdyn_salesaccelerationinsightid");
            entity.Property(e => e.MsdynSalesaccelerationinsightidname)
                .HasMaxLength(100)
                .HasColumnName("msdyn_salesaccelerationinsightidname");
            entity.Property(e => e.Name)
                .HasMaxLength(160)
                .HasColumnName("name");
            entity.Property(e => e.Numberofemployees).HasColumnName("numberofemployees");
            entity.Property(e => e.Onholdtime).HasColumnName("onholdtime");
            entity.Property(e => e.Opendeals).HasColumnName("opendeals");
            entity.Property(e => e.OpendealsDate)
                .HasColumnType("datetime")
                .HasColumnName("opendeals_date");
            entity.Property(e => e.OpendealsState).HasColumnName("opendeals_state");
            entity.Property(e => e.Openrevenue)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("openrevenue");
            entity.Property(e => e.OpenrevenueBase)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("openrevenue_base");
            entity.Property(e => e.OpenrevenueDate)
                .HasColumnType("datetime")
                .HasColumnName("openrevenue_date");
            entity.Property(e => e.OpenrevenueState).HasColumnName("openrevenue_state");
            entity.Property(e => e.Originatingleadid).HasColumnName("originatingleadid");
            entity.Property(e => e.Originatingleadidname)
                .HasMaxLength(160)
                .HasColumnName("originatingleadidname");
            entity.Property(e => e.Originatingleadidyominame)
                .HasMaxLength(160)
                .HasColumnName("originatingleadidyominame");
            entity.Property(e => e.Overriddencreatedon)
                .HasColumnType("datetime")
                .HasColumnName("overriddencreatedon");
            entity.Property(e => e.Ownerid).HasColumnName("ownerid");
            entity.Property(e => e.Owneridname)
                .HasMaxLength(200)
                .HasColumnName("owneridname");
            entity.Property(e => e.Owneridtype)
                .HasMaxLength(64)
                .HasColumnName("owneridtype");
            entity.Property(e => e.Owneridyominame)
                .HasMaxLength(200)
                .HasColumnName("owneridyominame");
            entity.Property(e => e.Ownershipcode).HasColumnName("ownershipcode");
            entity.Property(e => e.Ownershipcodename)
                .HasMaxLength(255)
                .HasColumnName("ownershipcodename");
            entity.Property(e => e.Owningbusinessunit).HasColumnName("owningbusinessunit");
            entity.Property(e => e.Owningbusinessunitname)
                .HasMaxLength(160)
                .HasColumnName("owningbusinessunitname");
            entity.Property(e => e.Owningteam).HasColumnName("owningteam");
            entity.Property(e => e.Owninguser).HasColumnName("owninguser");
            entity.Property(e => e.Parentaccountid).HasColumnName("parentaccountid");
            entity.Property(e => e.Parentaccountidname)
                .HasMaxLength(160)
                .HasColumnName("parentaccountidname");
            entity.Property(e => e.Parentaccountidyominame)
                .HasMaxLength(160)
                .HasColumnName("parentaccountidyominame");
            entity.Property(e => e.Participatesinworkflow).HasColumnName("participatesinworkflow");
            entity.Property(e => e.Participatesinworkflowname)
                .HasMaxLength(255)
                .HasColumnName("participatesinworkflowname");
            entity.Property(e => e.Paymenttermscode).HasColumnName("paymenttermscode");
            entity.Property(e => e.Paymenttermscodename)
                .HasMaxLength(255)
                .HasColumnName("paymenttermscodename");
            entity.Property(e => e.Preferredappointmentdaycode).HasColumnName("preferredappointmentdaycode");
            entity.Property(e => e.Preferredappointmentdaycodename)
                .HasMaxLength(255)
                .HasColumnName("preferredappointmentdaycodename");
            entity.Property(e => e.Preferredappointmenttimecode).HasColumnName("preferredappointmenttimecode");
            entity.Property(e => e.Preferredappointmenttimecodename)
                .HasMaxLength(255)
                .HasColumnName("preferredappointmenttimecodename");
            entity.Property(e => e.Preferredcontactmethodcode).HasColumnName("preferredcontactmethodcode");
            entity.Property(e => e.Preferredcontactmethodcodename)
                .HasMaxLength(255)
                .HasColumnName("preferredcontactmethodcodename");
            entity.Property(e => e.Preferredequipmentid).HasColumnName("preferredequipmentid");
            entity.Property(e => e.Preferredequipmentidname)
                .HasMaxLength(160)
                .HasColumnName("preferredequipmentidname");
            entity.Property(e => e.Preferredserviceid).HasColumnName("preferredserviceid");
            entity.Property(e => e.Preferredserviceidname)
                .HasMaxLength(160)
                .HasColumnName("preferredserviceidname");
            entity.Property(e => e.Preferredsystemuserid).HasColumnName("preferredsystemuserid");
            entity.Property(e => e.Preferredsystemuseridname)
                .HasMaxLength(200)
                .HasColumnName("preferredsystemuseridname");
            entity.Property(e => e.Preferredsystemuseridyominame)
                .HasMaxLength(200)
                .HasColumnName("preferredsystemuseridyominame");
            entity.Property(e => e.Primarycontactid).HasColumnName("primarycontactid");
            entity.Property(e => e.Primarycontactidname)
                .HasMaxLength(160)
                .HasColumnName("primarycontactidname");
            entity.Property(e => e.Primarycontactidyominame)
                .HasMaxLength(160)
                .HasColumnName("primarycontactidyominame");
            entity.Property(e => e.Primarysatoriid)
                .HasMaxLength(200)
                .HasColumnName("primarysatoriid");
            entity.Property(e => e.Primarytwitterid)
                .HasMaxLength(128)
                .HasColumnName("primarytwitterid");
            entity.Property(e => e.Processid).HasColumnName("processid");
            entity.Property(e => e.Revenue)
                .HasColumnType("decimal(28, 2)")
                .HasColumnName("revenue");
            entity.Property(e => e.RevenueBase)
                .HasColumnType("decimal(28, 4)")
                .HasColumnName("revenue_base");
            entity.Property(e => e.Sharesoutstanding).HasColumnName("sharesoutstanding");
            entity.Property(e => e.Shippingmethodcode).HasColumnName("shippingmethodcode");
            entity.Property(e => e.Shippingmethodcodename)
                .HasMaxLength(255)
                .HasColumnName("shippingmethodcodename");
            entity.Property(e => e.Sic)
                .HasMaxLength(20)
                .HasColumnName("sic");
            entity.Property(e => e.Slaid).HasColumnName("slaid");
            entity.Property(e => e.Slainvokedid).HasColumnName("slainvokedid");
            entity.Property(e => e.Slainvokedidname)
                .HasMaxLength(100)
                .HasColumnName("slainvokedidname");
            entity.Property(e => e.Slaname)
                .HasMaxLength(100)
                .HasColumnName("slaname");
            entity.Property(e => e.Stageid).HasColumnName("stageid");
            entity.Property(e => e.Statecode).HasColumnName("statecode");
            entity.Property(e => e.Statecodename)
                .HasMaxLength(255)
                .HasColumnName("statecodename");
            entity.Property(e => e.Statuscode).HasColumnName("statuscode");
            entity.Property(e => e.Statuscodename)
                .HasMaxLength(255)
                .HasColumnName("statuscodename");
            entity.Property(e => e.Stockexchange)
                .HasMaxLength(20)
                .HasColumnName("stockexchange");
            entity.Property(e => e.Teamsfollowed).HasColumnName("teamsfollowed");
            entity.Property(e => e.Telephone1)
                .HasMaxLength(50)
                .HasColumnName("telephone1");
            entity.Property(e => e.Telephone2)
                .HasMaxLength(50)
                .HasColumnName("telephone2");
            entity.Property(e => e.Telephone3)
                .HasMaxLength(50)
                .HasColumnName("telephone3");
            entity.Property(e => e.Territorycode).HasColumnName("territorycode");
            entity.Property(e => e.Territorycodename)
                .HasMaxLength(255)
                .HasColumnName("territorycodename");
            entity.Property(e => e.Territoryid).HasColumnName("territoryid");
            entity.Property(e => e.Territoryidname)
                .HasMaxLength(200)
                .HasColumnName("territoryidname");
            entity.Property(e => e.Tickersymbol)
                .HasMaxLength(10)
                .HasColumnName("tickersymbol");
            entity.Property(e => e.Timespentbymeonemailandmeetings)
                .HasMaxLength(1250)
                .HasColumnName("timespentbymeonemailandmeetings");
            entity.Property(e => e.Timezoneruleversionnumber).HasColumnName("timezoneruleversionnumber");
            entity.Property(e => e.Transactioncurrencyid).HasColumnName("transactioncurrencyid");
            entity.Property(e => e.Transactioncurrencyidname)
                .HasMaxLength(100)
                .HasColumnName("transactioncurrencyidname");
            entity.Property(e => e.Traversedpath)
                .HasMaxLength(1250)
                .HasColumnName("traversedpath");
            entity.Property(e => e.Utcconversiontimezonecode).HasColumnName("utcconversiontimezonecode");
            entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            entity.Property(e => e.Websiteurl)
                .HasMaxLength(200)
                .HasColumnName("websiteurl");
            entity.Property(e => e.Yominame)
                .HasMaxLength(160)
                .HasColumnName("yominame");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("productId");
            entity.Property(e => e.CurrentCost)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("currentCost");
            entity.Property(e => e.DefaultUoMidName)
                .HasMaxLength(50)
                .HasColumnName("defaultUoMidName");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.ProductNumber)
                .HasMaxLength(50)
                .HasColumnName("productNumber");
            entity.Property(e => e.ProductStructureName)
                .HasMaxLength(50)
                .HasColumnName("productStructureName");
            entity.Property(e => e.ProductTypeCode).HasColumnName("productTypeCode");
            entity.Property(e => e.QuantityOnHand).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StantardCost)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("stantardCost");
            entity.Property(e => e.StateCodeName)
                .HasMaxLength(50)
                .HasColumnName("stateCodeName");
            entity.Property(e => e.StatusCodeName)
                .HasMaxLength(50)
                .HasColumnName("statusCodeName");
            entity.Property(e => e.StockVolume)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("stockVolume");
            entity.Property(e => e.StockWeight)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("stockWeight");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
