using System.Collections.Generic;
using Turbo.Plugins.Default;
using System.Linq;
using System;

namespace Turbo.Plugins.Stone
{
    public class BossCCDebuffplugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection BossCCDecorator { get; set; }
        public WorldDecoratorCollection BossDebuffDecorator { get; set; }
        public WorldDecoratorCollection BossDirectionLineDecorator { get; set; }
        public IFont BossCCDebuffFont { get; set; }
        public bool showCC { get; set; }
        public bool showDebuff { get; set; }
        public bool showCCoffMessage { get; set; }
        public bool showDirectionLine { get; set; }
		public bool showMiniMapLine { get; set; }
		public bool showGroundLine { get; set; }
        public float CCoffStarttick { get; set; }
		public float offset { get; set; }
        private bool CCofftimerRunning = false;

        public BossCCDebuffplugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            showCC = true;
            showDebuff = true;
            showCCoffMessage = true;
            showDirectionLine = true;
			showMiniMapLine = true;
			showGroundLine = true;
			offset = -2.0f;

            BossCCDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 255, 255, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(255, 112, 48, 160, -1),
                    TextFont = Hud.Render.CreateFont("tahoma", 10f, 255, 112, 48, 160, true, false, 255, 0, 0, 0, true),
                });
            BossDebuffDecorator = new WorldDecoratorCollection(
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(255, 255, 128, 0, 0),
                BorderBrush = Hud.Render.CreateBrush(255, 112, 48, 160, -1),
                TextFont = Hud.Render.CreateFont("tahoma", 10f, 255, 112, 48, 160, true, false, 255, 0, 0, 0, true),
            });
            BossDirectionLineDecorator = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(192, 255, 255, 55, -1),
                ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                Radius = 10.0f,
                ShapePainter = new LineFromMeShapePainter(Hud),
            },
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6f, 200, 255, 255, 0, false, false, 128, 0, 0, 0, true),
                RadiusOffset = 10,
                Up = true,
            });
        }

        public void PaintWorld(WorldLayer layer)
        {
			foreach (var marker in Hud.Game.Markers)
			{
			if (showDirectionLine && showMiniMapLine && marker.SnoActor != null)
				{
				if (marker.SnoActor.Code.Contains("Boss"))
					{
						BossDirectionLineDecorator.ToggleDecorators<GroundLabelDecorator>(!marker.FloorCoordinate.IsOnScreen()); // do not display ground labels when the marker is on the screen
						BossDirectionLineDecorator.Paint(layer, null, marker.FloorCoordinate, marker.Name);
					}
				}
			}
            var monsters = Hud.Game.AliveMonsters;
            foreach (var monster in monsters)
                if (monster.Rarity == ActorRarity.Boss)
                {
                    if (showDirectionLine && showMiniMapLine)
                    {
                        BossDirectionLineDecorator.ToggleDecorators<GroundLabelDecorator>(!monster.FloorCoordinate.IsOnScreen()); // do not display ground labels when the marker is on the screen
                        BossDirectionLineDecorator.Paint(layer, null, monster.FloorCoordinate, null);
					}
					if (showDirectionLine && showGroundLine)
					{
						IScreenCoordinate boss = Hud.Window.CreateScreenCoordinate(monster.FloorCoordinate.ToScreenCoordinate().X, monster.FloorCoordinate.ToScreenCoordinate().Y);
						Hud.Render.CreateBrush(192, 255, 255, 55, -1).DrawLine(boss.X, boss.Y, Hud.Game.Me.ScreenCoordinate.X, Hud.Game.Me.ScreenCoordinate.Y + 60, 1.0f);
                    }
                    if ((showCCoffMessage) && (!monster.Frozen && !monster.Chilled && !monster.Slow && !monster.Stunned && !monster.Blind))
                    {
                        var CCofftime = (Hud.Game.CurrentGameTick - CCoffStarttick) / 60.0d;
                        String CCofftimetext = "CC off " + Math.Truncate(CCofftime) + "s";
                        if (!CCofftimerRunning)
                        {
                            CCoffStarttick = Hud.Game.CurrentGameTick;
                            CCofftimerRunning = true;
                        }
                        BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), CCofftimetext);
                    }

                    if ( (showCCoffMessage) && (monster.Frozen || monster.Chilled || monster.Slow || monster.Stunned || monster.Blind))
					{
                        if (CCofftimerRunning)
                        {
                            CCofftimerRunning = false;
                        }
					}
					string data1 = ""; 
                    if (monster.Frozen && showCC)
                    { 
					data1 += "Frozen";
					BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Chilled && showCC)
                    { 
					data1 += " Chilled";
					BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Slow && showCC)
                    {
					data1 += " Slow";
					BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Stunned && showCC)
                    {
					data1 += " Stunned";
					BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Blind && showCC)
                    {
					data1 += " Blind";
					BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Locust && showDebuff)
                    { 
					data1 += " Locust";
					BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Haunted && showDebuff)
                    { 
					data1 += " Haunted";
					BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Palmed && showDebuff)
                    { 
					data1 += " Palmed";
					BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.MarkedForDeath && showDebuff)
                    { 
					data1 += " MarkedForDeath";
					BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}
                    if (monster.Strongarmed && showDebuff)
                    { 
					data1 += " Strongarmed";
					BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), data1); 
					}

                }
        }
    }
}





   