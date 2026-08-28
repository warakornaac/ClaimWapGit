using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClaimWap.Models
{
    public class Claimlist
    {
        public string Id { get; set; }
        public string REQ_NO { get; set; }
        public string CLM_NO { get; set; }
        public string REQ_DATE { get; set; }
        public string CLM_DUEDATE { get; set; }
        public string Invoice_No { get; set; }
        public string Customer { get; set; }
        public string Item_No { get; set; }
        public string Description { get; set; }
        public string CLM_UOM { get; set; }
        public string CLM_QTY { get; set; }
        public string CLM_PERFORM_TEXT { get; set; }
        public string CLM_UPDATE_DATE { get; set; }
        public string CLM_UPDATE_BY { get; set; }
        public string CLM_STATUS_TEXT { get; set; }
        public List<Claimlist> Claim_Grid { get; set; }
    }
    public class CUS
    {
        public string CUSCOD { get; set; }
        public string CUSNAM { get; set; }
        public string CUSTYP { get; set; }
        public string PRO { get; set; }
        public string ADDR_01 { get; set; }
        public string Block_flg { get; set; }
        public string AACCrlimit { get; set; }
        public string AACBalance { get; set; }
        public string TACCrlimit { get; set; }
        public string TACBalance { get; set; }

    }
    public class shipto
    {
        public string customer { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string name2 { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
    }
    public class ItemListshipto
    {
        public shipto val { get; set; }

    }
    public class Stock
    {
        public string client	 { get; set; }
        public string nav_bin	 { get; set; }
        public string item_no	 { get; set; }
        public string Qty_avail { get; set; }	 
        public string RevQty	 { get; set; }
        public string WaitingPackQty { get; set; }
    }
    public class ListStock
    {
        public Stock val { get; set; }

    }
    public class InvoiceStatus
    {
        public string COMP{ get; set; }
        public string DOCNUM{ get; set; }
        public string PSTDAT{ get; set; }
        public string PEOPLE{ get; set; }
        public string CUSNAM{ get; set; }
        public string SLMCOD{ get; set; }
        public string SEC{ get; set; }
        public string STKGRP{ get; set; }
        public string STKCOD{ get; set; }
        public string STKDES{ get; set; }
        public string Qty{ get; set; }
        public string Price{ get; set; }
        public string Amt{ get; set; }
        public string DiscountPer { get; set; }
        public string DiscountAmt{ get; set; }
        public string Net_Price{ get; set; }
        public string NetAmt{ get; set; }
        public string FOC{ get; set; }
        public string AsOf{ get; set; }
        public string LastClaimNo{ get; set; }
        public string LastClaimdate{ get; set; }
        public string ClaimStatus { get; set; }
        public string Uom { get; set; }
        public string SLM { get; set; }
        public string Cuscod { get; set; }
        public string Prod { get; set; }
        public string ProdName { get; set; }
        public string GrpName { get; set; }
        public string QTY_Remaining  { get; set; }
        public string EXTDOC { get; set; }
        public string CrRefNo { get; set; }
        public List<InvoiceStatus> InvoiceStatus_Grid { get; set; }
     }
    public class InvoiceStatusListDetailGetdata
    {
        public InvoiceStatus val { get; set; }

    }
    public class VideoFiles
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> FileSize { get; set; }
        public string FilePath { get; set; }
    } 
    public class Stkgrp
    {

        public string STKGRP { get; set; }
        public string GRPNAM { get; set; }
    }
    public class Grptech2
    {

        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    public class Section
    {

        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Supno
    {

        public string No { get; set; }
        public string Detail { get; set; }
    }
    public class UseCustomer
    {
        public string ID { get; set; }
        public string USRID { get; set; }
     
    }
    public class UsrGrp_special
    {
        
       public string UsrID{ get; set; }
       public string Department{ get; set; }
       public string Email{ get; set; }
       public string UserCode { get; set; }
       public string Password{ get; set; }
       public string LastLogOn{ get; set; }
       public string pwdLastSet{ get; set; }
       public string Status{ get; set; }

    }
    public class UsrGrp_specialDetail
    {
        public UsrGrp_special val { get; set; }

    }
    public class Attribute
    {

        public string AttributeCode { get; set; }
        public string Description { get; set; }
    }
    public class PmListStkgrp
    {
        public Stkgrp val { get; set; }

    }
    public class SLM
    {

        public string SLMCOD { get; set; }
        public string SLMNAM { get; set; }
    }

    public class Datasave
    {
      public string user{ get; set; }
      public string req_no { get; set; }
      public string clm_no_sub { get; set; }
      public string scrapdatetec1 { get; set; }   
   
    }
    public class DefineCodeGetdata
    {
        public DefineCode val { get; set; }

    }
    public class CountProcess
    {
        public string CountAdmin { get; set; }
        public string Status { get; set; }
        public string company { get; set; }
       
    }
    public class ListCountProcess
    {
        public CountProcess val { get; set; }

    }
    public class DefineCode
    {
        public string ID { get; set; }
        public string CODE { get; set; }
        public string FIELD_RELATION { get; set; }
        public string DESCRIPTION_TH { get; set; }
        public string DESCRIPTION_EN { get; set; }
        public string INACTIVE { get; set; }
    }
    public class Rtwhadmin
    {       
         public string STMP_ID   { get; set; }
		 public string STMP_ID_SUB  { get; set; }
         public string STMP_STKCOD { get; set; }
         public string STMP_STKDES { get; set; }
		 public string STMP_ADMIN  { get; set; }
         public string STMP_SALES_REQQTY { get; set; }
		 public string STMP_ADMIN_REQQTY  { get; set; }
		 public string STMP_ADMIN_REQ_DATE  { get; set; }
		 public string ADMIN_REMARK   { get; set; }
		 public string STMP_QTY  { get; set; }
		 public string STMP_UPDATE_DATE   { get; set; }
		 public string STMP_UPDATE_BY  { get; set; }
		 public string STMP_STATUS  { get; set; }
         public string userlogin { get; set; }
    }
    public class RtwhadminListDetail
    {
        public Rtwhadmin val { get; set; }

    }
    public class ClimeWhdata
    {
        public string REQ_NO { get; set; }
        public string COMP { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_DATE { get; set; }
        public string REQ_Update_BY { get; set; }
        public string REQ_Update_DATE { get; set; }
        public string REQ_Dep_BY { get; set; }
        public string Imgsignature { get; set; }
        public string CLM_NO_SUB { get; set; }
        public string CLM_COMPANY { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string CLM_UOM { get; set; }
        public string CLM_STKGRP { get; set; }
        public string CLM_Location { get; set; }
        public string CLM_INVNO { get; set; }
        public string INV_QTY { get; set; }
        public string CLM_INVDATE { get; set; }
        public string CLM_REQ_QTY { get; set; }
        public string CLM_DATE { get; set; }
        public string CLM_USEDAY { get; set; }
        public string CLM_CAUSE { get; set; }
        public string CLM_PERFORM { get; set; }
        public string CLM_PERFORM_DES { get; set; }
        public string CLM_RCVSTATUS { get; set; }
        public string CLM_RCVBY { get; set; }
        public string CLM_RCVDATE { get; set; }
        public string CLM_ADMIN { get; set; }
        public string CLM_ADMIN_REMARK { get; set; }
        public string ADMIN_ANLYS_STATUS { get; set; }
        public string CLM_STATUS { get; set; }
        public string TECH1_NAME { get; set; }
        public string TECH1_ANLYS_DATE { get; set; }
        public string TECH1_ANLYS_RESULT { get; set; }
        public string TECH1_ANLYS_STATUS { get; set; }
        public string TECH1_PROCESS_STATUS { get; set; }
        public string ANLYS_AFTERPROCESS { get; set; }
        public string SCRAP_DATE { get; set; }
        public string CLM_TECH1_QTY { get; set; }
        public string TECH2_NAME { get; set; }
        public string TECH2_ANLYS_DATE { get; set; }
        public string TECH2_ANLYS_STATUS { get; set; }
        public string TECH2_PROCESS_STATUS { get; set; }
        public string TECH2_REMARK { get; set; }
        public string CLM_TECH2_QTY { get; set; }
        public string PM_NAME { get; set; }
        public string PM_APPRV_DATE { get; set; }
        public string PM_APPRV_STATUS { get; set; }
        public string PM_PROCESS_STATUS { get; set; }
        public string PM_Replacement { get; set; }
        public string PM_REMARK { get; set; }
        public string CLM_PM_QTY { get; set; }
        public string User_Print { get; set; }
        public string F_Supplier { get; set; }
        public string F_Disposal { get; set; }
        public string F_SupplierDate { get; set; }
        public string F_DisposalDate { get; set; }
        public string F_Mail { get; set; }
        public string CLM_UPDATE_BY { get; set; }
        public string CLM_UPDATE_DATE { get; set; }
        public string SEC { get; set; }
        public string DEPNAM { get; set; }
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
        public string GRPNAM { get; set; }
        public string PERFORMDESCRIPTION { get; set; }
        public string RCVSTATUSDESCRIPTION { get; set; }
        public string Technician { get; set; }
        public string ClmtypeTech { get; set; }
        public string Clmtype { get; set; }
        public string Process_Tech { get; set; }
        public string Process_PM { get; set; }
        public string TEC_ANLYS_STATUS { get; set; }
        public string TEC_PROCESS_STATUS { get; set; }
        public string TEC_ANLYS_AFTERPROCESS { get; set; }
        public string PM_ANLYS_STATUS { get; set; }
        public string PM_PROCESS_STATUS_ANLYS { get; set; }
        public string PM_ANLYS_AFTERPROCESS { get; set; }
        public string TECH1NAME { get; set; }
        public string CLM_SHELF_LOCATION { get; set; }
        public string CLM_QTY { get; set; }
        public string CLM_DUEDATE { get; set; }
        public string PM_AFTER_APPRV { get; set; }
        public string CLM_COMMENT { get; set; }
        public string CLM_TECH2_WASTE_QTY { get; set; }
        public string CLM_TECH2_GOOD_QTY { get; set; }
        public string CLM_BatchCode { get; set; }

        public string PM_Option1 { get; set; }
        public string PM_Optiontext1{ get; set; }
		public string PM_Option2{ get; set; }
		public string PM_Optiontext2{ get; set; }
		public string PM_Option3{ get; set; }
		public string PM_Optiontext3{ get; set; }
		public string PM_Option4{ get; set; }
		public string PM_Optiontext4{ get; set; }
		public string PM_Option5{ get; set; }
        public string PM_Optiontext5 { get; set; }

        public string ADMINWH_STATUS{ get; set; }
        public string ADMINWH_BY{ get; set; }
        public string ADMINWH_UPDATE{ get; set; }
        public string ADMINWH_REMARK { get; set; }

        public string UNITCOST { get; set; }
        public string TOTALCOST { get; set; }

        public string BU_Head{ get; set; }
        public string BU_APPRV_DATE { get; set; }
        public string BU_APPRV_STATUS{ get; set; }
        public string BU_REMARK{ get; set; }
        public string CLM_BU_QTY{ get; set; }
        public string CLM_BU_Ref { get; set; }

        public string BU_HeadNAM { get; set; }


        public string MD_Head { get; set; }
        public string MD_APPRV_DATE { get; set; }
        public string MD_APPRV_STATUS { get; set; }
        public string MD_REMARK { get; set; }
        public string CLM_MD_QTY { get; set; }
        public string CLM_MD_Ref { get; set; }
    }
    public class ClimewhListDetail
    {
        public ClimeWhdata val { get; set; }

    }
    public class Climedata
    {
      
       public string CLM_ID { get; set; }
       public string CLM_NO_SUB { get; set; }
       public string CUSCOD { get; set; }
      
       public string CLM_RCVBY { get; set; }
       public string CLM_RCVDATE { get; set; }
       public string STKCOD { get; set; }
       public string STKDES { get; set; }
       public string CLM_UOM { get; set; }
       public string CLM_INVNO { get; set; }
       public string CLM_INVDATE { get; set; }
       public string CLM_REQQTY { get; set; }
       public string CLM_QTY { get; set; }
       public string CLM_QTY_ORG { get; set; }
       public string CLM_QTY_PASS { get; set; }
       public string CLM_QTY_REJECT { get; set; }
       public string CLM_USEDAY { get; set; }
       public string CLM_CAUSE { get; set; }
       public string CLM_PERFORM { get; set; }
       public string CLM_PERFORM_DES { get; set; }
       public string CLM_RCVSTATUS { get; set; }
       public string PERFORMDESCRIPTION { get; set; }
       public string RCVSTATUSDESCRIPTION { get; set; }
       public string CLM_COMPANY { get; set; }
       public string CUSNAM { get; set; }
       public string SLMCOD { get; set; }
       public string SLMNAM { get; set; }
       public string ADMIN_ANLYS_STATUS { get; set; }
       public string TECH1_PROCESS_STATUS{ get; set; }
       public string TECH2_PROCESS_STATUS{ get; set; }
       public string TECH1_ANLYS_STATUS { get; set; }
       public string TECH1ANLYSSTATUDESCRIPTION { get; set; }
       public string TECH2_ANLYS_STATUS { get; set; }
       public string PM_PROCESS_STATUS { get; set; }
       public string Status { get; set; }
       public string CLM_NO { get; set; }
       public string CLM_Machine  { get; set; }
		public string CLM_Model { get; set; }
		public string CLM_ModelYear  { get; set; }
		public string CLM_EngineCode { get; set; }
		public string CLM_ChassisNo { get; set; }
		public string CLM_InjecPump { get; set; }
		public string CLM_TypeProduct { get; set; }
		public string CLM_WarrantyNo { get; set; }
		public string CLM_Milage { get; set; }
		public string CLM_DateDamage { get; set; }
        public string CLM_BatchCode { get; set; }
        public string CLM_STATUS { get; set; }
        public string STKGRP { get; set; }
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
        public string CLM_Ref  { get; set; }
        public string CLM_DATE   { get; set; }
        public string CLM_FOC { get; set; }
        public string F_BtnApp { get; set; }
        public string CLM_Installdate { get; set; }
        public string CLM_Contact { get; set; }
        public string CLM_ContactTel { get; set; }
        public string Requestdate { get; set; }
        public string Requestdatecus { get; set; }
        public string DEP { get; set; }
        public string DEPNAM { get; set; }
        public string GRPNAM { get; set; }
        public string CLM_Owner { get; set; }
        public string CLM_Location { get; set; }
        public string InsertBy  { get; set; }
        public string InsertDate { get; set; }
        public string CLM_SHELF_LOCATION { get; set; }
        public string CLM_TECH1_QTY { get; set; }
        public string CLM_TECH2_QTY { get; set; }
        public string CLM_PM_QTY { get; set; }
        public string REQ_BY  { get; set; }
        public string REQ_DATE { get; set; }
        public string REQ_Dep_BY { get; set; }
        public string CLM_UPDATE_BY { get; set; }
        public string CLM_UPDATE_DATE { get; set; }
        public string CLM_DUEDATE { get; set; }
        public string Log_No { get; set; }
        public string STATUSTEXT { get; set; }
        public string ANLYS_AFTERPROCESSTEXT { get; set; }
        public string ANLYS_AFTERPROCESS { get; set; }
        public string P_Claim { get; set; }
        public string User_Print { get; set; }
        public string PM_Option1 { get; set; }
        public string PM_Optiontext1 { get; set; }
        public string PM_Option2 { get; set; }
        public string PM_Optiontext2 { get; set; }
        public string PM_Option3 { get; set; }
        public string PM_Optiontext3 { get; set; }
        public string PM_Option4 { get; set; }
        public string PM_Optiontext4 { get; set; }
        public string PM_Option5 { get; set; }
        public string PM_Optiontext5 { get; set; }

        public string BU_Head { get; set; }
        public string BU_APPRV_DATE { get; set; }
        public string BU_APPRV_STATUS { get; set; }
        public string BU_REMARK { get; set; }
        public string CLM_BU_QTY { get; set; }
        public string CLM_BU_Ref { get; set; }

        public string MD_Head { get; set; }
        public string MD_APPRV_DATE { get; set; }
        public string MD_APPRV_STATUS { get; set; }
        public string MD_REMARK { get; set; }


        public string PMCODE { get; set; }
        public string PM_NAME { get; set; }

        public string PM_APPRV_DATE { get; set; }

        public string UNITCOST { get; set; }

        public string TOTCOST { get; set; }

        public string totcost { get; set; }

        public string CLM_MD_QTY { get; set; }

        public string CLM_MD_Ref { get; set; }
        public string SM_NAME { get; set; }
        public string SM_APPRV_DATE { get; set; }
        public string SM_PROCESS_STATUS { get; set; }
        public string SM_REMARK { get; set; }

        public string GM_NAME { get; set; }
        public string GM_APPRV_DATE { get; set; }
        public string GM_PROCESS_STATUS { get; set; }
        public string GM_REMARK { get; set; }
        public string CLM_ClaimNote { get; set; }
    }
    public class ClimetempListDetail
    {
        public Climedata val { get; set; }

    }
    public class ClimeDetail
    {
        public string CLM_ADMIN { get; set; }
        public string CLM_SHELF_LOCATION{ get; set; }
        public string CLM_RCVSTATUS { get; set; }
        public string CLM_QTY_ORG { get; set; }
        public string CLM_QTY { get; set; }
        public string CLM_QTY_PASS { get; set; }
        public string CLM_QTY_REJECT { get; set; }
        public string CLM_REQQTY { get; set; } 
        public string CLM_REMARK{ get; set; }
        public string CLM_RCVDATE { get; set; }
        public string CLM_RCVBY { get; set; }
        public string CLM_UOM{ get; set; }
        public string CLM_DUEDATE{ get; set; }
        public string CLM_DATE{ get; set; }
        public string Technician{ get; set; }
		public string STKGRP{ get; set; }
		public string TECH1_NAME{ get; set; }
        public string TECH1_ANLYS_DATE{ get; set; }
        public string CLM_PERFORM{ get; set; }
        public string ADMIN_ANLYS_STATUS { get; set; }
        public string TECH1_ANLYS_STATUS{ get; set; }
        public string ANLYS_AFTERPROCESS{ get; set; }
        public string SCRAP_DATE{ get; set; }
        public string TECH1_ANLYS_RESULT{ get; set; }
		public string TECH1_PROCESS_STATUS{ get; set; }		 
		public string TECH2_NAME{ get; set; }
		public string TECH2_ANLYS_STATUS{ get; set; }
		public string TECH2_REMARK{ get; set; }
		public string TECH2_ProblemCategory { get; set; }
		public string TECH2_ProblemCategoryDetail { get; set; }
		public string TECH2_ANLYS_DATE{ get; set; }
		public string TECH2_PROCESS_STATUS{ get; set; }
		public string PM_NAME { get; set; }
		public string PM_APPRV_DATE{ get; set; }
		public string PM_APPRV_STATUS{ get; set; } 
		public string PM_PROCESS_STATUS{ get; set; }
		public string PM_Replacement{ get; set; }
		public string PM_REMARK { get; set; }
		public string CLM_STATUS{ get; set; }
        public string CLM_UPDATE_DATE{ get; set; }
        public string CLM_UPDATE_BY { get; set; }
        public string Customer { get; set; }
        public string CLM_FRMSUP_STATUS { get; set; }
        public string REQ_NO  { get; set; }
        public string CLM_NO_SUB { get; set; }
        public string REQ_DATE { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string CLM_INVNO { get; set; }
        public string STATUSTEXT { get; set; }
        public string CLM_PERFORMTEXT { get; set; }
        public string ANLYS_AFTERPROCESSTEXT { get; set; }
        public string CLM_FRMSUP_DATE{ get; set; }
        public string CLM_FRMSUP_NO { get; set; }
        public string CLM_NO { get; set; }
        public string SLMCOD { get; set; }
        public string FCLM_QTY { get; set; }
        public string CLM_Machine{ get; set; }
        public string CLM_Model{ get; set; }
        public string CLM_ModelYear{ get; set; }
        public string CLM_EngineCode{ get; set; }
        public string CLM_ChassisNo{ get; set; }
        public string CLM_InjecPump{ get; set; }
        public string CLM_TypeProduct{ get; set; }
        public string CLM_WarrantyNo{ get; set; }
        public string CLM_Milage{ get; set; }
        public string CLM_DateDamage{ get; set; }
        public string CLM_BatchCode { get; set; }
        public string CLM_COMPANY { get; set; }
        public string CLM_CAUSE { get; set; }
        public string CLM_USEDAY { get; set; }
        public string PERFORMDESCRIPTION { get; set; }
        public string RCVSTATUSDESCRIPTION { get; set; }
        public string PROD { get; set; }
        public string CUSNAM { get; set; }
        public string SLMNAM { get; set; }
        public string TechnicianName { get; set; }
        public string CLM_FOC { get; set; }
        public string CS_No { get; set; }
        public string Log_No { get; set; }
        public string ClmtypeTech { get; set; }
        public string Clmtype { get; set; }
        public string Process_Tech{ get; set; }
        public string Process_PM{ get; set; }  
		public string TEC_ANLYS_STATUS{ get; set; }
        public string TEC_PROCESS_STATUS{ get; set; }
        public string TEC_ANLYS_AFTERPROCESS{ get; set; }
        public string PM_ANLYS_STATUS{ get; set; }
        public string PM_PROCESS_STATUS_ANLYS { get; set; }
        public string PM_ANLYS_AFTERPROCESS { get; set; }

        public string SM_NAME { get; set; }
        public string SM_APPRV_DATE { get; set; }
        public string SM_PROCESS_STATUS { get; set; }
        public string SM_REMARK { get; set; }
        public string SM_ANLYS_STATUS { get; set; }

        public string GM_NAME { get; set; }
        public string GM_APPRV_DATE { get; set; }
        public string GM_PROCESS_STATUS { get; set; }
        public string GM_REMARK { get; set; }

        public string Print_statusCN { get; set; }
        public string F_BtnApp { get; set; }
        public string IF_InvoiceNo { get; set; }
        public string CLM_Installdate { get; set; }
        public string CLM_Contact { get; set; }
        public string CLM_ContactTel { get; set; }
        public string VENDOR { get; set; }
        public string VENDORNAME { get; set; }
        public string UNIT_COST { get; set; }
        public string DEPCOD { get; set; }
        public string AMOUNT { get; set; }
        public string ClaimType { get; set; }
        public string Endprocessdate { get; set; }
        public string Sec { get; set; }
        public string InvoiceSupplier { get; set; }
        public string InvoiceDateSupplier { get; set; }
        public string Lastqtyinv { get; set; }
        public string Lastcurinv { get; set; }
        public string Lastunitcostinv { get; set; }
        public string LastAmountinv { get; set; }
        public string LastRATEinv { get; set; }
        public string Cmstyp { get; set; }
        public string CLM_COMMENT { get; set; }

        public string CLM_ID { get; set; }

        public string CUSCOD { get; set; }

        public string CLM_INVDATE { get; set; }

        public string CLM_Ref { get; set; }

        public string TECH1ANLYSSTATUDESCRIPTION { get; set; }

        public string Requestdate { get; set; }

        public string Requestdatecus { get; set; }
        public string CLM_CLAIMNOTE { get; set; }

        public string WR_Qty { get; set; }
        public string WE_Qty { get; set; }
        public string SLM_Remark { get; set; }
    }
    public class Salesreturnsupper
    {
          public string REQ_ID{ get; set; }
          public string STMP_ID{ get; set; }
          public string STMP_ID_SUB{ get; set; }
          public string STMP_COMPANY{ get; set; }
          public string STKCOD{ get; set; }
          public string STKDES{ get; set; }
          public string STKGRP{ get; set; }
          public string STMP_REQQTY{ get; set; }
          public string STMP_INVNO{ get; set; }
          public string STMP_INVDATE{ get; set; }
          public string STMP_CAUSE{ get; set; }
          public string STMP_PERFORM{ get; set; }
          public string STMP_RCVSTATUS{ get; set; }
          public string STMP_REQUESTBY{ get; set; }
          public string STMP_RCVDATE { get; set; }
          public string REQ_DATE{ get; set; }
          public string STMP_LineAMT { get; set; }
          public string CUSNAM { get; set; }
          public string CUSCOD { get; set; }
          public string SMSUP_APPRV_STATUS { get; set; }
          public string SLMCOD { get; set; }
          public string SLMNAM { get; set; }
          public string PERFORMDESCRIPTION { get; set; }
          public string RCVSTATUSDESCRIPTION { get; set; }
          public string PM_APPRV_DATE { get; set; }
          public string PM_APPRV_STATUS { get; set; }
          public string PM_PROCESS_STATUS { get; set; }
          public string PM_REMARK { get; set; }
          public string MD_APPRV_DATE { get; set; }
          public string MD_APPRV_STATUS { get; set; }
          public string MD_PROCESS_STATUS { get; set; }
          public string MD_REMARK { get; set; }
          public string STMP_STATUS { get; set; }
          public string STMP_STATUSDES { get; set; }
          public string STMP_ADMIN { get; set; }
		  public string STMP_ADMIN_REQQTY { get; set; }
          public string STMP_ADMIN_REQ_DATE { get; set; }
          public string SMSUP_APPRV_DATE { get; set; }
          public string ResonDes { get; set; }
          public string CN_No { get; set; }
          public string Statisallbill { get; set; }
    }

    public class SalesreturnMD
    {
        public string REQ_ID { get; set; }
        public string STMP_ID { get; set; }
        public string STMP_ID_SUB { get; set; }
        public string STMP_COMPANY { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string STKGRP { get; set; }
        public string STMP_REQQTY { get; set; }
        public string STMP_INVNO { get; set; }
        public string STMP_INVDATE { get; set; }
        public string STMP_CAUSE { get; set; }
        public string STMP_PERFORM { get; set; }
        public string STMP_RCVSTATUS { get; set; }
        public string STMP_REQUESTBY { get; set; }
        public string STMP_RCVDATE { get; set; }
        public string REQ_DATE { get; set; }
        public string STMP_LineAMT { get; set; }
        public string CUSNAM { get; set; }
        public string CUSCOD { get; set; }
        public string SMSUP_APPRV_STATUS { get; set; }
        public string SLMCOD { get; set; }
        public string SLMNAM { get; set; }
        public string PERFORMDESCRIPTION { get; set; }
        public string RCVSTATUSDESCRIPTION { get; set; }
        public string PM_APPRV_DATE { get; set; }
        public string PM_APPRV_STATUS { get; set; }
        public string PM_PROCESS_STATUS { get; set; }
        public string PM_REMARK { get; set; }
        public string MD_APPRV_DATE { get; set; }
        public string MD_APPRV_STATUS { get; set; }
        public string MD_PROCESS_STATUS { get; set; }
        public string MD_REMARK { get; set; }
        public string STMP_STATUS { get; set; }
        public string STMP_STATUSDES { get; set; }
        public string STMP_ADMIN { get; set; }
        public string STMP_ADMIN_REQQTY { get; set; }
        public string STMP_ADMIN_REQ_DATE { get; set; }
        public string SMSUP_APPRV_DATE { get; set; }
        public string ResonDes { get; set; }
        public string CN_No { get; set; }
        public string Statisallbill { get; set; }
    }
    public class SalesreturnsupperListDetailMD
    {
        public SalesreturnMD val { get; set; }

    }
    public class SalesreturnsupperListDetail
    {
        public Salesreturnsupper val { get; set; }

    }
    public class SalesreturnDetail
    {
        public string CUSCOD { get; set; }
        public string CUSNAM { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_DATE { get; set; }
        public string SLMCOD { get; set; }
        public string SLMNAM { get; set; }
        public string STMP_ID { get; set; }
        public string STMP_ID_SUB { get; set; }
        public string STMP_LastDocNo { get; set; }
        public string STMP_LastDocDate { get; set; }
        public string STMP_LineAMT { get; set; }
        public string STMP_COMPANY { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string STKGRP { get; set; }
        public string GRPNAM { get; set; }
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
        public string STMP_REQQTY { get; set; }
        public string STMP_QTY { get; set; }
        public string STMP_UOM { get; set; }
        public string STMP_INVNO { get; set; }
        public string STMP_INVDATE { get; set; }
        public string STMP_CAUSE { get; set; }
        public string STMP_PERFORM { get; set; }
        public string PERFORMDESCRIPTION { get; set; }
        public string STMP_RCVSTATUS { get; set; }
        public string RCVSTATUSDESCRIPTION { get; set; }
        public string STMP_REQUESTBY { get; set; }
        public string STMP_REQUESTDATE { get; set; }
        public string STMP_DATE { get; set; }
        public string STMP_ADMIN { get; set; }
        public string STMP_ADMIN_REQQTY { get; set; }
        public string STMP_ADMIN_REQ_DATE { get; set; }
        public string ADMIN_REMARK { get; set; }
        public string SMSUP_REMARK { get; set; }
        public string SMSUP_CODE { get; set; }
        public string SMSUP_APPRV_DATE { get; set; }
        public string SMSUP_APPRV_STATUS { get; set; }
        public string PM_NAME { get; set; }
        public string PM_APPRV_DATE { get; set; }
        public string PM_APPRV_STATUS { get; set; }
        public string PM_PROCESS_STATUS { get; set; }
        public string PM_REMARK { get; set; }
        public string MD_NAME { get; set; }
        public string MD_APPRV_DATE { get; set; }
        public string MD_APPRV_STATUS { get; set; }
        public string MD_PROCESS_STATUS { get; set; }
        public string MD_REMARK { get; set; }
        public string STMP_STATUS { get; set; }
        public string STMP_STATUS_CENTER { get; set; }
        public string Remake_Admin { get; set; }
        public string STMP_REASON_WH { get; set; }
    }

    public class SalesreturnDetailList
    {
        public SalesreturnDetail val { get; set; }

    }

    public class ClimeListDetail
    {
        public ClimeDetail val { get; set; }
    }

    // Class Image Files 
    public class ImageFiles
    {
       public string IMAGE_ID { get; set; }
       public string REQ_NO { get; set; }
       public string CLM_NO_SUB{ get; set; }
       public string IMAGE_NO{ get; set; }
       public string IMAGE_NAME{ get; set; }
       public string PATH { get; set; }
    }

    public class ImageFilesListDetail
    {
        public ImageFiles val { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required]
        //[EmailAddress]
        [StringLength(150)]
        [Display(Name = "User: ")]
        public string Usre { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 2)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
    }


    public class Pm
    {
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
    }

    public class PmListGetdata
    {
        public Pm val { get; set; }
    }

    public class itemimg
    {
        public string Img { get; set; }
        public string inCim_NoSub { get; set; }
        public string Cim_No { get; set; }
        public string Im_No { get; set; }
    }

    public class TechLo
    {
        public string DefineCode { get; set; }
        public string Location { get; set; }
    }

    public class TecListGetdata
    {
        public TechLo val { get; set; }
    }

     public class Item_Detail
    {
        public string Company { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string UOM { get; set; }
        public string SEC { get; set; }
        public string DEPNAM { get; set; }
        public string STKGRP { get; set; }
        public string GRPNAM { get; set; }
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
     }

     public class Item_DetailGetdata
     {
         public Item_Detail val { get; set; }
     }

    public class Receive_SupplierGetdata
    {
        public string CLM_NO_Supplier { get; set; }
        public string REQ_TotalQty { get; set; }
        public string REQ_TotalAMT { get; set; }
        public string REQ_TotalItem { get; set; }
        public string REQ_BY { get; set; }
        public string REQ_DATE { get; set; }
        public string Remake { get; set; }
        public string CLM_SUB_No { get; set; }
        public string STKCOD { get; set; }
        public string Send_Qty { get; set; }
        public string Unit_Price { get; set; }
        public string AMT { get; set; }
        public string Supplier { get; set; }
        public string STKDES { get; set; }
        public string UOM { get; set; }
        public string VENDORNAME { get; set; }
        public string INV_Sup_No { get; set; }
        public string INV_Qty_Sup { get; set; }
        public string CN_Sup_No { get; set; }
        public string CN_Qty_Sup { get; set; }
        public string CN_Amt { get; set; }
        public string INV_Qty_Sup_Cancel { get; set; }
        public string CN_Qty_Sup_Cancel { get; set; }
        public string Status { get; set; }
        public string CS_No { get; set; }
        public string Log_No { get; set; }
        public string STATUS { get; set; }
        public string STATUSName { get; set; }
        public string REQ_DATE_Clim { get; set; }
		public string Endprocessdate { get; set; }
		public string ClaimType { get; set; }
        public string CUSCOD { get; set; }
		public string ANLYS_AFTERPROCESSTEXT { get; set; }
		public string TECH2_NAME { get; set; }
		public string CUSNAM { get; set; }
		public string SLMCOD { get; set; }
        public string CLM_UOM { get; set; }
        public string SLMNAM { get; set; }
        public string InvoiceSupplier { get; set; }
        public string InvoiceDateSupplier { get; set; }
        public string CLM_QTY { get; set; }
        public string Lastqtyinv { get; set; }
        public string Lastcurinv { get; set; }
        public string Lastunitcostinv { get; set; }
        public string LastAmountinv { get; set; }
        public string LastRATEinv { get; set; }
        public string Cur_Sup { get; set; }
    }

    public class ListGetdataReceive_Supplier
    {
        public Receive_SupplierGetdata val { get; set; }
    }
    public class ClimedataRt
    {
        public string STMP_ID { get; set; }
        public string STMP_ID_SUB { get; set; }
        public string CUSCOD { get; set; }
        public string STMP_REQUESTBY { get; set; }
        public string CLM_RCVDATE { get; set; }
        public string STKCOD { get; set; }
        public string STKDES { get; set; }
        public string UOM { get; set; }
        public string STMP_INVNO { get; set; }
        public string STMP_INVDATE { get; set; }
        public string STMP_QTY { get; set; }
        public string STMP_CASE { get; set; }
        public string STMP_PERFORM { get; set; }
        public string PERFORMDESCRIPTION { get; set; }
        public string STMP_RCVSTATUS { get; set; }
        public string RCVSTATUSDESCRIPTION { get; set; }
        public string InsertDate { get; set; }
        public string COMPANY { get; set; }
        public string CUSNAM { get; set; }
        public string SLMCOD { get; set; }
        public string SLMNAM { get; set; }
        public string Status { get; set; }
        public string STKGRP { get; set; }
        public string PROD { get; set; }
        public string PRODNAM { get; set; }
        public string STMP_ReasonCodes { get; set; }
        public string Resoncodedes { get; set; }
        public string FOC { get; set; }
        public string Price { get; set; }
    }
    public class ClimeRttempListDetail
    {
        public ClimedataRt val { get; set; }
    }

    public class UsrGrp
    {
        [Key]
        public int ID { get; set; }
        public string company { get; set; }
        public string UsrID { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string SLMCOD { get; set; }
        public int UsrTyp { get; set; }
        public int ISApprover { get; set; }
        public int UsrClmStaff { get; set; }
        public int Usermail { get; set; }
    }
}
