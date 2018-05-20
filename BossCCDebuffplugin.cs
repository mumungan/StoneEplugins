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
        public IFont BossCCDebuffFont { get; set; }
        public bool showCC { get; set; }
        public bool showDebuff { get; set; }

        public BossCCDebuffplugin()
        {
            Enabled = true;
            showCC = true;
            showDebuff = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
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
        }

       public void PaintWorld(WorldLayer layer)
       {
           var monsters = Hud.Game.AliveMonsters;
           foreach (var monster in monsters)
               if (monster.Rarity == ActorRarity.Boss)
           {
               if (monster.Frozen && showCC)
        		{BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Frozen");}
               if (monster.Chilled && showCC)
		        {BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Chilled");}
               if (monster.Slow && showCC)
		        {BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Slow");}
               if (monster.Stunned && showCC)
		        {BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Stunned");}
               if (monster.Blind && showCC)
		        {BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Blind");}
               if (monster.Locust && showDebuff)
		        {BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Locust");}
               if (monster.Haunted && showDebuff)
		        {BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Haunted");}
               if (monster.Palmed && showDebuff)
		        {BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Palmed");}
               if (monster.MarkedForDeath && showDebuff)
		        {BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "MarkedForDeath");}
               if (monster.Strongarmed && showDebuff)
		        {BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Strongarmed");}

            }
        }
    }
}




   