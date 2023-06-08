using KitchenData;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace KitchenInferno.Utils
{
    public static class UnlockHelpers
    {
        public static List<(Locale, UnlockInfo)> CopyInfo(int unlockID)
        {
            GameDataObject gdo = GDOUtils.GetExistingGDO(unlockID);
            if (gdo == null || !(gdo is Unlock unlock))
                return new List<(Locale, UnlockInfo)>();
            return unlock.Info.GetLocales().Select(locale => (locale, unlock.Info.Get(locale))).ToList();
        }
    }
}
