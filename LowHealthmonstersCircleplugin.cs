using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{
    public class LowHealthmonstersCircleplugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection LowHealthDecorator { get; set; }
        public int LowHealthLimit { get; set; }

        public LowHealthmonstersCircleplugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            LowHealthLimit = 15;

            LowHealthDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud) 
                {
                    Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 10f),
                    Radius = 0.5f,
                }
                );
        }

        public void PaintWorld(WorldLayer layer)
        {
            var monsters = Hud.Game.AliveMonsters;
            foreach (var monster in monsters)
            {
            if (monster.CurHealth / monster.MaxHealth * 100 <= LowHealthLimit)
                {
                    LowHealthDecorator.Paint(layer, monster, monster.FloorCoordinate, monster.SnoMonster.NameLocalized);
                }
            }
        }
    }
}
	



   