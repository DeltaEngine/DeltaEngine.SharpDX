using System.Collections.Generic;

namespace CreepyTowers
{
	public class Names
	{
		//  Names of Fonts
		public const string FontChelseaMarket14 = "ChelseaMarket14";
	  public const string FontVerdana12 = "Verdana12";

		// Names of UI XMLs
	  public const string XmlIntroScene = "IntroMenu";
		public const string XmlMenuScene = "SceneMainMenu";
		public const string XmlCreditsScene = "SceneCredits";
		public const string XmlSceneChildsRoom = "SceneChildsRoom";
		public const string XmlSceneBathroom = "SceneBathroom";
		public const string XmlSceneLivingRoom = "SceneLivingRoom";

		// Names of Level Grid XMLs
		public const string XmlLevelsChildsRoomGrid = "LevelsChildsRoomGrid";
		public const string XmlLevelsBathroomGrid = "LevelsBathroomGrid";
		public const string XmlLevelsLivingRoomGrid = "LevelsLivingRoomGrid";

		//Names of Level GenerateWaves XMLs
		public const string XmlChildrensRoom = "LevelsChildrensRoom";

		//Names of Creep Groups XMLs
		public const string XmlGroupCreeps = "GroupCreeps";

		//Names of Properties XMLs
		public const string XmlTowerProperties = "TowerProperties";
		public const string XmlCreepProperties = "CreepProperties";

		// Names of SpriteSheet Animations
		public const string SpritesheetDyingCloud = "SpritesheetDyingCloud";

		// Names of GUI elements 
		public const string BackgroundMainMenu = "TextureBackgroundBlue";
		public const string ImageLogo = "Logo";
		public const string ImageMenuKid = "MenuKid";
		public const string ImageCredits = "CreditsScreen";
		public const string ImageHealthBarGreen100 = "HealthBarGreen100";
		public const string ImageHealthBarGreen80 = "HealthBarGreen80";
		public const string ImageHealthBarGreen60 = "HealthBarGreen60";
		public const string ImageHealthBarOrange50 = "HealthBarOrange50";
		public const string ImageHealthBarOrange40 = "HealthBarOrange40";
		public const string ImageHealthBarOrange25 = "HealthBarOrange25";
		public const string ImageHealthBarRed20 = "HealthBarRed20";
		public const string ImageHealthBarRed10 = "HealthBarRed10";
		public const string ImageHealthBarRed05 = "HealthBarRed5";
	  public const string ButtonIntroFlipRight = "IntroButtonFlipRight";
	  public const string ButtonIntroFlipLeft = "IntroButtonFlipLeft";
	  public const string ButtonIntroSkip = "IntroButtonSkip";
		public const string ButtonMainMenuPlay = "MenuButtonPlay";
		public const string ButtonMainMenuHelpAndOptions = "MenuButtonHelpAndOptions";
		public const string ButtonMainMenuCredits = "MenuButtonCredits";
		public const string ButtonMainMenuQuit = "MenuButtonQuit";
		public const string ButtonCreditsMenuBack = "ButtonBack";
		public const string ButtonAcidTower = "ButtonAcidTower";
		public const string ButtonFireTower = "ButtonFireTower";
		public const string ButtonIceTower = "ButtonIceTower";
		public const string ButtonImpactTower = "ButtonImpactTower";
		public const string ButtonSliceTower = "ButtonSliceTower";
		public const string ButtonWaterTower = "ButtonWaterTower";
		public const string ButtonRefresh = "ButtonRefresh";
		public const string ButtonCreep = "ButtonCreep";
		public const string ButtonNext = "ButtonNext";
		public const string ButtonBackLeft = "ButtonBackLeft";
		public const string ButtonContinue = "ButtonContinue";
		public const string UIOptions = "UIOptions";
		public const string UICreepwave = "UICreepwave";
		public const string UIPlayerHealth = "UIPlayerHealth";
		public const string UIAvatarUnicorn = "UIAvatarUnicorn";
		public const string UIUnicornSpecialAttackIntervention = "UIUnicornSpecialAttackIntervention";
		public const string UIUnicornSpecialAttackSwap = "UIUnicornSpecialAttackSwap";
		public const string UIAvatarDragon = "UIAvatarDragon";
		public const string UIDragonSpecialAttackBreath = "UIDragonSpecialAttackBreath";
		public const string UIDragonSpecialAttackCannon = "UIDragonSpecialAttackCannon";
		public const string UIGold = "UIGold";
		public const string UIGem = "UIGem";

		// Names of Comic strip elements
	  public const string ComicStripsStoryboardPanel1 = "ComicStripsStoryboardPanel1";
    public const string ComicStripsStoryboardPanel2 = "ComicStripsStoryboardPanel2";
    public const string ComicStripsStoryboardPanel3 = "ComicStripsStoryboardPanel3";
    public const string ComicStripsStoryboardPanel4 = "ComicStripsStoryboardPanel4";
    public const string ComicStripsStoryboardPanel5 = "ComicStripsStoryboardPanel5";
		public const string ComicStripDragon = "ComicStripDragon";
		public const string ComicStripPiggybank = "ComicStripPiggybank";
		public const string ComicStripUnicorn = "ComicStripUnicorn";
		public const string ComicStripBubble = "ComicStripBubble";
		public const string IconMouseLeft = "IconMouseLeft";
		public const string IconMouseRight = "IconMouseRight";
		public const string UnicornAttackMockup = "UnicornAttackMockup";
		public const string DragonAttackMockup = "ComicStripsDragonAttackMockup512";

		// Names of 3D Tower models 
		public const string TowerAcidConeJanitorHigh = "TowerAcidConeJanitorHigh";
		public const string TowerFireCandlehulaHigh = "TowerFireCandlehulaHigh";
		public const string ModelIceTower = "TowerFireCandlehulaHigh";
		public const string TowersImpactRangedKnightscalesHigh = "TowersImpactRangedKnightscalesHigh";
		public const string ModelSliceTower = "TowersImpactRangedKnightscalesHigh";
		public const string TowerWaterRangedWatersprayHigh = "TowerWaterRangedWatersprayHigh";

		// Names of 3D Creep models
		public const string CreepCottonMummy = "walk01";

		// Names of 3D level models
		public const string LevelsChildsRoom = "LevelsChildsRoom";
		public const string LevelsBathRoom = "LevelsBathroom";
		public const string LevelsLivingRoom = "LevelsLivingRoom";

		public static readonly List<string> ComicStripImages = new List<string>
		{
			ComicStripBubble,
			ComicStripDragon,
			ComicStripPiggybank,
			ComicStripUnicorn
		};

		public static readonly List<string> UiImages = new List<string>
		{
			UIAvatarUnicorn,
			UIAvatarDragon,
			UICreepwave,
			UIGem,
			UIGold,
			UIPlayerHealth
		};

		public static readonly List<string> UiButtons  = new List<string>
		{
			ButtonCreep,
			ButtonRefresh,
			ButtonContinue,
			UIUnicornSpecialAttackIntervention,
			UIUnicornSpecialAttackSwap,
			UIDragonSpecialAttackBreath,
			UIDragonSpecialAttackCannon
		};
	}
}