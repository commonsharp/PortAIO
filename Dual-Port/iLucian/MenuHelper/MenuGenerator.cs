using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace iLucian.MenuHelper
{
    class MenuGenerator
    {

        public static Menu comboOptions, harassOptions, laneclearOptions, miscOptions;

        public static void Generate()
        {
            Variables.Menu = MainMenu.AddMenu("iLucian", "com.ilucian");

            comboOptions = Variables.Menu.AddSubMenu(":: iLucian - Combo Options", "com.ilucian.combo");
            comboOptions.Add("com.ilucian.combo.q", new CheckBox("Use Q", true));
            comboOptions.Add("com.ilucian.combo.qExtended", new CheckBox("Use Extended Q", true));
            comboOptions.Add("com.ilucian.combo.w", new CheckBox("Use W", true));
            comboOptions.Add("com.ilucian.combo.e", new CheckBox("Use E", true));
            comboOptions.Add("com.ilucian.combo.eMode", new ComboBox("E Mode", 0, "Kite", "Side", "Cursor", "Enemy"));

            harassOptions = Variables.Menu.AddSubMenu(":: iLucian - Harass Options", "com.ilucian.harass");
            harassOptions.Add("com.ilucian.harass.q", new CheckBox("Use Q", true));
            harassOptions.Add("com.ilucian.harass.qExtended", new CheckBox("Use Extended Q", true));
            harassOptions.Add("com.ilucian.harass.w", new CheckBox("Use W", true));

            laneclearOptions = Variables.Menu.AddSubMenu(":: iLucian - Laneclear Options", "com.ilucian.laneclear");
            laneclearOptions.Add("com.ilucian.laneclear.q", new CheckBox("Use Q", true));
            laneclearOptions.Add("com.ilucian.laneclear.qMinions", new Slider("Cast Q on x minions", 3, 1, 10));

            miscOptions = Variables.Menu.AddSubMenu(":: iLucian - Misc Options", "com.ilucian.misc");
            miscOptions.Add("com.ilucian.misc.usePrediction", new CheckBox("Use W Pred", true));
            miscOptions.Add("com.ilucian.misc.gapcloser", new CheckBox("Use E For Gapcloser", true));
        }
    }
}