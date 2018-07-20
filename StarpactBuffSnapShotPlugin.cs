using Turbo.Plugins.Default;
using SharpDX;
using System;

namespace Turbo.Plugins.Stone
{
    public class StarpactBuffSnapShotPlugin : BasePlugin, IInGameWorldPainter
    {
        private float sbremaining { get; set; }
        private float sbstarpactstarttict { get; set; }
        private bool sbstarpacttimerRunning = false;
        private IFont StackFont { get; set; }
		private IFont textFont { get; set; }
		private IBrush visionBrush, edgeBrush, dynamoBrush;
		private int blackHolesb, blackHole, waveOfForcesb, waveOfForce, arcaneDynamosb, arcaneDynamo;
		private float resourcesb, resource;
		private bool getv, getv1, getv2;
		private string coe;

        public StarpactBuffSnapShotPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
  
			textFont = Hud.Render.CreateFont("tahoma", 14, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
            StackFont = Hud.Render.CreateFont("tahoma", 15, 255, 255, 255, 255, false, false, 255, 0, 0, 0, true);
			visionBrush = Hud.Render.CreateBrush(255, 112, 48, 160, 0);
			edgeBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -2);
			dynamoBrush = Hud.Render.CreateBrush(150, 150, 150, 150, 0);
        }

