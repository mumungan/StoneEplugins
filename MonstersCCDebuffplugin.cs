using Turbo.Plugins.Default;
using System;

namespace Turbo.Plugins.Stone
{
    public class MonstersCCDebuffplugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection InvulnerableLabelDecorator { get; set; }
        public WorldDecoratorCollection InvulnerableShapeDecorator { get; set; }

        public MonstersCCDebuffplugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            InvulnerableLabelDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud) 
		{
                    BackgroundBrush = Hud.Render.CreateBrush(255, 255, 255, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(255, 112, 48, 160, -1),
                    TextFont = Hud.Render.CreateFont("tahoma", 10f, 255, 112, 48, 160, true, false, 255, 0, 0, 0, true),
            });
            InvulnerableShapeDecorator = new WorldDecoratorCollection(
                new GroundShapeDecorator(Hud)
                {
                    RadiusTransformator = new StandardPingRadiusTransformator (Hud, 400),
                    Brush = Hud.Render.CreateBrush(255, 112, 48, 160, 4),
                    Radius = 4f,
            });
        }

       public void PaintWorld(WorldLayer layer)
       {
           var monsters = Hud.Game.AliveMonsters;
           foreach (var monster in monsters)
           {
               if (monster.Invulnerable && monster.IsElite)
                {
                    InvulnerableLabelDecorator.Paint(layer, monster, monster.FloorCoordinate, "Invulnerable");
		InvulnerableShapeDecorator.Paint(layer, monster, monster.FloorCoordinate, null);

                }
            }
        }
    }
}




   