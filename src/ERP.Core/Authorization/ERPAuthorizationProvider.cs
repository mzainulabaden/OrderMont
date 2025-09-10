using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace ERP.Authorization;

public class ERPAuthorizationProvider : AuthorizationProvider
{
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
			// PurchaseInvoice Permission
			context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice, L("PurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Create, L("CreatePurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Edit, L("EditPurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_Delete, L("DeletePurchaseInvoice"));
			context.CreatePermission(PermissionNames.LookUps_PurchaseInvoice_ApproveDocument, L("ApprovePurchaseInvoice"));


			// Item Permission
			context.CreatePermission(PermissionNames.LookUps_Item, L("Item"));
			context.CreatePermission(PermissionNames.LookUps_Item_Create, L("CreateItem"));
			context.CreatePermission(PermissionNames.LookUps_Item_Edit, L("EditItem"));
			context.CreatePermission(PermissionNames.LookUps_Item_Delete, L("DeleteItem"));

			// Vendor Permission
			context.CreatePermission(PermissionNames.LookUps_Vendor, L("Vendor"));
			context.CreatePermission(PermissionNames.LookUps_Vendor_Create, L("CreateVendor"));
			context.CreatePermission(PermissionNames.LookUps_Vendor_Edit, L("EditVendor"));
			context.CreatePermission(PermissionNames.LookUps_Vendor_Delete, L("DeleteVendor"));

			// Location Permission
			context.CreatePermission(PermissionNames.LookUps_Location, L("Location"));
			context.CreatePermission(PermissionNames.LookUps_Location_Create, L("CreateLocation"));
			context.CreatePermission(PermissionNames.LookUps_Location_Edit, L("EditLocation"));
			context.CreatePermission(PermissionNames.LookUps_Location_Delete, L("DeleteLocation"));

			// ItemCategory Permission
			context.CreatePermission(PermissionNames.LookUps_ItemCategory, L("ItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_ItemCategory_Create, L("CreateItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_ItemCategory_Edit, L("EditItemCategory"));
			context.CreatePermission(PermissionNames.LookUps_ItemCategory_Delete, L("DeleteItemCategory"));

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
