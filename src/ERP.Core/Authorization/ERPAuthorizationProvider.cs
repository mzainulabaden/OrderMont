using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization;

public class ERPAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
			// IMS_PurchaseInvoice Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice, L("IMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Create, L("CreateIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Edit, L("EditIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Delete, L("DeleteIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_ApproveDocument, L("ApproveIMS_PurchaseInvoice"));

			// IMS_PurchaseInvoice Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice, L("IMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Create, L("CreateIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Edit, L("EditIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Delete, L("DeleteIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_ApproveDocument, L("ApproveIMS_PurchaseInvoice"));

			// IMS_PurchaseInvoice Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice, L("IMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Create, L("CreateIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Edit, L("EditIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_Delete, L("DeleteIMS_PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_IMS_PurchaseInvoice_ApproveDocument, L("ApproveIMS_PurchaseInvoice"));

			// IMS_Item Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_Item, L("IMS_Item"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Item_Create, L("CreateIMS_Item"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Item_Edit, L("EditIMS_Item"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Item_Delete, L("DeleteIMS_Item"));

			// IMS_Vendor Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_Vendor, L("IMS_Vendor"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Vendor_Create, L("CreateIMS_Vendor"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Vendor_Edit, L("EditIMS_Vendor"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Vendor_Delete, L("DeleteIMS_Vendor"));

			// IMS_Location Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_Location, L("IMS_Location"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Location_Create, L("CreateIMS_Location"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Location_Edit, L("EditIMS_Location"));
			context.CreatePermission(PermissionNames.LookUps_IMS_Location_Delete, L("DeleteIMS_Location"));

			// IMS_ItemCategory Permission
			context.CreatePermission(PermissionNames.LookUps_IMS_ItemCategory, L("IMS_ItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_IMS_ItemCategory_Create, L("CreateIMS_ItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_IMS_ItemCategory_Edit, L("EditIMS_ItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_IMS_ItemCategory_Delete, L("DeleteIMS_ItemCategory"));

			

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
