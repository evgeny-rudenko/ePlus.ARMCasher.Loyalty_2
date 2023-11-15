// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyLogger
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using NLog;

namespace ePlus.ARMCasher.Loyalty
{
  public static class LoyaltyLogger
  {
    private static Logger _logger;

    private static Logger Log => LoyaltyLogger._logger ?? (LoyaltyLogger._logger = LogManager.GetLogger("loyalty_log"));

    public static void Info(string message) => LoyaltyLogger.Log.Info(message);

    public static void Error(string message) => LoyaltyLogger.Log.Error(message);
  }
}
