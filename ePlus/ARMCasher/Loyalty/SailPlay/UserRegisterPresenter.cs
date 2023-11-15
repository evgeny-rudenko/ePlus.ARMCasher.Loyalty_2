// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.SailPlay.UserRegisterPresenter
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.Loyalty;
using ePlus.Loyalty.Interfaces;
using ePlus.Loyalty.SailPlay;
using System;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.SailPlay
{
  public class UserRegisterPresenter : IDisposable
  {
    private FormSailPlayUserRegister View { get; set; }

    public UserRegisterPresenter(FormSailPlayUserRegister view) => this.View = view;

    public UserInfoResult ShowView() => this.View.ShowDialog() != DialogResult.OK ? (UserInfoResult) null : this.View.UserInfo;

    public UserInfoResult ShowView(string clientId, PublicIdType clientIdType) => this.View.ShowDialog(new UserInfoResult()
    {
      ID = clientIdType == PublicIdType.CardNumber ? clientId : (string) null,
      Phone = clientIdType == PublicIdType.Phone ? "+" + clientId : (string) null,
      Sex = "1",
      AgeTag = "35-45"
    }) != DialogResult.OK ? (UserInfoResult) null : this.View.UserInfo;

    public UserInfoResult ShowView(IUserInfo userInfo) => this.View.ShowDialog((UserInfoResult) userInfo) != DialogResult.OK ? (UserInfoResult) null : this.View.UserInfo;

    public void Dispose()
    {
      if (this.View == null)
        return;
      this.View.Dispose();
    }
  }
}
