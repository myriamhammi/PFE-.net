using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TEST_PFE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    originatingLead = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    preferredEquipement = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    preferredService = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    territory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    opendeals = table.Column<int>(type: "int", nullable: true),
                    openRevenue = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    openRevenueBase = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Destination OLE DB",
                columns: table => new
                {
                    accountcategorycode = table.Column<int>(type: "int", nullable: true),
                    accountcategorycodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    accountclassificationcode = table.Column<int>(type: "int", nullable: true),
                    accountclassificationcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    accountid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    accountnumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    accountratingcode = table.Column<int>(type: "int", nullable: true),
                    accountratingcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address1_addressid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    address1_addresstypecode = table.Column<int>(type: "int", nullable: true),
                    address1_addresstypecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address1_city = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    address1_composite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address1_country = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    address1_county = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_freighttermscode = table.Column<int>(type: "int", nullable: true),
                    address1_freighttermscodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address1_latitude = table.Column<double>(type: "float", nullable: true),
                    address1_line1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address1_line2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address1_line3 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address1_longitude = table.Column<double>(type: "float", nullable: true),
                    address1_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    address1_postalcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address1_postofficebox = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address1_primarycontactname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address1_shippingmethodcode = table.Column<int>(type: "int", nullable: true),
                    address1_shippingmethodcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address1_stateorprovince = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_telephone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_telephone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_telephone3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address1_upszone = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    address1_utcoffset = table.Column<int>(type: "int", nullable: true),
                    address2_addressid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    address2_addresstypecode = table.Column<int>(type: "int", nullable: true),
                    address2_addresstypecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address2_city = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    address2_composite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address2_country = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    address2_county = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_freighttermscode = table.Column<int>(type: "int", nullable: true),
                    address2_freighttermscodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address2_latitude = table.Column<double>(type: "float", nullable: true),
                    address2_line1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address2_line2 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address2_line3 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    address2_longitude = table.Column<double>(type: "float", nullable: true),
                    address2_name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    address2_postalcode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address2_postofficebox = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    address2_primarycontactname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    address2_shippingmethodcode = table.Column<int>(type: "int", nullable: true),
                    address2_shippingmethodcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    address2_stateorprovince = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_telephone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_telephone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_telephone3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    address2_upszone = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    address2_utcoffset = table.Column<int>(type: "int", nullable: true),
                    adx_createdbyipaddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    adx_createdbyusername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    adx_modifiedbyipaddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    adx_modifiedbyusername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    aging30 = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    aging30_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    aging60 = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    aging60_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    aging90 = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    aging90_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    businesstypecode = table.Column<int>(type: "int", nullable: true),
                    businesstypecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createdby = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    createdbyexternalparty = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    createdbyexternalpartyname = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    createdbyexternalpartyyominame = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    createdbyname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    createdbyyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    createdon = table.Column<DateTime>(type: "datetime", nullable: true),
                    createdonbehalfby = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    createdonbehalfbyname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    createdonbehalfbyyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    creditlimit = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    creditlimit_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    creditonhold = table.Column<bool>(type: "bit", nullable: true),
                    creditonholdname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    customersizecode = table.Column<int>(type: "int", nullable: true),
                    customersizecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    customertypecode = table.Column<int>(type: "int", nullable: true),
                    customertypecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    defaultpricelevelid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    defaultpricelevelidname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    donotbulkemail = table.Column<bool>(type: "bit", nullable: true),
                    donotbulkemailname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotbulkpostalmail = table.Column<bool>(type: "bit", nullable: true),
                    donotbulkpostalmailname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotemail = table.Column<bool>(type: "bit", nullable: true),
                    donotemailname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotfax = table.Column<bool>(type: "bit", nullable: true),
                    donotfaxname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotphone = table.Column<bool>(type: "bit", nullable: true),
                    donotphonename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotpostalmail = table.Column<bool>(type: "bit", nullable: true),
                    donotpostalmailname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotsendmarketingmaterialname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    donotsendmm = table.Column<bool>(type: "bit", nullable: true),
                    emailaddress1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    emailaddress2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    emailaddress3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    entityimage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    entityimage_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    entityimage_url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    entityimageid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    exchangerate = table.Column<decimal>(type: "decimal(28,12)", nullable: true),
                    fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    followemail = table.Column<bool>(type: "bit", nullable: true),
                    followemailname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ftpsiteurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    importsequencenumber = table.Column<int>(type: "int", nullable: true),
                    industrycode = table.Column<int>(type: "int", nullable: true),
                    industrycodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    isprivatename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    lastonholdtime = table.Column<DateTime>(type: "datetime", nullable: true),
                    lastusedincampaign = table.Column<DateTime>(type: "datetime", nullable: true),
                    marketcap = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    marketcap_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    marketingonly = table.Column<bool>(type: "bit", nullable: true),
                    marketingonlyname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    masteraccountidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    masteraccountidyominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    masterid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    merged = table.Column<bool>(type: "bit", nullable: true),
                    mergedname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    modifiedby = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifiedbyexternalparty = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifiedbyexternalpartyname = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    modifiedbyexternalpartyyominame = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    modifiedbyname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    modifiedbyyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    modifiedon = table.Column<DateTime>(type: "datetime", nullable: true),
                    modifiedonbehalfby = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    modifiedonbehalfbyname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    modifiedonbehalfbyyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    msa_managingpartnerid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    msa_managingpartneridname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    msa_managingpartneridyominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    msdyn_accountkpiid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    msdyn_accountkpiidname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    msdyn_gdproptout = table.Column<bool>(type: "bit", nullable: true),
                    msdyn_gdproptoutname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    msdyn_primarytimezone = table.Column<int>(type: "int", nullable: true),
                    msdyn_salesaccelerationinsightid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    msdyn_salesaccelerationinsightidname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    numberofemployees = table.Column<int>(type: "int", nullable: true),
                    onholdtime = table.Column<int>(type: "int", nullable: true),
                    opendeals = table.Column<int>(type: "int", nullable: true),
                    opendeals_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    opendeals_state = table.Column<int>(type: "int", nullable: true),
                    openrevenue = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    openrevenue_base = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    openrevenue_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    openrevenue_state = table.Column<int>(type: "int", nullable: true),
                    originatingleadid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    originatingleadidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    originatingleadidyominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    overriddencreatedon = table.Column<DateTime>(type: "datetime", nullable: true),
                    ownerid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    owneridname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    owneridtype = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    owneridyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ownershipcode = table.Column<int>(type: "int", nullable: true),
                    ownershipcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    owningbusinessunit = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    owningbusinessunitname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    owningteam = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    owninguser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parentaccountid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    parentaccountidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    parentaccountidyominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    participatesinworkflow = table.Column<bool>(type: "bit", nullable: true),
                    participatesinworkflowname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    paymenttermscode = table.Column<int>(type: "int", nullable: true),
                    paymenttermscodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    preferredappointmentdaycode = table.Column<int>(type: "int", nullable: true),
                    preferredappointmentdaycodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    preferredappointmenttimecode = table.Column<int>(type: "int", nullable: true),
                    preferredappointmenttimecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    preferredcontactmethodcode = table.Column<int>(type: "int", nullable: true),
                    preferredcontactmethodcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    preferredequipmentid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    preferredequipmentidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    preferredserviceid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    preferredserviceidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    preferredsystemuserid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    preferredsystemuseridname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    preferredsystemuseridyominame = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    primarycontactid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    primarycontactidname = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    primarycontactidyominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    primarysatoriid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    primarytwitterid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    processid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    revenue = table.Column<decimal>(type: "decimal(28,2)", nullable: true),
                    revenue_base = table.Column<decimal>(type: "decimal(28,4)", nullable: true),
                    sharesoutstanding = table.Column<int>(type: "int", nullable: true),
                    shippingmethodcode = table.Column<int>(type: "int", nullable: true),
                    shippingmethodcodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    sic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    slaid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    slainvokedid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    slainvokedidname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    slaname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    stageid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    statecode = table.Column<int>(type: "int", nullable: true),
                    statecodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    statuscode = table.Column<int>(type: "int", nullable: true),
                    statuscodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    stockexchange = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    teamsfollowed = table.Column<int>(type: "int", nullable: true),
                    telephone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    telephone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    telephone3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    territorycode = table.Column<int>(type: "int", nullable: true),
                    territorycodename = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    territoryid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    territoryidname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    tickersymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    timespentbymeonemailandmeetings = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: true),
                    timezoneruleversionnumber = table.Column<int>(type: "int", nullable: true),
                    transactioncurrencyid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    transactioncurrencyidname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    traversedpath = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: true),
                    utcconversiontimezonecode = table.Column<int>(type: "int", nullable: true),
                    versionnumber = table.Column<long>(type: "bigint", nullable: true),
                    websiteurl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    yominame = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    productId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    currentCost = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    defaultUoMidName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    productNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    productStructureName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    productTypeCode = table.Column<int>(type: "int", nullable: true),
                    QuantityOnHand = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    stantardCost = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    stateCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    statusCodeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    stockVolume = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    stockWeight = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    ValidFromDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.productId);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationTable",
                columns: table => new
                {
                    ConfigID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Configur__C3BC333CC4499F4F", x => x.ConfigID);
                    table.ForeignKey(
                        name: "FK_Configuration_Account",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                    table.ForeignKey(
                        name: "FK_Configuration_Product",
                        column: x => x.ProductID,
                        principalTable: "product",
                        principalColumn: "productId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationTable_AccountID",
                table: "ConfigurationTable",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationTable_ProductID",
                table: "ConfigurationTable",
                column: "ProductID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigurationTable");

            migrationBuilder.DropTable(
                name: "Destination OLE DB");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
