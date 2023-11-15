// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Properties.Resources
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ePlus.ARMCasher.Loyalty.Properties
{
  [DebuggerNonUserCode]
  [CompilerGenerated]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) ePlus.ARMCasher.Loyalty.Properties.Resources.resourceMan, (object) null))
          ePlus.ARMCasher.Loyalty.Properties.Resources.resourceMan = new ResourceManager("ePlus.ARMCasher.Loyalty.Properties.Resources", typeof (ePlus.ARMCasher.Loyalty.Properties.Resources).Assembly);
        return ePlus.ARMCasher.Loyalty.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture;
      set => ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture = value;
    }

    internal static string CustomerProcessingStatus_NotFound => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (CustomerProcessingStatus_NotFound), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_AlreadyBoundToAnotherCustomer => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_AlreadyBoundToAnotherCustomer), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_AlreadyBoundToCurrentCustmer => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_AlreadyBoundToCurrentCustmer), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_Bound => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_Bound), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_DiscountCardIsBlocked => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_DiscountCardIsBlocked), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_NotFound => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_NotFound), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string DiscountCardProcessingStatus_NotProcessed => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (DiscountCardProcessingStatus_NotProcessed), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxCardStatus_Activated => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxCardStatus_Activated), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxCardStatus_Blocked => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxCardStatus_Blocked), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxCardStatus_Inactive => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxCardStatus_Inactive), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxCardStatus_Issued => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxCardStatus_Issued), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxCardStatus_NotIssued => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxCardStatus_NotIssued), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxDiscountName_balance => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxDiscountName_balance), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static string MindboxDiscountName_promoAction => ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetString(nameof (MindboxDiscountName_promoAction), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);

    internal static Bitmap UserRegForm => (Bitmap) ePlus.ARMCasher.Loyalty.Properties.Resources.ResourceManager.GetObject(nameof (UserRegForm), ePlus.ARMCasher.Loyalty.Properties.Resources.resourceCulture);
  }
}
