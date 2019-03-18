using System;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{
    public class WizardArchonBuffCustomizerPlugin : BasePlugin, ICustomizer
    {
        public WizardArchonBuffCustomizerPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public void Customize()
        {
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.TimeLeftFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.StackFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            Hud.GetPlugin<TopRightBuffListPlugin>().BuffPainter.ShowTooltips = true;
            Hud.GetPlugin<TopRightBuffListPlugin>().PositionX = 0.5f;
            Hud.GetPlugin<TopRightBuffListPlugin>().PositionY = 0.3f;
            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.SizeMultiplier = 0.8f;

            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.Rules.Add(new BuffRule(134872) { IconIndex = 2, MinimumIconCount = 1, ShowTimeLeft = false, ShowStacks = true }); // Archon 
            Hud.GetPlugin<TopRightBuffListPlugin>().RuleCalculator.Rules.Add(new BuffRule(403464) { IconIndex = 1, MinimumIconCount = 1, ShowStacks = true, ShowTimeLeft = true }); //GogokOfSwiftnessPrimary
        }
	}
}