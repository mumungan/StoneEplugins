// Special thanks to Gigi and SeaDragon. This Plugin bases on Gigi's EliteBarPlugin and SeaDragon(glq)' MonstersCountPlugin
//Gigi's EliteBarPlugin
//https://www.ownedcore.com/forums/diablo-3/turbohud/turbohud-approved-plugins/612897-english-gigi-elitebarplugin.html
//https://github.com/d3gigi/plugins/blob/master/Gigi/EliteBarPlugin.cs
// SeaDragon(glq)' MonstersCountPlugin
//https://www.ownedcore.com/forums/diablo-3/turbohud/turbohud-approved-plugins/612942-international-glq-monsterscountplugin.html
//https://pastebin.com/Jm1V3cvk

using System.Linq;
using System;
using System.Collections.Generic;
using Turbo.Plugins.Default;
using System.Text;

namespace Turbo.Plugins.Stone
{
    public class SummonerMonsterBarCountPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection Decorator { get; set; }
        public IFont DefaultTextFont { get; set; }
        public IFont LightFont { get; set; }
        public IFont RedFont { get; set; }
        public IFont NameFont { get; set; }
        public IBrush BackgroundBrush { get; set; }
        public IBrush BorderBrush { get; set; }
        public IBrush RareBrush { get; set; }
        public IBrush RareJuggerBrush { get; set; }
        public IBrush RareMinionBrush { get; set; }
        public IBrush ChampionBrush { get; set; }
        public IBrush BossBrush { get; set; }
        public bool JuggernautHighlight { get; set; }
        public bool ShowMonsterType { get; set; }
        public bool ShowMeScreenBaseYard { get; set; }
        public bool ShowMeScreenMaxYard { get; set; }
        public bool ShowSummonerCount { get; set; }
        public bool ShowSummonerEliteBar { get; set; }
        public bool ShowSummonerNormalMonsterBar { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float XScaling { get; set; }
        public float YScaling { get; set; }
        public int BaseYard { get; set; }
        public int MaxYard { get; set; }
        public string PercentageDescriptor { get; set; }
        public Dictionary<MonsterAffix, string> DisplayAffix;
        private StringBuilder textBuilder;
        private Dictionary<string, string> SummonerMonsternames1 = new Dictionary<string, string>();
        private Dictionary<string, string> SummonerMonsternames2 = new Dictionary<string, string>();

        private float px, py, h, w2;

        public SummonerMonsterBarCountPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            ShowMeScreenBaseYard = true;    // 1. MeScreenBaseYard on, off
            ShowMeScreenMaxYard = true;     // 2. MeScreenMaxYard on, off
            ShowSummonerCount = true;       // 3. SummonerCount on, off
            ShowSummonerEliteBar = true;    // 4. EliteSummonerBar on, off
            ShowSummonerNormalMonsterBar = true; //5.NormalSummonerBar on, off

            JuggernautHighlight = true;
            ShowMonsterType = true;
            XScaling = 0.7f;
            YScaling = 1.2f;
            PercentageDescriptor = "0";
            XPos = Hud.Window.Size.Width * 0.73f;
            YPos = Hud.Window.Size.Height * 0.05f;
            BaseYard = 40;
            MaxYard = 120;
            DisplayAffix = new Dictionary<MonsterAffix, string>();
            SummonerMonsternames1.Add("Retching Cadaver", "6646");
            SummonerMonsternames1.Add("Deathspitter", "6638");
            SummonerMonsternames1.Add("Spewing Horror", "6640");
            SummonerMonsternames1.Add("Dust Retcher", "6641");

            SummonerMonsternames2.Add("Tomb Guardian", "450352");
            SummonerMonsternames2.Add("Vengeful Summoner", "5390");
            SummonerMonsternames2.Add("Returned Summoner", "5388");
            SummonerMonsternames2.Add("Tortured Summoner", "329319");

            textBuilder = new StringBuilder();

            Decorator = new WorldDecoratorCollection(
                new MapShapeDecorator(Hud)
                {
                Brush = Hud.Render.CreateBrush(180, 255, 50, 50, 0),
                ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = 2,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 200, 50, 50, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 6.5f, 255, 255, 255, 255, false, false, false),
                }
                );


