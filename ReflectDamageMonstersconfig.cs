using Turbo.Plugins.Default;

namespace Turbo.Plugins.Stone
{
    public class ReflectDamageMonstersConfig: BasePlugin, ICustomizer
    {
        public ReflectDamageMonstersConfig()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public void Customize()
        {
            Hud.RunOnPlugin<ReflectDamageMonstersPlugin>(plugin =>
            {                
			plugin.ShowGroundLabel = true; //GroundLabel on, off
			plugin.ShowMiniMapLabel = true; //MiniMapLabel on, off
			// Offset
            plugin.RFoffset = -2.0f; // Customize ★GroundLabel offset 
            plugin.RadiusOffset = 15.0f; // Customize ★Label minimap offset
			// Label
            plugin.RFLabel = "★"; // Customize ReflectDamageMonsters' Label + monsterG1 Label
			plugin.monsterG2Label = "💥"; // Customize Label
			plugin.monsterG3Label = "🔆";
			plugin.monsterG4Label = "☠️";
			plugin.affixLabel_1 = "👹";
			plugin.affixLabel_2 = "🛑";
			plugin.affixLabel_3 = "🏴‍";
			// Label Colour
			plugin.RFLabelColour = "yellow"; // Customize Label colour  //white, yellow, red, green, blue, blueviolet, orange, black, fuchsia, gold, deeppink. Choose from these colors.
			plugin.monsterG2LabelColour = "yellow";  
			plugin.monsterG3LabelColour = "yellow";
			plugin.monsterG4LabelColour = "blueviolet";
			plugin.affixLabel_1Colour = "white"; // // Customize the Label colour of EliteAffix //white, yellow, red, green, blue, blueviolet, orange, black, fuchsia, gold, deeppink. Choose from these colors.
			plugin.affixLabel_2Colour = "white";
			plugin.affixLabel_3Colour = "white";
			// Label Size
			plugin.groundLabelsizeG1 = 12.0f; // Customize groundLabelsize
			plugin.groundLabelsizeG2 = 12.0f;
			plugin.groundLabelsizeG3 = 12.0f;
			plugin.groundLabelsizeG4 = 12.0f;
			plugin.groundLabelsizeaffix1 = 12.0f; // Customize the groundLabelsize of EliteAffix
			plugin.groundLabelsizeaffix2 = 12.0f;;
			plugin.groundLabelsizeaffix3 = 12.0f;;
			plugin.mapLabelsizeG1 = 8.0f; // Customize MapLabelsize 
			plugin.mapLabelsizeG2 = 8.0f;
			plugin.mapLabelsizeG3 = 8.0f;
			plugin.mapLabelsizeG4 = 8.0f;
			plugin.mapLabelsizeaffix1 = 8.0f; // Customize the MapLabelsize of EliteAffix
			plugin.mapLabelsizeaffix2 = 8.0f;
			plugin.mapLabelsizeaffix3 = 8.0f;
			
			// Monster Group 1 that wants to label. monsterG1Label uses ReflectDamageMonsters'  label
            plugin.monsterG1_1 = ""; // Insert the Name of the monster. you can use English or your default game language
            plugin.monsterG1_2 = "";  // monsterGroup 1 Label colour
            plugin.monsterG1_3 = "";      
            plugin.monsterG1_4 = "";
            // Monster Group 2 that wants to label. 
            plugin.monsterG2_1 = "Retching Cadaver";  // monsterGroup 2 Label colour
            plugin.monsterG2_2 = "Deathspitter";
            plugin.monsterG2_3 = "Spewing Horror";
            plugin.monsterG2_4 = "Dust Retcher";
			// Monster Group 3 that wants to label. 
            plugin.monsterG3_1 = "Tomb Guardian";  // monsterGroup 3 Label colour
            plugin.monsterG3_2 = "Vengeful Summoner";
            plugin.monsterG3_3 = "Returned Summoner";
            plugin.monsterG3_4 = "Tortured Summoner";
			// Monster Group 4 that wants to label. 
            plugin.monsterG4_1 = "";  // monsterGroup 4 Label colour
            plugin.monsterG4_2 = "";
            plugin.monsterG4_3 = "";
            plugin.monsterG4_4 = "";
			// Elite Affix Label
			plugin.affixname_1 = "Wormhole"; // Insert the the Affix of Elite. you can use English or your default game language. ex) Wormhole, Juggernaut, Illusionist
			plugin.affixname_2 = "Juggernaut";
			plugin.affixname_3 = "Illusionist";
			});
         }
	}
}
