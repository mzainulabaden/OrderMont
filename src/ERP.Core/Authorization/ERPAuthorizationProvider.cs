using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization;

public class ERPAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
			// DemandBook Permission
			context.CreatePermission(PermissionNames.LookUps_DemandBook, L("DemandBook"));
			context.CreatePermission(PermissionNames.LookUps_DemandBook_Create, L("CreateDemandBook"));
			context.CreatePermission(PermissionNames.LookUps_DemandBook_Edit, L("EditDemandBook"));
			context.CreatePermission(PermissionNames.LookUps_DemandBook_Delete, L("DeleteDemandBook"));

		;

		// FINANCE_ProfitLoseNote Permission
		context.CreatePermission(PermissionNames.LookUps_FINANCE_ProfitLoseNote, L("FINANCE_ProfitLoseNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Create, L("CreateFINANCE_ProfitLoseNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Edit, L("EditFINANCE_ProfitLoseNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_ProfitLoseNote_Delete, L("DeleteFINANCE_ProfitLoseNote"));

		// DayBook Permission
		context.CreatePermission(PermissionNames.LookUps_DayBook, L("DayBook"));
		context.CreatePermission(PermissionNames.LookUps_DayBook_Create, L("CreateDayBook"));
		context.CreatePermission(PermissionNames.LookUps_DayBook_Edit, L("EditDayBook"));
		context.CreatePermission(PermissionNames.LookUps_DayBook_Delete, L("DeleteDayBook"));

		// FINANCE_AccountGroups Permission
		context.CreatePermission(PermissionNames.LookUps_FINANCE_AccountGroups, L("FINANCE_AccountGroups"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_AccountGroups_Create, L("CreateFINANCE_AccountGroups"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_AccountGroups_Edit, L("EditFINANCE_AccountGroups"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_AccountGroups_Delete, L("DeleteFINANCE_AccountGroups"));

        // Universal Permission
        context.CreatePermission(PermissionNames.UNIVERSAL_Dashboard, L("UniversalDashboard"));
        context.CreatePermission(PermissionNames.UNIVERSAL_Products, L("UniversalProducts"));
        context.CreatePermission(PermissionNames.UNIVERSAL_GeneralSetups, L("UniversalGeneralSetups"));
        context.CreatePermission(PermissionNames.UNIVERSAL_HRMSetups, L("UniversalHRMSetups"));
        context.CreatePermission(PermissionNames.UNIVERSAL_EmployeeManage, L("UniversalEmployeeManage"));
        context.CreatePermission(PermissionNames.UNIVERSAL_ChartOfAccount, L("UniversalChartOfAccount"));
        context.CreatePermission(PermissionNames.UNIVERSAL_PurchaseManagement, L("UniversalPurchaseManagement"));
        context.CreatePermission(PermissionNames.UNIVERSAL_SalesManagement, L("UniversalSalesManagement"));
        context.CreatePermission(PermissionNames.UNIVERSAL_Finance, L("UniversalFinance"));
        context.CreatePermission(PermissionNames.UNIVERSAL_UserManagement, L("UniversalUserManagement"));
        context.CreatePermission(PermissionNames.UNIVERSAL_Reports, L("UniversalReports"));

        // FINANCE_GeneralNote Permission
        context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralNote, L("FINANCE_GeneralNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralNote_Create, L("CreateFINANCE_GeneralNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralNote_Edit, L("EditFINANCE_GeneralNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralNote_Delete, L("DeleteFINANCE_GeneralNote"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralNote_ApproveDocument, L("ApproveFINANCE_GeneralNote"));

		// HRM_Todo Permission
		context.CreatePermission(PermissionNames.LookUps_HRM_Todo, L("Todo"));
		context.CreatePermission(PermissionNames.LookUps_HRM_Todo_Create, L("Create_Todo"));
		context.CreatePermission(PermissionNames.LookUps_HRM_Todo_Edit, L("Edit_Todo"));
		context.CreatePermission(PermissionNames.LookUps_HRM_Todo_Update, L("Update_Todo"));
		context.CreatePermission(PermissionNames.LookUps_HRM_Todo_Delete, L("Delete_Todo"));

		// FINANCE_JournalVoucher Permission
		context.CreatePermission(PermissionNames.LookUps_FINANCE_JournalVoucher, L("FINANCE_JournalVoucher"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_JournalVoucher_Create, L("CreateFINANCE_JournalVoucher"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_JournalVoucher_Edit, L("EditFINANCE_JournalVoucher"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_JournalVoucher_Delete, L("DeleteFINANCE_JournalVoucher"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_JournalVoucher_ApproveDocument, L("ApproveFINANCE_JournalVoucher"));

		// IMS_WarehouseStockAdjustment Permission
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockAdjustment, L("IMS_WarehouseStockAdjustment"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Create, L("CreateIMS_WarehouseStockAdjustment"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Edit, L("EditIMS_WarehouseStockAdjustment"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_Delete, L("DeleteIMS_WarehouseStockAdjustment"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockAdjustment_ApproveDocument, L("ApproveIMS_WarehouseStockAdjustment"));

		// IMS_WarehouseStockLedger Permission
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockLedger, L("IMS_WarehouseStockLedger"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockLedger_Create, L("CreateIMS_WarehouseStockLedger"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockLedger_Edit, L("EditIMS_WarehouseStockLedger"));
		context.CreatePermission(PermissionNames.LookUps_IMS_WarehouseStockLedger_Delete, L("DeleteIMS_WarehouseStockLedger"));


        // FINANCE_DefaultIntegrations Permission
        context.CreatePermission(PermissionNames.LookUps_FINANCE_DefaultIntegrations, L("FINANCE_DefaultIntegrations"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Create, L("CreateFINANCE_DefaultIntegrations"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Edit, L("EditFINANCE_DefaultIntegrations"));
	    context.CreatePermission(PermissionNames.LookUps_FINANCE_DefaultIntegrations_Delete, L("DeleteFINANCE_DefaultIntegrations"));

		// CompanyProfile Permission
		context.CreatePermission(PermissionNames.LookUps_CompanyProfile, L("CompanyProfile"));
		context.CreatePermission(PermissionNames.LookUps_CompanyProfile_Create, L("CreateCompanyProfile"));
		context.CreatePermission(PermissionNames.LookUps_CompanyProfile_Edit, L("EditCompanyProfile"));
		context.CreatePermission(PermissionNames.LookUps_CompanyProfile_Update, L("UpdateCompanyProfile"));
		context.CreatePermission(PermissionNames.LookUps_CompanyProfile_Delete, L("DeleteCompanyProfile"));

		// FINANCE_GeneralPayment Permission
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralPayment, L("FINANCE_GeneralPayment"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralPayment_Create, L("CreateFINANCE_GeneralPayment"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralPayment_Edit, L("EditFINANCE_GeneralPayment"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralPayment_Delete, L("DeleteFINANCE_GeneralPayment"));
		context.CreatePermission(PermissionNames.LookUps_FINANCE_GeneralPayment_ApproveDocument, L("ApproveFINANCE_GeneralPayment"));


        // SalesReturn Permission
        context.CreatePermission(PermissionNames.LookUps_SalesReturn, L("SalesReturn"));
        context.CreatePermission(PermissionNames.LookUps_SalesReturn_Create, L("CreateSalesReturn"));
        context.CreatePermission(PermissionNames.LookUps_SalesReturn_Update, L("UpdateSalesReturn"));
        context.CreatePermission(PermissionNames.LookUps_SalesReturn_Delete, L("DeleteSalesReturn"));
        context.CreatePermission(PermissionNames.LookUps_SalesReturn_ApproveDocument, L("ApproveSalesReturn"));

        // SalesInvoice Permission
        context.CreatePermission(PermissionNames.LookUps_SalesInvoice, L("SalesInvoice"));
        context.CreatePermission(PermissionNames.LookUps_SalesInvoice_Create, L("CreateSalesInvoice"));
        context.CreatePermission(PermissionNames.LookUps_SalesInvoice_Update, L("UpdateSalesInvoice"));
        context.CreatePermission(PermissionNames.LookUps_SalesInvoice_Delete, L("DeleteSalesInvoice"));
        context.CreatePermission(PermissionNames.LookUps_SalesInvoice_ApproveDocument, L("ApproveSalesInvoice"));

        // SalesOrder Permission
        context.CreatePermission(PermissionNames.LookUps_SalesOrder, L("SalesOrder"));
        context.CreatePermission(PermissionNames.LookUps_SalesOrder_Create, L("CreateSalesOrder"));
        context.CreatePermission(PermissionNames.LookUps_SalesOrder_Update, L("UpdateSalesOrder"));
        context.CreatePermission(PermissionNames.LookUps_SalesOrder_Delete, L("DeleteSalesOrder"));
        context.CreatePermission(PermissionNames.LookUps_SalesOrder_ApproveDocument, L("ApproveSalesOrder"));

        // SalesTracking Permission
        context.CreatePermission(PermissionNames.LookUps_SalesTracking, L("SalesTracking"));

        // PurchaseReturn Permission
        context.CreatePermission(PermissionNames.LookUps_PurchaseReturn, L("PurchaseReturn"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseReturn_Create, L("CreatePurchaseReturn"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseReturn_Update, L("UpdatePurchaseReturn"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseReturn_Delete, L("DeletePurchaseReturn"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseReturn_ApproveDocument, L("ApprovePurchaseReturn"));

        // Warehouse Permission
        context.CreatePermission(PermissionNames.LookUps_Warehouse, L("Warehouse"));
        context.CreatePermission(PermissionNames.LookUps_Warehouse_Create, L("CreateWarehouse"));
        context.CreatePermission(PermissionNames.LookUps_Warehouse_Update, L("UpdateWarehouse"));
        context.CreatePermission(PermissionNames.LookUps_Warehouse_Delete, L("DeleteWarehouse"));

        // PurchaseInvoice Permission
        context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice, L("PurchaseInvoice"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Create, L("CreatePurchaseInvoice"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Update, L("UpdatePurchaseInvoice"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Delete, L("DeletePurchaseInvoice"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_ApproveDocument, L("ApprovePurchaseInvoice"));

        // PurchaseOrder Permission
        context.CreatePermission(PermissionNames.LookUps_PurchaseOrder, L("PurchaseOrder"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseOrder_Create, L("CreatePurchaseOrder"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseOrder_Update, L("UpdatePurchaseOrder"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseOrder_Delete, L("DeletePurchaseOrder"));
        context.CreatePermission(PermissionNames.LookUps_PurchaseOrder_ApproveDocument, L("ApprovePurchaseOrder"));

        // PaymentMode Permission
        context.CreatePermission(PermissionNames.LookUps_PaymentMode, L("PaymentMode"));
        context.CreatePermission(PermissionNames.LookUps_PaymentMode_Create, L("CreatePaymentMode"));
        context.CreatePermission(PermissionNames.LookUps_PaymentMode_Update, L("UpdatePaymentMode"));
        context.CreatePermission(PermissionNames.LookUps_PaymentMode_Delete, L("DeletePaymentMode"));
        context.CreatePermission(PermissionNames.LookUps_PaymentMode_ApproveDocument, L("ApprovePaymentMode"));

        // Item Permission
        context.CreatePermission(PermissionNames.LookUps_Item, L("Item"));
        context.CreatePermission(PermissionNames.LookUps_Item_Create, L("CreateItem"));
        context.CreatePermission(PermissionNames.LookUps_Item_Update, L("UpdateItem"));
        context.CreatePermission(PermissionNames.LookUps_Item_Delete, L("DeleteItem"));

        // ItemCategory Permission
        context.CreatePermission(PermissionNames.LookUps_ItemCategory, L("ItemCategory"));
        context.CreatePermission(PermissionNames.LookUps_ItemCategory_Create, L("CreateItemCategory"));
        context.CreatePermission(PermissionNames.LookUps_ItemCategory_Update, L("UpdateItemCategory"));
        context.CreatePermission(PermissionNames.LookUps_ItemCategory_Delete, L("DeleteItemCategory"));

        // COALevel04 Permission
        context.CreatePermission(PermissionNames.LookUps_COALevel04, L("COALevel04"));
        context.CreatePermission(PermissionNames.LookUps_COALevel04_Create, L("CreateCOALevel04"));
        context.CreatePermission(PermissionNames.LookUps_COALevel04_Update, L("UpdateCOALevel04"));
        context.CreatePermission(PermissionNames.LookUps_COALevel04_Delete, L("DeleteCOALevel04"));

        // COALevel03 Permission
        context.CreatePermission(PermissionNames.LookUps_COALevel03, L("COALevel03"));
        context.CreatePermission(PermissionNames.LookUps_COALevel03_Create, L("CreateCOALevel03"));
        context.CreatePermission(PermissionNames.LookUps_COALevel03_Update, L("UpdateCOALevel03"));
        context.CreatePermission(PermissionNames.LookUps_COALevel03_Delete, L("DeleteCOALevel03"));

        // COALevel02 Permission
        context.CreatePermission(PermissionNames.LookUps_COALevel02, L("COALevel02"));
        context.CreatePermission(PermissionNames.LookUps_COALevel02_Create, L("CreateCOALevel02"));
        context.CreatePermission(PermissionNames.LookUps_COALevel02_Update, L("UpdateCOALevel02"));
        context.CreatePermission(PermissionNames.LookUps_COALevel02_Delete, L("DeleteCOALevel02"));

        // COALevel01 Permission
        context.CreatePermission(PermissionNames.LookUps_COALevel01, L("COALevel01"));
        context.CreatePermission(PermissionNames.LookUps_COALevel01_Create, L("CreateCOALevel01"));
        context.CreatePermission(PermissionNames.LookUps_COALevel01_Update, L("UpdateCOALevel01"));
        context.CreatePermission(PermissionNames.LookUps_COALevel01_Delete, L("DeleteCOALevel01"));

        // AccountType Permission
        context.CreatePermission(PermissionNames.LookUps_AccountType, L("AccountType"));
        context.CreatePermission(PermissionNames.LookUps_AccountType_Create, L("CreateAccountType"));
        context.CreatePermission(PermissionNames.LookUps_AccountType_Update, L("UpdateAccountType"));
        context.CreatePermission(PermissionNames.LookUps_AccountType_Delete, L("DeleteAccountType"));

        // Currency Permission
        context.CreatePermission(PermissionNames.LookUps_Currency, L("Currency"));
        context.CreatePermission(PermissionNames.LookUps_Currency_Create, L("CreateCurrency"));
        context.CreatePermission(PermissionNames.LookUps_Currency_Update, L("UpdateCurrency"));
        context.CreatePermission(PermissionNames.LookUps_Currency_Delete, L("DeleteCurrency"));

        // LinkWith Permission
        context.CreatePermission(PermissionNames.LookUps_LinkWith, L("LinkWith"));
        context.CreatePermission(PermissionNames.LookUps_LinkWith_Create, L("CreateLinkWith"));
        context.CreatePermission(PermissionNames.LookUps_LinkWith_Update, L("UpdateLinkWith"));
        context.CreatePermission(PermissionNames.LookUps_LinkWith_Delete, L("DeleteLinkWith"));

        // Attendance Permission
        context.CreatePermission(PermissionNames.LookUps_Attendance, L("Attendance"));
        context.CreatePermission(PermissionNames.LookUps_Attendance_Submit, L("SubmitAttendance"));

        // Designation Permission
        context.CreatePermission(PermissionNames.LookUps_Designation, L("Designation"));
        context.CreatePermission(PermissionNames.LookUps_Designation_Create, L("CreateDesignation"));
        context.CreatePermission(PermissionNames.LookUps_Designation_Update, L("UpdateDesignation"));
        context.CreatePermission(PermissionNames.LookUps_Designation_Delete, L("DeleteDesignation"));

        // Employee Permission
        context.CreatePermission(PermissionNames.LookUps_Employee, L("Employee"));
        context.CreatePermission(PermissionNames.LookUps_Employee_Create, L("CreateEmployee"));
        context.CreatePermission(PermissionNames.LookUps_Employee_Update, L("UpdateEmployee"));
        context.CreatePermission(PermissionNames.LookUps_Employee_Delete, L("DeleteEmployee"));

        // EmployeeSalary Permission
        context.CreatePermission(PermissionNames.LookUps_EmployeeSalary, L("EmployeeSalary"));
        context.CreatePermission(PermissionNames.LookUps_EmployeeSalary_Create, L("CreateEmployeeSalary"));
        context.CreatePermission(PermissionNames.LookUps_EmployeeSalary_Update, L("UpdateEmployeeSalary"));
        context.CreatePermission(PermissionNames.LookUps_EmployeeSalary_Delete, L("DeleteEmployeeSalary"));
        context.CreatePermission(PermissionNames.LookUps_EmployeeSalary_ApproveDocument, L("ApproveEmployeeSalary"));

        // GazettedHoliday Permission
        context.CreatePermission(PermissionNames.LookUps_GazettedHoliday, L("GazettedHoliday"));
        context.CreatePermission(PermissionNames.LookUps_GazettedHoliday_Create, L("CreateGazettedHoliday"));
        context.CreatePermission(PermissionNames.LookUps_GazettedHoliday_Update, L("UpdateGazettedHoliday"));
        context.CreatePermission(PermissionNames.LookUps_GazettedHoliday_Delete, L("DeleteGazettedHoliday"));

        // Unit Permission
        context.CreatePermission(PermissionNames.LookUps_Unit, L("Unit"));
        context.CreatePermission(PermissionNames.LookUps_Unit_Create, L("CreateUnit"));
        context.CreatePermission(PermissionNames.LookUps_Unit_Update, L("UpdateUnit"));
        context.CreatePermission(PermissionNames.LookUps_Unit_Delete, L("DeleteUnit"));

        // Users & Roles
        context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
        context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
        context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
    }

    private static ILocalizableString L(string name)
    {
        return new LocalizableString(name, ERPConsts.LocalizationSourceName);
    }
}
