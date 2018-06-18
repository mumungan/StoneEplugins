using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{

    public class ReflectDamageMonstersPlugin : BasePlugin, IInGameWorldPainter
	{

        public WorldDecoratorCollection reflectdamagemonsterDecorator { get; set; }
        private List<uint> reflectdamagemonsters { get; set; }

        public ReflectDamageMonstersPlugin()
		{
            Enabled = true;
		}

        public override void Load(IController hud)
        {
            base.Load(hud);

            reflectdamagemonsters = new List<uint>()
            {
                26245, //Sand Dweller
                116305, //Sand Dweller
                196846, //Sand Dweller
                116075,
                26066, //Dune Dervish
                26067, //Vicious Magewraith
                3982, //Vicious Magewraith
            };

            reflectdamagemonsterDecorator = new WorldDecoratorCollection(
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 12f, 255, 255, 255, 0, true, false, false),
                },
                new GroundLabelDecorator(Hud)
                {
                    TextFont = Hud.Render.CreateFont("tahoma", 18f, 255, 255, 255, 0, false, false, false),
                }
                );
        }


        public void PaintWorld(WorldLayer layer)
        {
            var monsters = Hud.Game.AliveMonsters;
            foreach (var monster in monsters)
            {
                if (reflectdamagemonsters.Contains(monster.SnoMonster.Sno))
                {
                    reflectdamagemonsterDecorator.Paint(layer, monster, monster.FloorCoordinate.Offset(0, 0, -2), "★");
                }
            }
        }

    }

}