using System;
using System.Linq;
using EloBuddy.SDK;
using ExorAIO.Utilities;
using LeagueSharp.Common;
using EloBuddy;

namespace ExorAIO.Champions.Olaf
{
    /// <summary>
    ///     The logics class.
    /// </summary>
    internal partial class Logics
    {
        /// <summary>
        ///     Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void Killsteal(EventArgs args)
        {
            /// <summary>
            ///     The KillSteal Q Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                Variables.getCheckBoxItem(Variables.QMenu, "qspell.ks"))
            {
                foreach (var target in HeroManager.Enemies.Where(t => !Bools.IsSpellShielded(t) && t.IsValidTarget(Variables.Q.Range) && !t.IsValidTarget(Variables.AARange) && t.Health < Variables.Q.GetDamage(t)))
                {
                    var castPosition = Targets.Target.Position.Extend(ObjectManager.Player.Position, -100);
                    var castPosition2 = Targets.Target.Position.Extend(ObjectManager.Player.Position, -70);

                    if (ObjectManager.Player.LSDistance(Targets.Target.ServerPosition) >= 300)
                    {
                        Variables.Q.Cast(castPosition);
                    }
                    else
                    {
                        Variables.Q.Cast(castPosition2);
                    }
                }
            }
        }
    }
}