            //Colorization
            DefaultTextFont = Hud.Render.CreateFont("tahoma", 11, 255, 255, 128, 0, false, false, 250, 0, 0, 0, true);
            LightFont = Hud.Render.CreateFont("tahoma", 7f, 128, 255, 255, 255, true, false, true);
            RedFont = Hud.Render.CreateFont("tahoma", 7f, 255, 255, 0, 0, true, false, true);
            NameFont = Hud.Render.CreateFont("tahoma", 7f, 255, 255, 255, 255, true, false, true);
            BackgroundBrush = Hud.Render.CreateBrush(255, 125, 120, 120, 0);
            BorderBrush = Hud.Render.CreateBrush(0, 255, 255, 255, 1);
            RareBrush = Hud.Render.CreateBrush(255, 255, 128, 0, 0);
            RareJuggerBrush = Hud.Render.CreateBrush(255, 255, 0, 0, 0);
            RareMinionBrush = Hud.Render.CreateBrush(204, 200, 100, 0, 0);
            ChampionBrush = Hud.Render.CreateBrush(255, 0, 128, 255, 0);
            BossBrush = Hud.Render.CreateBrush(255, 200, 20, 0, 0);

        }

        private void DrawHealthBar(WorldLayer layer, IMonster m, ref float yref)
        {
            var w = m.CurHealth * w2 / m.MaxHealth;
            var per = LightFont.GetTextLayout((m.CurHealth * 100 / m.MaxHealth).ToString(PercentageDescriptor) + "%");
            var y = YPos + py * 8 * yref;
            IBrush cBrush = null;
            IFont cFont = null;

            //Brush selection
            switch (m.Rarity)
            {
                case ActorRarity.Boss:
                    cBrush = BossBrush;
                    break;
                case ActorRarity.Champion:
                    cBrush = ChampionBrush;
                    break;
                case ActorRarity.Rare:
                    cBrush = RareBrush;
                    break;
                case ActorRarity.RareMinion:
                    cBrush = RareMinionBrush;
                    break;
                default:
                    cBrush = BackgroundBrush;
                    break;
            }

            //Jugger Highlight
            if (JuggernautHighlight && m.Rarity == ActorRarity.Rare && HasAffix(m, MonsterAffix.Juggernaut))
            {
                cFont = RedFont;
                cBrush = RareJuggerBrush;
            }
            else
                cFont = NameFont;


            //Draw Rectangles
            BackgroundBrush.DrawRectangle(XPos, y, w2, h);
            BorderBrush.DrawRectangle(XPos, y, w2, h);
            cBrush.DrawRectangle(XPos, y, (float)w, h);
            LightFont.DrawText(per, XPos + 8 + w2, y - py);

            //Draw MonsterType
            if (ShowMonsterType)
            {
                var name = cFont.GetTextLayout(m.SnoMonster.NameLocalized);
                cFont.DrawText(name, XPos + 3, y - py);
            }

            //increase linecount
            yref += 1.0f;
        }

        private void DrawPack(WorldLayer layer, IMonsterPack p, ref float yref)
        {
            //Check if any affixes are wished to be displayed
            if (DisplayAffix.Any())
            {
                string dAffix = "";
                foreach (ISnoMonsterAffix afx in p.AffixSnoList)
                {        //iterate affix list
                    if (DisplayAffix.Keys.Contains(afx.Affix))          //if affix is an key
                        dAffix += DisplayAffix[afx.Affix] + " ";        //add to output
                }
                if (!string.IsNullOrEmpty(dAffix))
                {
                    var d = LightFont.GetTextLayout(dAffix);
                    var y = YPos + py * 8 * yref;
                    LightFont.DrawText(d, XPos, y - py);
                    yref += 1.0f;
                }
            }
            //iterate all alive monsters of pack and print healthbars
            foreach (IMonster m in p.MonstersAlive)
                DrawHealthBar(layer, m, ref yref);
        }

        private bool HasAffix(IMonster m, MonsterAffix afx)
        {
            return m.AffixSnoList.Any(a => a.Affix == afx);
        }

        public void PaintWorld(WorldLayer layer)
        {
            px = Hud.Window.Size.Width * 0.00155f * XScaling;
            py = Hud.Window.Size.Height * 0.001667f * YScaling;
            h = py * 6;
            w2 = px * 60;
            float yref = 0.01f;
            int summonerelite1count = 0;
            int summonerelite2count = 0;
            int summonedelite1count = 0;
            int summonedelite2count = 0;
            int summonedcount = 0;
            int summoner1count = 0;
            int summoner2count = 0;
            int monstersCountBaseYard = 0;
            int monstersCountMaxYard = 0;

            if (ShowMeScreenBaseYard)
            {
                var monsters1 = Hud.Game.AliveMonsters.Where(m => ((m.SummonerAcdDynamicId == 0 && m.IsElite) || !m.IsElite) && m.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= BaseYard);
                foreach (var monster in monsters1)
                {
                    monstersCountBaseYard++;
                }
                var text1 = string.Format("{0} Yard : {1}", BaseYard, monstersCountBaseYard);
                var layer1 = DefaultTextFont.GetTextLayout(text1);
                DefaultTextFont.DrawText(layer1, Hud.Window.Size.Width * 0.34f, Hud.Window.Size.Height * 0.22f);
            }
            if (ShowMeScreenMaxYard)
            {

                var monsters2 = Hud.Game.AliveMonsters.Where(m => ((m.SummonerAcdDynamicId == 0 && m.IsElite) || !m.IsElite) && m.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= MaxYard);
                foreach (var monster in monsters2)
                {
                    monstersCountMaxYard++;
                }
                var text2 = string.Format("{0} Yard : {1}", MaxYard, monstersCountMaxYard);
                var layer2 = DefaultTextFont.GetTextLayout(text2);
                DefaultTextFont.DrawText(layer2, Hud.Window.Size.Width * 0.34f, Hud.Window.Size.Height * 0.24f);
            }

            var monsters = Hud.Game.AliveMonsters;
            foreach (var monster in monsters)
            {
                if (monster.IsElite)
                {
                    if (SummonerMonsternames1.ContainsKey(monster.SnoMonster.NameEnglish) || SummonerMonsternames1.ContainsKey(monster.SnoMonster.NameLocalized))
                    {
                        if (monster.SummonerAcdDynamicId == 0) summonerelite1count++;
                        else summonedelite1count++;
                        if (ShowSummonerEliteBar)
                        {
                            DrawHealthBar(layer, monster, ref yref);
                            yref += 0.5f;
                        }
                        Decorator.Paint(layer, monster, monster.FloorCoordinate, "E" + monster.SnoMonster.NameLocalized);
                    }
                    if (SummonerMonsternames2.ContainsKey(monster.SnoMonster.NameEnglish) || SummonerMonsternames2.ContainsKey(monster.SnoMonster.NameLocalized))
                    {
                        if (monster.SummonerAcdDynamicId == 0) summonerelite2count++;
                        else summonedelite2count++;
                        if (ShowSummonerEliteBar)
                        {
                            DrawHealthBar(layer, monster, ref yref);
                            yref += 0.5f;
                        }
                        Decorator.Paint(layer, monster, monster.FloorCoordinate, "E" + monster.SnoMonster.NameLocalized);
                    }
                }
            }
            foreach (var monster in monsters)
            {
                if (!monster.IsElite)
                {
                    if (monster.SummonerAcdDynamicId != 0) summonedcount++;
                    if (SummonerMonsternames1.ContainsKey(monster.SnoMonster.NameEnglish) || SummonerMonsternames1.ContainsKey(monster.SnoMonster.NameLocalized))
                    {
                        summoner1count++;
                        if (ShowSummonerNormalMonsterBar)
                        {
                            DrawHealthBar(layer, monster, ref yref);
                            yref += 0.5f;
                        }
                        Decorator.Paint(layer, monster, monster.FloorCoordinate, monster.SnoMonster.NameLocalized);
                    }
                    if (SummonerMonsternames2.ContainsKey(monster.SnoMonster.NameEnglish) || SummonerMonsternames2.ContainsKey(monster.SnoMonster.NameLocalized))
                    {
                        summoner2count++;
                        if (ShowSummonerNormalMonsterBar)
                        {
                            DrawHealthBar(layer, monster, ref yref);
                            yref += 0.5f;
                        }
                        Decorator.Paint(layer, monster, monster.FloorCoordinate, monster.SnoMonster.NameLocalized);
                    }
                }

            }
            if (ShowSummonerCount)
            {
                textBuilder.Clear();
                if (summonerelite1count > 0)
                {
                    textBuilder.AppendFormat("EZombieS: {0}", summonerelite1count);
                    textBuilder.AppendLine();
                }
                if (summonedelite1count > 0)
                {
                    textBuilder.AppendFormat("illuZombieS: {0}", summonedelite1count);
                    textBuilder.AppendLine();
                }
                if (summonerelite2count > 0)
                {
                    textBuilder.AppendFormat("ESkeletonS: {0}", summonerelite2count);
                    textBuilder.AppendLine();
                }
                if (summonedelite2count > 0)
                {
                    textBuilder.AppendFormat("illuSkeletonS: {0}", summonedelite2count);
                    textBuilder.AppendLine();
                }
                if (summoner1count > 0)
                {
                    textBuilder.AppendFormat("ZombieS: {0}", summoner1count);
                    textBuilder.AppendLine();
                }
                if (summoner2count > 0)
                {
                    textBuilder.AppendFormat("SkeletonS: {0}", summoner2count);
                    textBuilder.AppendLine();
                }
                if (summonedcount > 0)
                {
                    textBuilder.AppendLine();
                    textBuilder.AppendFormat("Summoned: {0}", summonedcount);
                    textBuilder.AppendLine();
                }
            var layout = DefaultTextFont.GetTextLayout(textBuilder.ToString());
            DefaultTextFont.DrawText(layout, Hud.Window.Size.Width * 0.76f, Hud.Window.Size.Height * 0.61f);
            }
        }

  }
}

    


