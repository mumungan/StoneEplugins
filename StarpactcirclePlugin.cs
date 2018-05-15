using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{
    public class StarpactcirclePlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection meteorcircleDeco { get; set; }
        public WorldDecoratorCollection meteorstringDeco { get; set; }
        public WorldDecoratorCollection meteortimerDecorator { get; set; }
        public float remaining { get; set; }
        public float starpactstarttict { get; set; } 

        public StarpactcirclePlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

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
            if (remaining < 0) remaining = 0;
            foreach (var actor in actors)
            {
                    switch (actor.SnoActor.Sno)
                    {
                        case 217142:
                            meteorcircleDeco.Paint(layer, actor, actor.FloorCoordinate, null);
                            if (me.Stats.ResourceCurArcane == 0)
                            {
                                starpactstarttict = Hud.Game.CurrentGameTick;
                                meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized);
                                break;
                            }
                            if (remaining >= 0.1)
                            {
                                meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized);
                                break;
                            }
                            if (remaining < 0.1 && remaining > 0)
                            {
                                if (me.Powers.BuffIsActive(430674, 1) && me.Powers.BuffIsActive(134456))
                                {
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, "VIS" + Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_ArcaneTorrent.NameLocalized);
                                    break;
                                }
                                if (me.Powers.BuffIsActive(134456))
                                {
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_ArcaneTorrent.NameLocalized);
                                    break;
                                }
                                if (me.Powers.BuffIsActive(430674, 1) && me.Powers.BuffIsActive(91549))
                                {
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, "VIS" + Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_Disintegrate.NameLocalized);
                                    break;
                                }
                                if (me.Powers.BuffIsActive(91549))
                                {
                                    meteorstringDeco.Paint(layer, actor, actor.FloorCoordinate, Hud.Sno.SnoPowers.Wizard_Meteor.NameLocalized + " + " + Hud.Sno.SnoPowers.Wizard_Disintegrate.NameLocalized);
                                    break;
                                }
                            }
                                meteorcircleDeco.Paint(layer, actor, actor.FloorCoordinate, null);
                                meteortimerDecorator.Paint(layer, actor, actor.FloorCoordinate, null);
                                break;
                    }
            }
        }
    }
}


