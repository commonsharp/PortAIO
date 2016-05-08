﻿using System;
using System.Collections.Generic;
using System.Linq;
using EvadeA;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using EloBuddy;

namespace YasuoPro
{
    class TargettedDanger
    {
        public class SData
        {
            internal string spellName;
            internal string championName;
            internal SpellSlot spellSlot;

            internal float delay
            {
                get
                {
                    return Helper.GetSliderFloat("enabled." + spellName + ".delay", YasuoMenu.EvadeM) + Helper.GetSliderInt("Evade.Delay", YasuoMenu.EvadeM);
                }
            }
            internal bool IsEnabled
            {
                get
                {
                    return Helper.GetBool("enabled." + spellName, YasuoMenu.EvadeM);
                }
            }
        }

        internal static List<SData> spellList = new List<SData>();

        //Credits to h3h3 for spellnames

        static TargettedDanger()
        {
            AddSpells();
        }

        static void AddSpells()
        {
            AddSpell("Syndra", "syndrar", SpellSlot.R);
            AddSpell("Veigar", "veigarprimordialburst", SpellSlot.R);
            AddSpell("Malzahar", "alzaharnethergrasp", SpellSlot.R);
            AddSpell("Caitlyn", "CaitlynAceintheHole", SpellSlot.R, 1000);
            AddSpell("Caitlyn", "CaitlynHeadshotMissile", SpellSlot.Unknown);
            AddSpell("Brand", "BrandWildfire", SpellSlot.R);
            AddSpell("Brand", "brandconflagrationmissile", SpellSlot.E);
            AddSpell("Kayle", "judicatorreckoning", SpellSlot.Q);
            AddSpell("Pantheon", "PantheonQ", SpellSlot.Q);
            AddSpell("Taric", "Dazzle", SpellSlot.Q);
            AddSpell("Viktor", "viktorpowertransfer", SpellSlot.Q);
            AddSpell("Ahri", "ahrifoxfiremissiletwo", SpellSlot.W);
            AddSpell("Elise", "EliseHumanQ", SpellSlot.Q);
            AddSpell("Shaco", "TwoShivPoison", SpellSlot.E);
            AddSpell("Urgot", "UrgotHeatseekingHomeMissile", SpellSlot.Q);
            AddSpell("Lucian", "LucianPassiveShot", SpellSlot.Unknown);
            AddSpell("Baron", "BaronAcidBall", SpellSlot.Unknown);
            AddSpell("Baron", "BaronAcidBall2", SpellSlot.Unknown);
            AddSpell("Baron", "BaronDeathBreathProj1", SpellSlot.Unknown);
            AddSpell("Baron", "BaronDeathBreathProj3", SpellSlot.Unknown);
            AddSpell("Baron", "BaronSpike", SpellSlot.Unknown);
            AddSpell("Leblanc", "LeblancChaosOrbM", SpellSlot.Q);
            AddSpell("Annie", "disintegrate", SpellSlot.Q);
            AddSpell("TwistedFate", "goldcardpreattack", SpellSlot.W);
            AddSpell("TwistedFate", "bluecardpreattack", SpellSlot.W);
            AddSpell("TwistedFate", "redcardpreattack", SpellSlot.W);
            AddSpell("Kassadin", "NullLance", SpellSlot.Q);
            AddSpell("Teemo", "BlindingDart", SpellSlot.Q);
            AddSpell("Malphite", "SeismicShard", SpellSlot.Q);
            AddSpell("Vayne", "VayneCondemn", SpellSlot.E);
            AddSpell("Nunu", "IceBlast", SpellSlot.E);
            AddSpell("Tristana", "BusterShot", SpellSlot.R);
            AddSpell("Cassiopeia", "CassiopeiaTwinFang", SpellSlot.E);
            AddSpell("Pantheon", "Pantheon_Throw", SpellSlot.Q);
            AddSpell("Akali", "AkaliMota", SpellSlot.Q);
            AddSpell("Anivia", "Frostbite", SpellSlot.E);
            AddSpell("Katarina", "KatarinaQ", SpellSlot.Q);
            AddSpell("Katarina", "KatarinaRSound", SpellSlot.R);
            AddSpell("Fiddlesticks", "FiddlesticksDarkWind", SpellSlot.E);
            AddSpell("MissFortune", "MissFortuneBulletTime", SpellSlot.R);
            AddSpell("MissFortune", "MissFortuneRicochetShot", SpellSlot.Q);
        }

        static void AddSpell(string champname, string spellname, SpellSlot slot, float del = 0)
        {
            spellList.Add(new SData { championName = champname, spellName = spellname, spellSlot = slot });
        }

        public static SData GetSpell(string spellName)
        {
            return spellList.FirstOrDefault(spell => string.Equals(spell.spellName, spellName, StringComparison.CurrentCultureIgnoreCase));
        }

        public static void OnUpdate()
        {
            foreach (var ls in DetectedPolygons)
            {
                if (YasuoEvade.TickCount - ls.StartTick >= ls.data.delay + Helper.GetSliderInt("Evade.Delay", YasuoMenu.EvadeM))
                {
                    if (ls.poly.PointInPolygon(Helper.Yasuo.ServerPosition.LSTo2D()) == 1)
                    {
                        var pos = Helper.Yasuo.ServerPosition.LSExtend(ls.argss.Target.Position, 50);
                        Helper.Spells[Helper.W].Cast(pos);
                    }
                }
            }
        }


        static List<LittleStruct> DetectedPolygons = new List<LittleStruct>();

        struct LittleStruct
        {
            public EvadeA.Geometry.Polygon poly;
            public GameObjectProcessSpellCastEventArgs argss;
            public Vector2 RealEndPos;
            public float StartTick;
            public SData data;
        }

        public static void OnDraw(EventArgs args)
        {
            foreach (var d in DetectedPolygons)
            {
                d.poly.Draw(System.Drawing.Color.Red, 5);
            }
        }

        internal static void SpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Helper.GetBool("Evade.WTS", YasuoMenu.EvadeM) || sender.IsAlly || !SpellSlot.W.IsReady() || (!Helper.GetBool("Evade.FOW", YasuoMenu.EvadeM) && !sender.IsVisible))
            {
                return;
            }
            if (args.SData.Name.Equals("MissFortuneBulletTime"))
            {
                var ssdata = GetSpell(args.SData.Name);
                if (ssdata.IsEnabled)
                {
                    var end = args.Start.LSTo2D().LSExtend(args.End.LSTo2D(), 1400);
                    EvadeA.Geometry.Rectangle rect = new EvadeA.Geometry.Rectangle(args.Start.LSTo2D(), end, args.SData.LineWidth);
                    var topoly = rect.ToPolygon();
                    var newls = new LittleStruct { poly = topoly, argss = args, RealEndPos = end, StartTick = YasuoEvade.TickCount, data = ssdata };
                    DetectedPolygons.Add(newls);
                    LeagueSharp.Common.Utility.DelayAction.Add(3000, () => DetectedPolygons.Clear());
                }
            }
            if (!args.Target.IsMe)
            {
                return;
            }
            //Console.WriteLine(args.SData.Name + " " + sender.BaseSkinName);
            var sdata = GetSpell(args.SData.Name);
            if (sdata != null && sdata.IsEnabled)
            {
                var castpos = Helper.Yasuo.ServerPosition.LSExtend(args.Start, 50);
                LeagueSharp.Common.Utility.DelayAction.Add((int)sdata.delay, () => Helper.Spells[Helper.W].Cast(castpos));
            }
        }
    }
}
