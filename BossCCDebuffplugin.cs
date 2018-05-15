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

        public BossCCDebuffplugin()
        {
            Enabled = true;
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
            if (monster.Frozen)
		{
            BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Frozen");
                   }
            if (monster.Chilled)
		{
            BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Chilled");
                   }
            if (monster.Slow)
		{
            BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Slow");
                   }
            if (monster.Stunned)
		{
            BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Stunned");
                   }
            if (monster.Blind)
		{
            BossCCDecorator.Paint(layer, monster, monster.FloorCoordinate, "Blind");
                   }
            if (monster.Locust)
		{
            BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Locust");
                   }
            if (monster.Haunted)
		{
            BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Haunted");
                   }
            if (monster.Palmed)
		{
            BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Palmed");
                   }
            if (monster.MarkedForDeath)
		{
            BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "MarkedForDeath");
                   }
            if (monster.Strongarmed)
		{
            BossDebuffDecorator.Paint(layer, monster, monster.FloorCoordinate, "Strongarmed");
                   }

            }
        }
    }
}




   