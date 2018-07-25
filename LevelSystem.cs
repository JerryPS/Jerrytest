using UnityEngine;
using System;


[CreateAssetMenu (fileName = "LevelSystemData", menuName = "Ball breker ScritableObject/LevelSystemData", order = 2)]
	
	public class LevelSystem : ScriptableObject
	{
    public Sprite simpleBlockSprite;

    public Sprite nonDestroyBlockSprite;
	public int MinStarBlockCome,MaxStarBlockCome ;
	public float givePercentageForPerfect ;
	public Color homeScreenColor,ball1Color,ball2Color,scoreTextColor;
	public int startingsnakeballs;

	public WallSystem bossBlockWall;

	public WallSystem bossWall;

	public WallSystem wall0;
	public WallSystem wallSmall;
	public WallSystem wallMedium;
	public WallSystem wallLarge;

	public LevelListProgress[] numberOfProgressLevels;
	public Levels[] numberOfLevels ;
	public bool showBonuspatterns;
	public Life life;
	public int[] addEnemyAtHit ;
	public Coins coinsData ;

	public WaveClass[] waveTimesLevels ;
	public Color DhomeScreenColor,Dball1Color,Dball2Color,DwallZero,DwallSmall,DwallMedium,DwallHard,DscoreTextColor;
    public IAPProductID[] numberOfIap;
    public IAPProductIDLife[] numberOfIapOfLife;

	}

[Serializable]
public class IAPProductID
{
    public bool tabForVideoads;
    public Sprite packImage;
    public string packName;
    public string PackProductID;
    public string PackPrice;
    public long PointsGiveToThisPack;

}
[Serializable]
public class IAPProductIDLife
{
    public Sprite packImage;
    public string packName;
    public string PackProductID;
    public string PackPrice;
    public int LifeToGiveThisPack;

}

[Serializable]
public class LevelListProgress
{
	public int MinlevelNumber ;
	public int MaxlevelNumber ;
	public int timeForlevelInSeconds ;


}


[Serializable]
public class WaveClass
{
	public int MinScore,MaxScore ;
	public float waitingTime ;
}
[Serializable]
public class  DividerLevels
{
	public int MinScore,MaxScore ;
	public int afterHits ;
	public float MinYscale, MaxYscale;
	public float MinMoveSpeed, MaxMoveSpeed;
	public TypeOfDivider[] numberOfWalls ;
}
[Serializable]
public class DividerData
{
public DividerLevels[] numberOfdividerLevels ;

}
[Serializable]
public class Life
{
	public int afterHitLifeCome;
	public int maxLife ;
}
[Serializable]
public class Coins
{
	public int afterHitcoinCome;
	public int maxCoin ;
}
[Serializable]
public class Levels
{
	public int MinScore, MaxScore, Rate;
	public float blockMoveSpeed;
	public int skipThislevelNow;
	public TypeOfLevels typeOfLevels;

	public TypesOfWall[] numberOfWalls ;

}

[Serializable]
public class WallSystem
{
	public String name ;
	public float ratioMin,ratioMax ;
	public Color wallColor ;
	public int minValue ;
}

[Serializable]
public enum TypesOfWall
{
	Wall0,
	WallSmall,
	WallMedium,
	WallLarge
}


[Serializable]
public enum TypeOfDivider
{
	Divider_Wall0,
	Divider_WallSmall,
	Divider_WallMedium,
	Divider_WallLarge
}

[Serializable]
public enum TypeOfLevels
{
	FourBlock,
	FiveBlock,
	PatternsLevel,
	MovingLevels,
	FiveBlock_PatternLevel,
	FiveBlock_MovingLevels,
	Pattern_MovingLevels,
	FiveBlock_PatternLevel_MovingBlock,
    HardStaticPatterLevel,
    HardMovingpatternLevel
}

