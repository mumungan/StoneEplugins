using Turbo.Plugins.Default;
using SharpDX;
using System;
using System.Linq;

namespace Turbo.Plugins.Stone
{
    public class WizardArchonPlugin : BasePlugin, IInGameWorldPainter, ICustomizer
    {
        private double ArchonCooldownremaining { get; set; }
		private IFont textFont { get; set; }
        private readonly int[] skillOrder = new int[] { 2, 3, 4, 5, 0, 1 };

        public WizardArchonPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
  
			textFont = Hud.Render.CreateFont("tahoma", 16, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
        }

        public void Customize()
        {
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.TimeLeftFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.StackFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.ShowTooltips = true;
            Hud.GetPlugin<TopRightBuffListPlugin>().PositionX = 0.5f;
            Hud.GetPlugin<TopRightBuffListPlugin>().PositionY = 0.3f;
            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.SizeMultiplier = 0.8f;

            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.Rules.Add(new BuffRule(134872) { IconIndex = 2, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = true }); // Archon 
            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.Rules.Add(new BuffRule(403464) { IconIndex = 1, MinimumIconCount = 1, ShowStacks = true, ShowTimeLeft = true }); //GogokOfSwiftnessPrimary
        }

        public void PaintWorld(WorldLayer layer)
        {
            var me = Hud.Game.Me;
            float x = Hud.Window.Size.Width / 2 + Hud.Window.Size.Width * 0.038f;
            float y = Hud.Window.Size.Height / 2 - Hud.Window.Size.Height * 0.2f;
            var rect = new RectangleF(x, y, 40.0f, 40.0f);
            if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard)
            {
                foreach (var i in skillOrder)
                {
                    var skill = me.Powers.SkillSlots[i];
                    if (skill == null || skill.SnoPower.Sno != 134872) continue;
                    ArchonCooldownremaining = (skill.CooldownFinishTick - Hud.Game.CurrentGameTick) / 60.0d;
                    if (ArchonCooldownremaining < 0) ArchonCooldownremaining = 0;
                    Hud.Texture.GetTexture(Hud.Sno.GetSnoPower(134872).NormalIconTextureId).Draw(rect.X - 35.0f, rect.Y, 40.0f, 40.0f);
                    var layout = textFont.GetTextLayout(Math.Truncate(ArchonCooldownremaining).ToString());
                    textFont.DrawText(layout, rect.Right - (rect.Width / 8.0f) - (float)Math.Ceiling(layout.Metrics.Width) - 35.0f, rect.Bottom - layout.Metrics.Height);
                }
            }

        }
    }
}

