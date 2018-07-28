using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{
    public class StarpactcirclePlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection meteorcircleDeco { get; set; }
        public WorldDecoratorCollection meteorstringDeco { get; set; }
        public WorldDecoratorCollection meteorvisionstringDeco { get; set; }
        public WorldDecoratorCollection meteortimerDecorator { get; set; }
		public bool timeron { get; set; }
        public float remaining { get; set; }
        public float starpactstarttict { get; set; }
        private bool starpacttimerRunning = false;

        public StarpactcirclePlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

			timeron = true;
            meteorcircleDeco = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 0, 140, 255, 6),
                    Radius = 13,
                }
                );
            meteorstringDeco = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 255, 255, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(255, 112, 48, 160, -1),
                    TextFont = Hud.Render.CreateFont("tahoma", 11, 255, 0, 140, 255, true, false, 128, 0, 0, 0, true),
                }
            );
            meteorvisionstringDeco = new WorldDecoratorCollection(
            new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 255, 128, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(255, 112, 48, 160, -1),
                    TextFont = Hud.Render.CreateFont("tahoma", 11, 255, 120, 0, 120, true, false, 128, 0, 0, 0, true),
                }
                );
            meteortimerDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    CountDownFrom = 1.25f,
                    TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 100, 255, 150, true, false, 128, 0, 0, 0, true),
                },
                new GroundTimerDecorator(Hud)
                {
                    CountDownFrom = 1.25f,
                    BackgroundBrushEmpty = Hud.Render.CreateBrush(100, 0, 0, 0, 0),
                    BackgroundBrushFill = Hud.Render.CreateBrush(200, 223, 47, 2, 0),
                    Radius = 25,
                }
            );
        }

        public void PaintWorld(WorldLayer layer)
        {
            var actors = Hud.Game.Actors;
            var me = Hud.Game.Me;
            remaining = 1.25f - ((Hud.Game.CurrentGameTick - starpactstarttict) / 60.0f);
			if (starpacttimerRunning == true && remaining <= 0) starpacttimerRunning = false;
            if (remaining < 0) remaining = 0;
            foreach (var actor in actors)
            {
                    switch (actor.SnoActor.Sno)
                    {
                        case 217142:
                            meteorcircleDeco.Paint(layer, actor, actor.FloorCoordinate, null);
                            if (Hud.Game.Me.HeroClassDefinition.HeroClass != HeroClass.Wizard)
                            {
                                meteortimerDecorator.Paint(layer, actor, actor.FloorCoordinate, null);
                                break;
                            }
                            if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard)
                            {
                                if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && me.Stats.ResourceCurArcane == 0)
                                {
                                    if (!starpacttimerRunning)
                                    {
                                        starpactstarttict = Hud.Game.CurrentGameTick;
                                        starpacttimerRunning = true;
                                    }
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized);
                                    if (timeron) meteortimerDecorator.Paint(layer, actor, actor.FloorCoordinate.Offset(0, 0, -3), null);
									break;
                                }
                                if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && remaining >= 0.1)
                                {
								    if (starpacttimerRunning)
                                    {
                                        starpacttimerRunning = false;
                                    }
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized);
                                    if (timeron) meteortimerDecorator.Paint(layer, actor, actor.FloorCoordinate.Offset(0, 0, -3), null);
									break;
                                }
                                if (Hud.Game.Me.HeroClassDefinition.HeroClass == HeroClass.Wizard && remaining < 0.1 && remaining > 0)
                                {
									if (timeron) meteortimerDecorator.Paint(layer, actor, actor.FloorCoordinate.Offset(0, 0, -3), null);
                                    if (starpacttimerRunning)
                                    {
                                        starpacttimerRunning = false;
                                    }
                                    if (me.Powers.BuffIsActive(430674, 1) && me.Powers.BuffIsActive(134456))
                                    {
                                        meteorvisionstringDeco.Paint(layer, actor, actor.FloorCoordinate, "VIS" + Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_ArcaneTorrent.NameLocalized);
                                        break;
                                    }
                                    if (me.Powers.BuffIsActive(134456))
                                    {
                                        meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_ArcaneTorrent.NameLocalized);
                                        break;
                                    }
                                    if (me.Powers.BuffIsActive(430674, 1) && me.Powers.BuffIsActive(91549))
                                    {
                                        meteorvisionstringDeco.Paint(layer, actor, actor.FloorCoordinate, "VIS" + Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_Disintegrate.NameLocalized);
                                        break;
                                    }
                                    if (me.Powers.BuffIsActive(91549))
                                    {
                                        meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_Disintegrate.NameLocalized);
                                        break;
                                    }
                                }
                            }
                            break;
                    }
            }
        }
    }
}

