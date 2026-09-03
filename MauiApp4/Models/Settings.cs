namespace MauiApp4.Models;

public static class Settings
{
    public const string DefaultTipPctKey = "DefaultTipPct";

    public static double GetDefaultTipPct()
    {
        return Preferences.Get(DefaultTipPctKey, 15.0);
    }

    public static void SetDefaultTipPct(double value)
    {
        Preferences.Set(DefaultTipPctKey, value);
    }
}
