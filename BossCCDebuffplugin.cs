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
						IScreenCoordinate boss = Hud.Window.CreateScreenCoordinate(monster.ScreenCoordinate.X, monster.ScreenCoordinate.Y);
						Hud.Render.CreateBrush(192, 255, 255, 55, -1).DrawLine(boss.X, boss.Y + 60, Hud.Game.Me.ScreenCoordinate.X, Hud.Game.Me.ScreenCoordinate.Y + 60, 1.0f);
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
                    if (monster.Frozen && showCC)
                    { BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Frozen"); }
                    if (monster.Chilled && showCC)
                    { BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Chilled"); }
                    if (monster.Slow && showCC)
                    { BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Slow"); }
                    if (monster.Stunned && showCC)
                    { BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Stunned"); }
                    if (monster.Blind && showCC)
                    { BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Blind"); }
                    if (monster.Locust && showDebuff)
                    { BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Locust"); }
                    if (monster.Haunted && showDebuff)
                    { BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Haunted"); }
                    if (monster.Palmed && showDebuff)
                    { BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Palmed"); }
                    if (monster.MarkedForDeath && showDebuff)
                    { BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "MarkedForDeath"); }
                    if (monster.Strongarmed && showDebuff)
                    { BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, offset), "Strongarmed"); }

                }
        }
    }
}





   