        public void PaintWorld(WorldLayer layer)
        {
            var actors = Hud.Game.Actors;
            var me = Hud.Game.Me;
			if (Hud.Game.Me.HeroClassDefinition.HeroClass != HeroClass.Wizard) return;
            sbremaining = 1.25f - ((Hud.Game.CurrentGameTick - sbstarpactstarttict) / 60.0f);
			if (sbstarpacttimerRunning == true && sbremaining <= 0) sbstarpacttimerRunning = false;
            if (sbremaining < 0) sbremaining = 0;  	
            float x = Hud.Window.Size.Width / 2 - Hud.Window.Size.Width * 0.042f;
            float y  = Hud.Window.Size.Height / 2 - Hud.Window.Size.Height * 0.41f;
            var rect = new RectangleF(x, y, 40.0f, 40.0f);
			if (coe == "V")
			{
				visionBrush.DrawRectangle(rect);
				edgeBrush.DrawRectangle(rect);
			}
			dynamoBrush.DrawRectangle(rect.X + 120.0f, rect.Y, 40.0f, 40.0f);

			if (resourcesb == null) resourcesb = 0;
			var resourcetext = textFont.GetTextLayout(Math.Truncate(resourcesb).ToString());
			textFont.DrawText(resourcetext, Hud.Window.Size.Width * 0.435f, Hud.Window.Size.Height * 0.1f);
			Hud.Texture.GetItemTexture(Hud.Sno.SnoItems.P2_Unique_Ring_04).Draw(rect);
			if (String.IsNullOrEmpty(coe)) coe = "";
			var coetext = StackFont.GetTextLayout(coe);
			StackFont.DrawText(coetext, rect.Right - (rect.Width / 8.0f) - (float)Math.Ceiling(coetext.Metrics.Width), rect.Bottom - coetext.Metrics.Height);
			if (blackHolesb == null) blackHolesb = 0;
			Hud.Texture.GetTexture(Hud.Sno.GetSnoPower(243141).NormalIconTextureId).Draw(rect.X + 40.0f, rect.Y, 40.0f, 40.0f);	
			var layout = StackFont.GetTextLayout(blackHolesb.ToString());
			StackFont.DrawText(layout, rect.Right - (rect.Width / 8.0f) - (float)Math.Ceiling(layout.Metrics.Width) + 40.0f, rect.Bottom - layout.Metrics.Height);
			if (waveOfForcesb == null) waveOfForcesb = 0;
			Hud.Texture.GetTexture(Hud.Sno.GetSnoPower(30796).NormalIconTextureId).Draw(rect.X + 80.0f, rect.Y, 40.0f, 40.0f);	
			var layout1 = StackFont.GetTextLayout(waveOfForcesb.ToString());
			StackFont.DrawText(layout1, rect.Right - (rect.Width / 8.0f) - (float)Math.Ceiling(layout1.Metrics.Width) + 80.0f, rect.Bottom - layout1.Metrics.Height);
			if (arcaneDynamosb == null) arcaneDynamosb = 0;
			Hud.Texture.GetTexture(Hud.Sno.GetSnoPower(208823).NormalIconTextureId).Draw(rect.X + 120.0f, rect.Y, 40.0f, 40.0f);	
			var layout2 = StackFont.GetTextLayout(arcaneDynamosb.ToString());
			StackFont.DrawText(layout2, rect.Right - (rect.Width / 8.0f) - (float)Math.Ceiling(layout2.Metrics.Width) + 120.0f, rect.Bottom - layout2.Metrics.Height);

			if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && Hud.Game.Me.Stats.ResourceCurArcane > 0)
			{
				var skill = Hud.Game.Me.Powers.GetBuff(243141);
				if (skill != null && Hud.Game.Me.Powers.GetBuff(243141).IconCounts[5] > 0)
				{
					getv = true;
					blackHole = Hud.Game.Me.Powers.GetBuff(243141).IconCounts[5];
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else if (getv == true)
				{
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else 
				{
					blackHole = 0;
				}
				var skill1 = Hud.Game.Me.Powers.GetBuff(30796);
				if (skill1 != null && Hud.Game.Me.Powers.GetBuff(30796).IconCounts[2] > 0)
				{
					getv1 = true;
					waveOfForce = Hud.Game.Me.Powers.GetBuff(30796).IconCounts[2];
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else if (getv1 == true)
				{
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else
				{
					waveOfForce = 0;
				}
				var skill2 = Hud.Game.Me.Powers.GetBuff(208823);
				if (skill2 != null && Hud.Game.Me.Powers.GetBuff(208823).IconCounts[1] > 0)
				{
					getv2 = true;
					arcaneDynamo = Hud.Game.Me.Powers.GetBuff(208823).IconCounts[1];
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else if (getv2 == true)
				{
					resource = Hud.Game.Me.Stats.ResourceCurArcane;
				}
				else
				{
					arcaneDynamo = 0;
				}
			}

	        foreach (var actor in actors)
            {
                    switch (actor.SnoActor.Sno)
                    {
                        case 217142:
                            if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard)
                            {
                                if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && me.Stats.ResourceCurArcane == 0)
                                {
                                    if (!sbstarpacttimerRunning)
                                    {
                                        sbstarpactstarttict = Hud.Game.CurrentGameTick;
										sbstarpacttimerRunning = true;
										if (blackHole > 0) blackHolesb = blackHole;
										else blackHolesb = 0;
										if (waveOfForce > 0) waveOfForcesb = waveOfForce;
										else waveOfForcesb = 0;
										if (arcaneDynamo > 0) arcaneDynamosb = arcaneDynamo;
										else arcaneDynamosb = 0;
										resourcesb = resource;

										blackHole = 0;
										waveOfForce = 0;
										arcaneDynamo = 0;
										resource = 0;
                                    }
                                    break;
                                }
                                if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && sbremaining > 0)
                                {
                                    if (sbstarpacttimerRunning)
                                    {
                                        sbstarpacttimerRunning = false;
										getv = false;
										getv1 = false;
										getv2 = false;
                                    }
	                                if ( sbremaining < 0.4 && sbremaining > 0)
									{
										if (me.Powers.BuffIsActive(430674, 1)) coe = "V";
										else if (me.Powers.BuffIsActive(430674, 2)) coe = "C";
										else if (me.Powers.BuffIsActive(430674, 3)) coe = "F";
										else if (me.Powers.BuffIsActive(430674, 5)) coe = "L";
										else coe = "";
										break;
									}
									break;
                                }
                            }
                            break;
                    }
            }
        }
    }
}

