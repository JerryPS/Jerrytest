using UnityEngine;
using System.Collections;
using System.Collections.Generic ;
using UnityEngine.UI;
using mixpanel;
using PaperPlaneTools;
using MarchingBytes;


public enum DimesionEvents
{
	DefaultGroup,
	HardGroup,
	EasyGroup
}
public class PlatfromController : MonoBehaviour {


    public static bool isPowerBooster, isSpeedBooster, isPointsBooster;

    public int afterNumberOfGamesBossCome = 3;

	public static bool callFiveWave,callFourWave ;

	public GameObject iapButton,newtankbttn,shopbttn,Perfecttext,bossPrefab,bossdead;
	public bool giveExtraLife;

	public float ballRate;

	public static bool showIntersitial;
	public static bool levelEnded;

	
	public float startBallRate,startBallPower ;


    public float startBallRateBackUp, startBallPowerBackUp;

	[HideInInspector]
	public static long score;
	public static int blockSpeed ,waveScore;
	public  double rate;
	public float ballRateIncreseBY;
	public float powerIncreseBY;
	public float timeFforMutipleBalls;
	public float timeFforPowerBalls;
	public float timeFforRateBalls;
	public float powerComesAfter;
	[HideInInspector]



	[Header("Constant speed of Ball in X direction")]
	public  float ballConstantSpeed = 2.0f ;

	[HideInInspector]
	public  float blockMoveSpeed = 3.0f ;

	public float dragSestivity ;
	public float addThisValueWhenFiveBlock;
	[Space]


	[HideInInspector]

	public List<GameObject> goList = new List<GameObject>();

	public Transform canvasPlane;

	public bool _tutorialStarted,StopFire;

	public float currentPower;

	public static PlatfromController _instance ;

	public static float simplePower,simpleRate;

	private  LevelSystem _levelSystem;
	private ABTestData _abTestData;
	private LevelPatternsData _levelPatternsData;
	private ShopData _shopData;
	private BossData _bossData;
    private BallData _ballData;

    public BallData ballData
    {
        get
        {
            if (!_ballData)
            {


                string groupName = VoodooSauce.GetPlayerCohort();
                if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewPatternSystem)
                {
                    _ballData = Resources.Load("NewPatterns/BallData") as BallData;

                }
                else
                {

                    _ballData = Resources.Load("Default/BallData") as BallData;
                }
            }
            return _ballData;

        }
    }
	public LevelSystem levelSystemData {
		get {
			if (!_levelSystem) {
				#if Hard
				PlayerPrefs.SetString ("CustomGroup", DimesionEvents.HardGroup.ToString ());
				#endif

				string groupName = VoodooSauce.GetPlayerCohort();
				 {
                    if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewPatternSystem)
                    {
                        _levelSystem = Resources.Load("NewPatterns/LevelSystemData") as LevelSystem;

                    }
                    else
                    {
                        _levelSystem = Resources.Load("Default/LevelSystemData") as LevelSystem;
                    }
				}

			}
			return _levelSystem;
		}
	}
	public ABTestData AbTestData {
		get {
			if (!_abTestData) {

				_abTestData =Resources.Load ("ABTestData") as ABTestData;
			}
			return _abTestData;
		}
	}
	public ShopData ShopData {
		get {
			if (!_shopData) {
                if (StringConstant.ABNewLevelProgressSystem == StringConstant.ABNewLevelProgressSystem) {
					_shopData = Resources.Load ("ShopDataBoss") as ShopData;

                } 
                else {
                    _shopData = Resources.Load("ShopDataNew") as ShopData;
				}
			}
			return _shopData;
		}
	}

	public LevelPatternsData levelPatternsData {
		get {
			if (!_levelPatternsData) {
                if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewPatternSystem)
                {
                    _levelPatternsData = Resources.Load("NewPatterns/LevelPatternsData") as LevelPatternsData;

                }
                else
                {
                    _levelPatternsData = Resources.Load("Default/LevelPatternsData") as LevelPatternsData;
                }
			}
			return _levelPatternsData;
		}
	}

	public BossData bossData {
		get {
			if (!_bossData) {

				_bossData =Resources.Load ("BossData") as BossData;
			}
			return _bossData;
		}
	}
	public static bool onceLifeAsk= true;
	public Transform[] obstaclePosLeft;
	public Transform[] obstaclePosRight;
	public Transform[] obstaclePosRight5;
	public Transform[] obstaclePosRight5Bonus;

	public Transform platfromPos;

	public float timescale;
	public GameObject playerPart,player1,player2,player3 ;
	public GameObject SpwanItemAB,circleObstaleAB,MutipleBallAB;
	public GameObject obstacleLeft,obstacleRight,playerDeathEffect,blockBreak,ballBreakEffect,coinEffect;

	public int increaseDifficultyAfter= 20 ;

	public SpriteRenderer _playerMaterial;
	public static List<GameObject> playersList = new List<GameObject>();
	GameObject _lastplatfrom1 ;
	GameObject _lastplatfrom2;
	GameObject _lastplatfrom3;
	GameObject _lastplatfrom4;
	public float spikescolorChangeSpeed;
	public List<GameObject> leftLine = new List<GameObject> ();
	public List<GameObject> rightline = new List<GameObject> ();
	public static int objectspawnController;
	List<int> randomPattern = new List<int>();
	 int previousPattern = 0;

	List<int> randomLevelPattern = new List<int>();
	 int previousLevelPattern = 0;

	List<int> randomMovingPattern = new List<int>();
	 int previousMovingPattern =0 ;
   public GameObject playDestroy,lifeItemParticle;
   public Transform spriteParent;

	[HideInInspector]
	public int breakParticleCounter;
	public List<Vector3> pickUpObjects = new List<Vector3> ();
    public double coinNum  ;
	bool firstline ;
	// Use this for initialization
	public List<Transform> ballList = new List<Transform> ();

	float startDragValue ;
	float increaseDragValue ;
	public Transform dragFingure; 

	void Awake()
	{
		_instance = this;
		Application.targetFrameRate = 60;
		VoodooSauce.RegisterPurchaseDelegate(new TestPurchaseDelegate());
		Input.gyro.enabled = false;

	}


	void SetPlayerChorot()
	{
		
		#if EasyGame
		{
		PlayerPrefs.SetString ("VoodooSauce_Cohort", StringConstant.ABNewlevelEasy);
		}
		#endif
		#if VibrationANDStar
		{
		PlayerPrefs.SetString ("VoodooSauce_Cohort", StringConstant.ABSoundBlockVibrationANDStar);
		}
		#endif

		#if LevelProgress

		PlayerPrefs.SetString ("VoodooSauce_Cohort", StringConstant.ABNewLevelProgressSystem);
		#endif
        #if NewShopSkins
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewShopSkins);

        #endif
        #if NewShopSkinsHard
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewShopSkinsHard);

        #endif
        #if NewIAP
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewIAPSystem);
        #endif
        #if Banner
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewBannerSystem);

        #endif

        #if Promo
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewCrossSystem);
        #endif

        #if NewPatterns
        PlayerPrefs.SetString("VoodooSauce_Cohort", StringConstant.ABNewPatternSystem);
        #endif
	}
	IEnumerator Start()
	{
		SetPlayerChorot ();
        VoodooSauce.SetInterstitialAdsDisplayConditions(30,30,3);
		Application.runInBackground = false;
		startDragValue = dragSestivity;
		increaseDragValue = dragSestivity + addThisValueWhenFiveBlock;
		//if (VoodooSauce.GetPlayerCohort ()!=null) 

		{
		}
		//Debug.unityLogger.logEnabled = false;
		Application.targetFrameRate= 20000;
		yield return new WaitForSeconds (0f);
	{
			
			UIManager._instance.homeScreen.SetActive (true);
			UIManager._instance.homeScreen.transform.GetComponent<Animator> ().Play ("IN");
            UIManager._instance.boosterButton.gameObject.SetActive(false);

			//StatsPanel._instance.SetStastPanel ();

		}

		InvokeRepeating ("Makeitrue", 0.01f, 0.5f);

		Camera.main.backgroundColor = PlatfromController._instance.levelSystemData.homeScreenColor;
		for (int i = 0; i < 4; i++) {
			randomPattern.Add (i);
		}
		for (int i = 0; i < levelPatternsData.numberofPatterns.Length  ; i++) {
			randomLevelPattern.Add (i);
		}
		for (int i = 0; i < levelPatternsData.numberofMovingLevels.Length ; i++) {
			randomMovingPattern.Add (i);

		}


		previousPattern = ShuffleArray (randomPattern,previousPattern);
		previousLevelPattern = ShuffleArray (randomLevelPattern,previousLevelPattern);
		previousMovingPattern = ShuffleArray (randomMovingPattern,previousMovingPattern);


		if (PlayerPrefs.HasKey ("NoAds")|| VoodooSauce.IsPremium()) {
			iapButton.SetActive (false);
		} else {
			iapButton.SetActive (true);

		}
		if (VoodooSauce.GetPlayerCohort () == StringConstant.ABShop || StringConstant.ABShopMissions == StringConstant.ABShopMissions) {
			newtankbttn.SetActive (false);
			shopbttn.SetActive (true);
		} else {
			newtankbttn.SetActive (true);
			shopbttn.SetActive (false);
		}

		VoodooSauce.ShowBanner(() => Debug.Log("BANNER DISPLAYED (height "));
        if (!PlayerPrefs.HasKey("ShopControl"))
        {
            PlayerPrefs.SetInt("ShopControl", 1);
            ShopController._instance.Setprogrees(TankUnlockMission.PowerUpgradeLevel, 1);
            ShopController._instance.Setprogrees(TankUnlockMission.SpeedUpgradeLevel, 1);
        }


        yield return new WaitForSeconds(0.1f);
        if(StringConstant.ABNewIAPSystem == StringConstant.ABNewIAPSystem)
        {
            if (!PlayerPrefs.HasKey("FirstTimeNotify"))
            {
                PlayerPrefs.SetInt("FirstTimeNotify",1);
                UIManager._instance.ShowNotificationpanelFirstTime();
            }
            UIManager._instance.boosterButton.gameObject.SetActive(true);

        }
        if (PlayerPrefs.GetInt("newSkinSelect") == 0)
        {
            UIManager._instance.playerSprite.sprite = PlatfromController._instance.ShopData.numberOfTank[PlayerPrefs.GetInt("SelectCurrentTank")].tankSkin;
        }
        else
        {
            UIManager._instance.playerSprite.sprite = PlatfromController._instance.ShopData.numberOfTank[PlayerPrefs.GetInt("SelectCurrentTank")].tankNewSkins[PlayerPrefs.GetInt("CurrentTankSkin")].tankSkin;

        }
        UIManager._instance.CheckShopnotification();
	}
	int timer ;
	float timerInGame ;

	string PrefsCohort = "VoodooSauce_Cohort";

	public void OnFSShow(int adnumber) {
		if (VoodooSauce.GetPlayerCohort ()!=null) 
		{
			
			#if !Abhi

		Value props = new Value();
		props["FS number"] = adnumber;
		// other props here...

		Mixpanel.people.Increment("FS show", 1);

		Mixpanel.Track("FS show", props);
			#endif
		}
	}
	public void OnRVShow(int adnumber) {
		if (VoodooSauce.GetPlayerCohort ()!=null) 
		{
			
			#if !Abhi

		Value props = new Value();
		props["RV number"] = adnumber;
		// other props here...

		Mixpanel.people.Increment("RV show", 1);

		Mixpanel.Track("RV show", props);
			#endif
		}
	}
	public void OnRVOpprotunity(int rvOpprotunity) {
		if (VoodooSauce.GetPlayerCohort ()!=null) 
		{
			#if !Abhi

		Value props = new Value();
		props["RV opportunity number"] = rvOpprotunity;
		// other props here...

		Mixpanel.people.Increment("RV opportunity presented", 1);

		Mixpanel.Track("RV opportunity presented", props);
			#endif
		}
	}
	public void StopTimer()
	{
		if (IsInvoking ("IncreaseTime")) {
			CancelInvoke ("IncreaseTime");

		}
		StopGameTimer ();
	}
	public void StopGameTimer()
	{
		if (IsInvoking ("IncreaseTimeGame")) {
			CancelInvoke ("IncreaseTimeGame");

		}
	}
	public void StartCounter()
	{
		timer = 0; 
		PlatfromController.showIntersitial = false;

		if (IsInvoking ("IncreaseTime")) {
			CancelInvoke ("IncreaseTime");

		}
		if(!IsInvoking("IncreaseTime"))
		{
			InvokeRepeating ("IncreaseTime", 1, 1);
		}
	}


	public void StartGameCounter()
	{
		PlatfromController.levelEnded = false;
        UIManager._instance.bounsText.gameObject.SetActive((false));
        UIManager._instance.bonusCheckmark.gameObject.SetActive((false));
        UIManager._instance.levelText.gameObject.SetActive((true));

		int level = PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString());

		for (int i = 0; i < levelSystemData.numberOfProgressLevels.Length; i++) {
			Debug.LogError ("level" + level);

			if (level >= levelSystemData.numberOfProgressLevels [i].MinlevelNumber && level <= levelSystemData.numberOfProgressLevels [i].MaxlevelNumber) {
				timeForComplete =(int) levelSystemData.numberOfProgressLevels [i].timeForlevelInSeconds; 
				Debug.LogError ("counter" + timeForComplete);

			}
		}

		if (IsInvoking ("IncreaseTimeGame")) {
			CancelInvoke ("IncreaseTimeGame");

		}
		if(!IsInvoking("IncreaseTimeGame"))
		{
			InvokeRepeating ("IncreaseTimeGame", 1, 0.01f);
		}
	}
	float timeForComplete= 0 ;


	public static bool bossSpawn,bossDestroy;


	void StopBlinkEffect()
	{
		UIManager._instance.blinkEffect.gameObject.SetActive (false);

	}

	void IncreaseTimeGame()
	{
		
			if(!bossSpawn)
			{
			timerInGame = timerInGame + 0.01f;
			UIManager._instance.levelprogressSlider.value = timerInGame / timeForComplete;
			if (UIManager._instance.levelprogressSlider.value > 0.98f) {
                if ((PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString ()) + 1) % afterNumberOfGamesBossCome == 0)
                if (bossData.numberOfBoss.Length > (PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString ()) + 1) / afterNumberOfGamesBossCome) {
					if (!bossDestroy) {
						bossSpawn = true;
						isSpawnBlockCorutineON = false;

						StopCoroutine ("SpawnPlatfromRight");

                        GameObject boss = Instantiate (bossData.numberOfBoss [((PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString ()) + 1) / afterNumberOfGamesBossCome)-1].bossPrefab) as GameObject;
						boss.GetComponent<BossManager> ().priorityNumber = rate * Random.Range (levelSystemData.bossWall.ratioMin, levelSystemData.bossWall.ratioMax);
                        boss.GetComponent<BossManager> ().bossNumber = ((PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString ()) + 1) / afterNumberOfGamesBossCome)-1;
						boss.SetActive (false);
						boss.SetActive (true);
						UIManager._instance.blinkEffect.gameObject.SetActive (true);
						Invoke ("StopBlinkEffect", 4f);
					} else {
						if (!isSpawnBlockCorutineON) {
							
							StartCoroutine ("SpawnPlatfromRight");
						}
					}
				}
			
			}
			if (timerInGame > timeForComplete) {
				PlatfromController.levelEnded = true;
                UIManager._instance.bounsText.gameObject.SetActive((true));
                UIManager._instance.bonusCheckmark.gameObject.SetActive((true));
                UIManager._instance.levelText.gameObject.SetActive((false));
				CancelInvoke ("IncreaseTimeGame");
				int level = PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString());
				level++;
				PlayerPrefs.SetInt (GamePlayerPrefs.Gamelevelprogress.ToString(),level);

			}
			}

	}


	void IncreaseTime()
	{
		{
			timer = timer + 1;
			Debug.Log ("counter" + timer);

			if (timer < UIManager._instance.ballData.easyGameDeciderTime) {
				PlatfromController.showIntersitial = true;
			} else {
				PlatfromController.showIntersitial = false;

				CancelInvoke ("IncreaseTime");

			}
		} 
	}
		
	void Makeitrue()
	{
				PlatfromController._instance.stopMoving = false;

	}
	public void SpawnPlayer()
	{
		if (!TankController._instance || _tutorialStarted ) {
			UIManager._instance.character.gameObject.SetActive (false);
			int tankNumber = PlayerPrefs.GetInt ("TankNumber");

			if (tankNumber == 3) {
				Instantiate (player3);
			} else if (tankNumber == 2) {
				Instantiate (player2);

			} else {
				Instantiate (player1);

			}

		}
		UIManager.gameStarted = true;
		startFrompLay = true;
		PlatfromController._instance.moveTank = true;

	}


	public void SpwanPlayerTutorial(float time)
	{
		Invoke ("SpawnPlayerTutorial", time);
		if (PlatfromController._instance.sheildLife) {
			PlatfromController._instance.sheildLife = false;
		}
	}
	 void SpawnPlayerTutorial()
	{
		if (!TankController._instance || _tutorialStarted ) {
			UIManager._instance.character.gameObject.SetActive (false);

			Instantiate (player1);

		}
		UIManager.gameStarted = true;
		startFrompLay = true;
		PlatfromController._instance.moveTank = true;

	}
	[HideInInspector]
	public volatile bool stopMoving ;

	public volatile bool moveTank ;

	public bool  sheildLife ;
	void StartGamePlay () {
		CameraMovement.highScore = false;

		 coinNum =0;
		StopCoroutine("SpawnPlatfromRight");
		PlatfromController.playersList.Clear ();

		if (TankController._instance) {
			Destroy (TankController._instance.gameObject);
			PlatfromController.playersList.Remove (TankController._instance.gameObject);

		}
		if (PlatfromController._instance.ShopData.numberOfTank [PlayerPrefs.GetInt ("SelectCurrentTank")].tankAbility == TankAbility.Shield) {
			sheildLife = true;
		} else {
			sheildLife = false;
		}

		onceLifeAsk = true;
		PlatfromController.score = 0;
		PlatfromController.waveScore = levelSystemData.numberOfLevels [PlayerPrefs.GetInt ("SkipThisLevel") + 1].MinScore;


		PlatfromController._instance.pickUpObjects.Clear ();
		GameObject[] part = GameObject.FindGameObjectsWithTag ("part");
		for (int j = 0; j < part.Length; j++) {
			Destroy (part[j]);
		}
		GameObject[] part10 = GameObject.FindGameObjectsWithTag ("MBall");
		for (int j = 0; j < part10.Length; j++) {
			Destroy (part10[j]);
		}
		GameObject[] destroy = GameObject.FindGameObjectsWithTag ("Obstacles");
		for (int j = 0; j < destroy.Length; j++) {
			Destroy (destroy[j]);
		}
		GameObject[] destroy2 = GameObject.FindGameObjectsWithTag ("SpawnItem");
		for (int j = 0; j < destroy2.Length; j++) {
			Destroy (destroy2[j]);
		}
		GameObject[] destroy3 = GameObject.FindGameObjectsWithTag ("Destroy");
		for (int j = 0; j < destroy3.Length; j++) {
			Destroy (destroy3[j]);
		}
		UIManager._instance.gameOver.transform.gameObject.SetActive (false);
		//playersList.Clear ();
		if (!startFrompLay) {
			UIManager._instance.homeScreen.SetActive (true);
			UIManager._instance.homeScreen.transform.GetComponent<Animator> ().Play ("IN");
            UIManager._instance.ShowNotificationpanel();

			UIManager._instance.newScoreImage.gameObject.SetActive (false);
			ShopController._instance.Setprogrees (TankUnlockMission.PlayNumberOFGames, 1);
			UIManager.gameStarted = false;
			UIManager._instance.character.gameObject.SetActive (true);
			transform.GetComponent<BoxCollider> ().enabled = true;
			Camera.main.orthographicSize = 5;
			PlatfromController._instance.FourWaveBlock ();

            if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewCrossSystem)
            {
                UIManager._instance.ShowHomeScreenCrosspromo();
            }

		} else {
            if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewCrossSystem)
            {
                UIManager._instance.HideCrosspromo();
            }
            ShopController._instance.newBoxSkinUnlockedChild.Clear();
            ShopController._instance.newBoxSkinUnlockedParent.Clear();

			Camera.main.orthographicSize = 5;
			PlatfromController._instance.FourWaveBlock ();
			UIManager._instance.newScoreImage.gameObject.SetActive (false);
			UIManager._instance.playerScoretext.color = PlatfromController._instance.levelSystemData.scoreTextColor;

			UIManager._instance.recordtext.text = PlatfromController._instance.ShowValueHIghScore();
			UIManager._instance.homeScreen.transform.GetComponent<Animator> ().Play ("Out");
			//UIManager._instance.coinBox.transform.GetComponent<Animator> ().Play ("Out");
            PlatfromController.bossSpawn = false;


			if (_tutorialStarted) {
				
				StartCoroutine ("StartTutorial");

			} else {
                HomeStats._instance.SetValue();

                if (StringConstant.ABNewIAPSystem == StringConstant.ABNewIAPSystem)
                {
                    CheckBooster();
                }

				SpwanPlatfromRight ();
				StartCoroutine (StartGameAfterAnimation ());
				UIManager._instance.playerScoretext.transform.gameObject.SetActive (true);
				UIManager._instance.playerScoretext.color = PlatfromController._instance.levelSystemData.scoreTextColor;
				if (VoodooSauce.GetPlayerCohort () == StringConstant.ABNewlevelEasy) {
					StartCounter ();
				}
                if (StringConstant.ABNewLevelProgressSystem == StringConstant.ABNewLevelProgressSystem) {
                    timerInGame = 0; 

					StartGameCounter ();
					UIManager._instance.levelprogressSlider.gameObject.SetActive (true);
					UIManager._instance.levelprogressSlider.value = 0;
					UIManager._instance.reachlevelText.text =  (PlayerPrefs.GetInt (GamePlayerPrefs.Gamelevelprogress.ToString())+1).ToString();
					PlatfromController.bossDestroy = false;
					PlatfromController.bossSpawn = false;


				}
			}
			transform.GetComponent<BoxCollider> ().enabled = false;

		}
		showstarBlocknumber = Random.Range (levelSystemData.MinStarBlockCome, levelSystemData.MaxStarBlockCome);

	}


     TankAbility currentTankAbility;

    public void CheckBooster()
    {
        if(UIManager._instance.boosterIN)
        {
            UIManager._instance.boosterIN = false;
            UIManager._instance.boosterButton.transform.GetComponent<Animator>().Play("Out");

        }
        currentTankAbility = PlatfromController._instance.ShopData.numberOfTank[PlayerPrefs.GetInt("SelectCurrentTank")].tankAbility;

        isPointsBooster = false;
        isPowerBooster = false;
        isSpeedBooster = false;
        startBallRate = startBallRateBackUp;
        startBallPower = startBallPowerBackUp;
        if (UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfPointsBooster.ToString()) > 0)
        {
            long numberOfBooster = UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfPointsBooster.ToString()) - 1;
            UIManager._instance.SetPlayerPrefsValue(GamePlayerPrefs.NumberOfPointsBooster.ToString(), numberOfBooster.ToString());
            isPointsBooster = true;
        }
        if (UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfPowerBooster.ToString()) > 0)
        {
            long numberOfBooster = UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfPowerBooster.ToString()) - 1;
            UIManager._instance.SetPlayerPrefsValue(GamePlayerPrefs.NumberOfPowerBooster.ToString(), numberOfBooster.ToString());
            isPowerBooster = true;

            if(currentTankAbility == TankAbility.Power)
            {
                startBallPower = startBallPower * 1.15f;
  
            }else{
                startBallPower = startBallPower * 1.3f;

            }
        }
        if (UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfSpeedBooster.ToString()) > 0)
        {
            long numberOfBooster = UIManager._instance.GetPlayerPrefsValue(GamePlayerPrefs.NumberOfSpeedBooster.ToString()) - 1;
            UIManager._instance.SetPlayerPrefsValue(GamePlayerPrefs.NumberOfSpeedBooster.ToString(), numberOfBooster.ToString());
            isSpeedBooster = true;
            if (currentTankAbility == TankAbility.Power)
            {
                startBallRate = startBallRate *  1.5f;
   
            }else{
                startBallRate = startBallRate * 1.5f;

            }
        }
    }
	public int numberOfBlock ;
	public bool destroySmallPlatfrom,collectPowerUP,normalTutorialBlock,destroyNormalTutorialBlock,tankNeeded;

	IEnumerator StartTutorial()
	{
		numberOfBlock = 0;
		tankNeeded = true;
		destroySmallPlatfrom = false;
		collectPowerUP = false;
		normalTutorialBlock = false;
		destroyNormalTutorialBlock = false;
		PlatfromController._instance.stopMoving = false;
		StopFire = true;
		UIManager._instance.character.gameObject.SetActive (false);

		yield return new WaitForSeconds (0.1f);
		UIManager._instance.tutorial.SetActive (true);
		UIManager._instance.playerScoretext.transform.gameObject.SetActive (true);
		UIManager._instance.playerScoretext.color = PlatfromController._instance.levelSystemData.scoreTextColor;

		//UIManager._instance.playerScoretext.transform.GetComponent<Animator> ().Play ("IN");
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("IN");
		Color c = TutorialManager._instance.checkMarkImage.GetComponent<Image> ().color;
		c.a = 0;
		TutorialManager._instance.checkMarkImage.GetComponent<Image> ().color = c;

	//	UIManager._instance.coinBox.transform.GetComponent<Animator> ().Play ("IN");
		if (PlatfromController._instance._tutorialStarted) {
			PlatfromController._instance.ballRate = UIManager._instance.ballData.tutorialBallRate;
			PlatfromController._instance.currentPower = UIManager._instance.ballData.tutorialBallPower;

		}
		SpawnPlayer ();

		if (PlatfromController._instance._tutorialStarted) {
			PlatfromController._instance.ballRate = UIManager._instance.ballData.tutorialBallRate;
			PlatfromController._instance.currentPower = UIManager._instance.ballData.tutorialBallPower;

		}
		TutorialManager._instance.hand.gameObject.SetActive (false);
		//TutorialManager._instance.completext.gameObject.SetActive (false);
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Start");
		//TutorialManager._instance.completext.transform.GetComponent<Text> ().enabled = false;
		//TutorialManager._instance.checkMarkImage.transform.GetComponent<Image> ().enabled = false;
		TutorialManager._instance.fadeScreen.gameObject.SetActive (false);
yield return new WaitForSeconds (0.5f);
		TutorialManager._instance.fadeScreen.gameObject.SetActive (false);
		UIManager._instance.homeScreen.SetActive (false);
		TutorialManager._instance.hand.gameObject.SetActive (true);

		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-1-IN");
		TutorialManager._instance.hand.transform.GetComponent<Animator> ().Play ("rightleft");

		//TutorialManager._instance.tutorialtext.text = UIManager._instance.ballData._tutorial.tutorialText [0];
	//	TutorialManager._instance.rightArrow.gameObject.SetActive (true);
	//	TutorialManager._instance.leftArrow.gameObject.SetActive (true);

yield return new WaitUntil (() => StopFire == false);
		//TutorialManager._instance.tutorialtext.gameObject.SetActive (false);
yield return new WaitForSeconds (3.5f);

        TutorialManager._instance.tutorialtext.gameObject.SetActive (true);
		//TutorialManager._instance.checkMarkImage.transform.GetComponent<Image> ().enabled = true;

		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-1-Out");
		yield return new WaitForSeconds (0.83f);

		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-2-IN");

		yield return new WaitForSeconds (2.03f);

		GenrateTutorialBlock (1,3,2,4,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium);

yield return new WaitUntil (() => destroySmallPlatfrom == true);
		destroySmallPlatfrom = false;
		numberOfBlock = 0;
		GenrateTutorialBlock (1,3,2,4,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium,TypesOfWall.WallMedium);
yield return new WaitUntil (() => destroySmallPlatfrom == true);
	   // TutorialManager._instance.rightArrow.gameObject.SetActive (false);
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-2-Out");
		yield return new WaitForSeconds (0.83f);

		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-3-IN");

	    TutorialManager._instance.tutorialtext.transform.GetComponent<Animator> ().Play ("IN");
		TutorialManager._instance.hand.transform.GetComponent<Animator> ().Play ("left");

		//TutorialManager._instance.tutorialtext.text = UIManager._instance.ballData._tutorial.tutorialText [2];
		if (rightline.Count > 1) {
			for (int i = 0; i < rightline.Count; i++) {
			//	Destroy (rightline [i].gameObject);
				EasyObjectPool.instance.ReturnObjectToPool(rightline [i].gameObject);

			}}
		rightline.Clear ();
		normalTutorialBlock = true;
		yield return new WaitForSeconds (1.0f);
		while (!destroyNormalTutorialBlock) {
			GenrateTutorialBlock (3,3,19,29,TypesOfWall.WallSmall,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge);
			yield return new WaitForSeconds (4.5f);
		}
		yield return new WaitUntil (() => destroyNormalTutorialBlock == true);
		yield return new WaitForSeconds (0.5f);
		TutorialManager._instance.hand.transform.GetComponent<Animator> ().Play ("right");
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("rightHighlite");

		destroyNormalTutorialBlock = false;
		while (!destroyNormalTutorialBlock) {
			GenrateTutorialBlock (2,4,15,25,TypesOfWall.WallSmall,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge,true);

			yield return new WaitForSeconds (4.5f);
		}
		yield return new WaitUntil (() => destroyNormalTutorialBlock == true);

		destroyNormalTutorialBlock = false;
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-3-Out");
		yield return new WaitForSeconds (0.83f);
		TutorialManager._instance.hand.transform.GetComponent<Animator> ().Play ("Done");
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-4-IN");
		TutorialManager._instance.hand.gameObject.SetActive (false);
yield return new WaitForSeconds (1.5f);
        while (!collectPowerUP) {
		Instantiate (circleObstaleAB, new Vector3 (-1.25f, 9f, -5), Quaternion.identity);
yield return new WaitForSeconds (4.0f);
		}
		collectPowerUP = false;
		while (!collectPowerUP) {
			Instantiate (SpwanItemAB, new Vector3 (Random.Range (-2, 2), 10f, -5), Quaternion.identity);
			yield return new WaitForSeconds (4.0f);
		}
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("Stage-4-Out");

yield return new WaitForSeconds (2.5f);
		while (!destroyNormalTutorialBlock) {

			GenrateTutorialBlock (3,5,4,6,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge,TypesOfWall.WallLarge);
			yield return new WaitForSeconds (3.5f);
		}
		yield return new WaitUntil (() => destroyNormalTutorialBlock == true);

		TutorialManager._instance.tutorialtext.gameObject.SetActive (false);
		TutorialManager._instance.tutorialtext.gameObject.SetActive (true);
		yield return new WaitForSeconds (0.83f);
		if (TankController._instance) {
			tankNeeded = false;
			Destroy (TankController._instance.gameObject);

		}
		UIManager._instance.tutorial.transform.GetComponent<Animator> ().Play ("End");
yield return new WaitForSeconds (3.5f);
	    _tutorialStarted = false;
		PlayerPrefs.SetInt("TutorialDone",1);
		startFrompLay = false;
		FromHomeStartGame ();
		UIManager._instance.tutorial.SetActive (false);
		TutorialManager._instance.fadeScreen.gameObject.SetActive (false);

	}


	IEnumerator StartGameAfterAnimation()
	{
		PlatfromController._instance.stopMoving = false;
		yield return new WaitForSeconds (0.1f);
		UIManager.gameStarted = true;
		UIManager._instance.playerScoretext.color = PlatfromController._instance.levelSystemData.scoreTextColor;
		UIManager._instance.gamePlay.SetActive (true);
		UIManager._instance.playerScoretext.transform.gameObject.SetActive (true);
		UIManager._instance.playerScoretext.transform.GetComponent<Animator> ().Play ("IN");
		UIManager._instance.gamePlay.transform.GetComponent<Animator> ().Play ("IN");
		SpawnPlayer ();
		yield return new WaitForSeconds (0.5f);
		UIManager._instance.homeScreen.SetActive (false);
	}

	void GenrateTutorialBlock(int fMin,int fMax,int eMin,int eMax,TypesOfWall _firstWall,TypesOfWall _secondWall,TypesOfWall _thirdWall,TypesOfWall _foruthWall,TypesOfWall _fifthWall,bool lowRight=false)
	{
		rightline.Clear ();

		for (int i = 0; i < 4; i++) {
			{
				_lastplatfrom1 = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight [i].position.x, obstaclePosRight [i].position.y, -5f), Quaternion.identity);

				//_lastplatfrom1 = Instantiate (obstacleRight, new Vector3 (obstaclePosRight[i].position.x,obstaclePosRight[i].position.y, -5f), Quaternion.identity) as GameObject;
			}
			_lastplatfrom1.GetComponent<BoxCollider> ().enabled = true;
			_lastplatfrom1.GetComponent<Platfrom> ().onceSeted = false;
			_lastplatfrom1.GetComponent<Platfrom> ().makeSpeedLess = 0;
			_lastplatfrom1.GetComponent<Platfrom> ().bonusBlock = false;
			_lastplatfrom1.GetComponent<Platfrom> ().blockCounted = false;
			_lastplatfrom1.GetComponent<Platfrom> ().bossblock = false;
            _lastplatfrom1.GetComponent<Platfrom>().nonDestroyBlock = false;
			_lastplatfrom1.GetComponent<Platfrom> ().blockMoveType = BlockMoveType.None;
			_lastplatfrom1.GetComponent<Platfrom> ().distanceToMove = 0f;
			_lastplatfrom1.GetComponent<Platfrom> ().sideMovespeed = 0f;


			_lastplatfrom1.GetComponent<Platfrom> ().typeOfPlatfrom = Platfromtype.RightBlock;
			rightline.Add (_lastplatfrom1);
			_lastplatfrom1.transform.SetParent (spriteParent);
			_lastplatfrom1.transform.localPosition = new Vector3 (obstaclePosRight [i].position.x, obstaclePosRight [i].position.y, -5f);
			_lastplatfrom1.transform.localScale = Vector3.one;
			if (!lowRight) {
				if (i == 0) {
					_lastplatfrom1.GetComponent<Platfrom> ().priorityNumber = Random.Range (fMin, fMax);
					_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _firstWall;
				} else {
					_lastplatfrom1.GetComponent<Platfrom> ().priorityNumber = Random.Range (eMin, eMax);
					if (i == 1) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _secondWall;
					} else if (i == 2) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _thirdWall;

					} else if (i == 3) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _foruthWall;

					}
				}
			} else {
				if (i == 3) {
					_lastplatfrom1.GetComponent<Platfrom> ().priorityNumber = Random.Range (fMin, fMax);
					_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _firstWall;
				} else {
					_lastplatfrom1.GetComponent<Platfrom> ().priorityNumber = Random.Range (eMin, eMax);

					if (i == 1) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _secondWall;
					} else if (i == 2) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _thirdWall;

					} else if (i == 0) {
						_lastplatfrom1.GetComponent<Platfrom> ().typeofwall = _foruthWall;

					}
				}
			}
			_lastplatfrom1.GetComponent<Platfrom> ().SetValues ();
		}
	}
	int incraser_counter ,starBlockLineCount;
	public static bool startFrompLay;
	public int numberofGame;

	public void StartGame()
	{
		if (!startFrompLay) {
			//System.GC.Collect ();
			//Resources.UnloadUnusedAssets ();
			startFrompLay = true;
			createBonus = false;
			UIManager._instance.recordtext.text = PlayerPrefs.GetInt ("HighScore").ToString ();
			long currentprogress = UIManager._instance.GetPlayerPrefsValue ("NumberOFGame");
			currentprogress = currentprogress + 1;
			UIManager._instance.SetPlayerPrefsValue ("NumberOFGame", currentprogress.ToString ());
			numberofGame = numberofGame + 1;
			StartGamePlay ();
			UIManager._instance.playerScoretext.text = "0";
			numberOflifes = 0;
			enemy.Clear ();
			{
				{
			VoodooSauce.OnGameStarted();
				}
			}
		}
	}
	public void FromHomeStartGame()
	{
		startFrompLay = false;
		UIManager._instance.recordtext.text = PlayerPrefs.GetInt ("HighScore").ToString ();
		StartGamePlay ();
		StatsPanel._instance = UIManager._instance.statsPanellevel.GetComponent<StatsPanel> ();
	}


	public void StopCortuineOFBlocks()
	{
		StopCoroutine("SpawnPlatfromRight");
		isSpawnBlockCorutineON = false;

	}
	public void SpwanPlatfromRight()
	{
		UIManager._instance.playerScoretext.text = PlatfromController._instance.ShowValueScore();;
		StopCoroutine("SpawnPlatfromRight");
		StartCoroutine ("SpawnPlatfromRight");
	}

	int lineNumber ;
	int currentLevel, currentIndex;
	int currentLevellevelp, currentIndexlevelp;
	int currentLevelmovelevelp, currentIndexmovelevelp;


	public  int GetSelectAndGenerateRandomPattern ()
	{
		if (currentIndex > randomPattern.Count - 1) {
			previousPattern = ShuffleArray (randomPattern,previousPattern);
			currentIndex = 0;
		}
		currentLevel = randomPattern [currentIndex];
		currentIndex++;
		return currentLevel ;
	}
	public  int GetSelectAndGenerateRandomLevelPattern ()
	{
		if (currentIndexlevelp > randomLevelPattern.Count - 1) {
			previousLevelPattern = ShuffleArray (randomLevelPattern,previousLevelPattern);
			currentIndexlevelp = 0;
		}
		currentLevellevelp = randomLevelPattern [currentIndexlevelp];
		currentIndexlevelp++;
		return currentLevellevelp ;
	}
	public  int GetSelectAndGenerateRandomMovingPattern ()
	{

		if (currentIndexmovelevelp > (randomMovingPattern.Count - 1)) {
			previousMovingPattern = ShuffleArray (randomMovingPattern,previousMovingPattern);
			currentIndexmovelevelp = 0;
		}
		currentLevelmovelevelp = randomMovingPattern [currentIndexmovelevelp];
		currentIndexmovelevelp++;
		return currentLevelmovelevelp ;
	}
	public Vector3 tanklatestPos,refrencePos ;
	void OnMouseDown()
	{
		PlatfromController._instance.stopMoving = false;
	}
	 void OnMouseDrag()
	{
	if(PlatfromController._instance._tutorialStarted)
		StopFire = false;
		
		if(!PlatfromController._instance.stopMoving)
		{
			if (TankController._instance) {
				PlatfromController._instance.moveTank = true;
				#if UNITY_EDITOR
			    TankController._instance.transform.position += new Vector3 ((Input.GetAxis ("Mouse X") * dragSestivity), 0f);
				#else
				TankController._instance.transform.position += new Vector3 ((Input.GetAxis ("Mouse X") * dragSestivity)/2, 0f);
				#endif
			}
		}
	}
	Vector3 _drag, refrence;
	public void OnMouseDragO()
	{
	if (giveExtraLife) {
			dragFingure.transform.gameObject.SetActive (false);
            if (StringConstant.ABNewLevelProgressSystem == StringConstant.ABNewLevelProgressSystem)
            {
                StartGameCounter();
            }

			giveExtraLife = false;
			PlatfromController._instance.StopFire = false;
			StopCoroutine("SpawnPlatfromRight");
			StartCoroutine ("SpawnPlatfromRight");
		}
		if(PlatfromController._instance._tutorialStarted)
		StopFire = false;

		if(!PlatfromController._instance.stopMoving)
		{
				if (TankController._instance) {
				PlatfromController._instance.moveTank = true;
				float xValue = Input.GetAxis ("Mouse X") * dragSestivity ;
				#if UNITY_EDITOR
				TankController._instance.transform.position += new Vector3 ((xValue), 0f);
				#else
				TankController._instance.transform.position += new Vector3 ((xValue)/2, 0f);
				#endif

				}
			}
		}

	int ShuffleArray (List<int> arr,int _previouseValue)
	{
	for (int i = arr.Count - 1; i > 0; i--) {
			int r = UnityEngine.Random.Range (0, i + 1);
			int tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
		try{
		if (arr [0] == _previouseValue) {
			int tmp = arr [0];
			arr [0] = arr [1];
			arr [1] = tmp;
			}}catch(System.ArgumentOutOfRangeException e) {
			return 0;

		}
		_previouseValue = arr [arr.Count - 1];
		return _previouseValue;
	}
	Vector3 lasttrnsfromPos ;

   public void SetDifficultyRight()
	{
	for (int i = 0; i < rightline.Count; i++) {
			SetPriority (rightline [i],rightline.Count,i);
		}
	}

	public IEnumerator MovePos(GameObject objectMove,Vector3 movetoPos)
	{
		float time = 0;
		while (time < 1f) {
			time += 0.00001f;
			objectMove.transform.position = Vector3.Lerp (objectMove.transform.position, movetoPos, time);
		}
		yield return new WaitForEndOfFrame ();
	}

	public static int previousLineNumber=1;
	float  timeTowait ;
	public void WaitingTime()
	{
		for (int i = 0; i < levelSystemData.waveTimesLevels.Length; i++) {
			if (PlatfromController.waveScore >= levelSystemData.waveTimesLevels [i].MinScore && PlatfromController.waveScore <= levelSystemData.waveTimesLevels [i].MaxScore)
			{
			timeTowait = levelSystemData.waveTimesLevels [i].waitingTime;
			}
		}
	}
	TypeOfLevels currentTypeOfLevel ;

	bool createBonus ,bonusCreated;

	bool isSpawnBlockCorutineON ;
	public IEnumerator SpawnPlatfromRight()
	{
        if (!bossSpawn)
        {
            isSpawnBlockCorutineON = true;
            destroySmallPlatfrom = false;
            numberOfBlock = 0;
            onlyOneScore = false;
            yield return new WaitForSeconds(0.5f);
            while (!destroySmallPlatfrom)
            {
                WaitingTime();
                yield return new WaitForEndOfFrame();
                numberOfBlock = 0;
                rightline.Clear();
                lineNumber++;
                if (PlayerPrefs.GetInt("CreteBonusPattern") == 1 && PlatfromController.playersList.Count > 0 && !createBonus && levelSystemData.showBonuspatterns)
                {



                    CreteBonusPattern();
                    createBonus = true;
                    bonusCreated = true;
                    yield return new WaitForSeconds(4f);

                }
                else
                {
                    SetTypesOfWall();

                    if (numberOfBonusBlocks.Count < 1 && bonusCreated)
                    {
                        long bonusNumber = UIManager._instance.GetPlayerPrefsValue("BonusLevelNumber");
                        bonusNumber = bonusNumber + 1;
                        UIManager._instance.SetPlayerPrefsValue("BonusLevelNumber", bonusNumber.ToString());
                        bonusCreated = false;
                        Debug.LogError("Next ");
                    }
                    PlatfromController.waveScore++;


                    if (currentTypeOfLevel == TypeOfLevels.FourBlock || currentTypeOfLevel == TypeOfLevels.FiveBlock)
                    {
                        CreateNormallevel();
                        if (!destroySmallPlatfrom)
                            yield return new WaitForSeconds(timeTowait);
                    }
                    else if (currentTypeOfLevel == TypeOfLevels.PatternsLevel)
                    {
                        CretePattern();

                        if (!destroySmallPlatfrom)
                            yield return new WaitForSeconds(3f);
                    }
                    else if (currentTypeOfLevel == TypeOfLevels.MovingLevels)
                    {
                        CreteMovingPattern();
                        if (!destroySmallPlatfrom)
                            yield return new WaitForSeconds(3f);
                    }
                    else if (currentTypeOfLevel == TypeOfLevels.FiveBlock_PatternLevel)
                    {

                        int randomeCome = Random.Range(0, 100);
                        if (randomeCome > 74)
                        {
                            CretePattern();
                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(3f);
                        }
                        else
                        {
                            CreateNormallevel();
                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(timeTowait);
                        }

                    }
                    else if (currentTypeOfLevel == TypeOfLevels.FiveBlock_MovingLevels)
                    {
                        int randomeCome = Random.Range(0, 100);
                        if (randomeCome > 74)
                        {
                            CreteMovingPattern();

                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(3f);
                        }
                        else
                        {
                            CreateNormallevel();
                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(timeTowait);
                        }
                    }
                    else if (currentTypeOfLevel == TypeOfLevels.FiveBlock_PatternLevel_MovingBlock)
                    {
                        int randomeCome = Random.Range(0, 100);

                        if (randomeCome > 74)
                        {
                            CreteMovingPattern();

                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(3f);
                        }
                        else if (randomeCome > 49)
                        {
                            CretePattern();

                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(3f);
                        }
                        else
                        {
                            CreateNormallevel();
                            if (!destroySmallPlatfrom)
                                yield return new WaitForSeconds(timeTowait);
                        }
                    }else if(currentTypeOfLevel == TypeOfLevels.HardStaticPatterLevel)
                    {
                        CreteHardStaticPattern();
                        if (!destroySmallPlatfrom)
                            yield return new WaitForSeconds(timeTowait);
                    }else if(currentTypeOfLevel == TypeOfLevels.HardMovingpatternLevel)
                    {
                        CreteHardMovingPattern();
                        if (!destroySmallPlatfrom)
                            yield return new WaitForSeconds(3f);
                    }
                }
                yield return new WaitForEndOfFrame();
                UIManager._instance.playerScoretext.text = ShowValueScore();
                onlyOneScore = false;
            }
            SpwanPlatfromRight();
        }
	}

	[HideInInspector]
	public Dictionary<int,long> storedObject = new Dictionary<int, long>() ;


	public void CreateNormallevel()
	{
		int blockLegth = 0;
		if (!callFiveWave) {
			blockLegth =4;
		} else {
			blockLegth = 5;
		}
		for (int i = 0; i < blockLegth; i++) {
			if (!callFiveWave) {
				_lastplatfrom1 = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight [i].position.x, obstaclePosRight [i].position.y, -5f), Quaternion.identity);
				//_lastplatfrom1 = Instantiate (obstacleRight, new Vector3 (obstaclePosRight [i].position.x, obstaclePosRight [i].position.y, -5f), Quaternion.identity) as GameObject;
			} else {
				_lastplatfrom1 = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y, -5f), Quaternion.identity);

				//_lastplatfrom1 = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y, -5f), Quaternion.identity) as GameObject;
			}

			starBlockLineCount++;
			_lastplatfrom1.GetComponent<Platfrom> ().showStar = false;

            if (StringConstant.ABSoundBlockVibrationANDStar== StringConstant.ABSoundBlockVibrationANDStar)
			{
				if (starBlockLineCount == showstarBlocknumber) {

					Debug.LogError ("Showblock");
					starBlockLineCount = 0;
					showstarBlocknumber = Random.Range (levelSystemData.MinStarBlockCome, levelSystemData.MaxStarBlockCome);
					_lastplatfrom1.GetComponent<Platfrom> ().showStar = true;

				}
			}

			_lastplatfrom1.GetComponent<BoxCollider> ().enabled = true;
			_lastplatfrom1.GetComponent<Platfrom> ().onceSeted = false;
			_lastplatfrom1.GetComponent<Platfrom> ().bonusBlock = false;
			_lastplatfrom1.GetComponent<Platfrom> ().blockCounted = false;
			_lastplatfrom1.GetComponent<Platfrom> ().bossblock = false;
            _lastplatfrom1.GetComponent<Platfrom>().nonDestroyBlock = false;

			_lastplatfrom1.GetComponent<Platfrom> ().blockMoveType = BlockMoveType.None;
			_lastplatfrom1.GetComponent<Platfrom> ().distanceToMove = 0f;
			_lastplatfrom1.GetComponent<Platfrom> ().sideMovespeed = 0f;

			_lastplatfrom1.GetComponent<Platfrom> ().typeOfPlatfrom = Platfromtype.RightBlock;
			_lastplatfrom1.GetComponent<Platfrom> ().lineNumber = lineNumber; 
			rightline.Add (_lastplatfrom1);
			_lastplatfrom1.transform.SetParent (spriteParent);

			//storedObject.Add (_lastplatfrom1, lineNumber);

			if (!callFiveWave) {
				_lastplatfrom1.transform.localPosition = new Vector3 (obstaclePosRight [i].position.x, obstaclePosRight [i].position.y, -5f);
			} else {
				_lastplatfrom1.transform.localPosition = new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y, -5f);
			}

			_lastplatfrom1.transform.localScale = Vector3.one;
		}


		SetDifficultyRight ();
		incraser_counter++;

		if (incraser_counter % powerComesAfter == 0) {
			SpawnPower ();
		}
	}

			int showstarBlocknumber ;
	public string ShowValueScore()
	{
		string value = UIManager._instance.GetAbreviation (PlatfromController.score);
		return value;
	}
	public string ShowValueHIghScore()
	{
		string value =  UIManager._instance.GetAbreviation(UIManager._instance.GetPlayerPrefsValue (GamePlayerPrefs.highScore.ToString ()));
		return value;
	}
	public static bool onlyOneScore;
	public void SetDifficultyLeft()
	{
	for (int i = 0; i < leftLine.Count; i++) {
			SetPriority (leftLine [i],leftLine.Count,i);
		}
	}
	GameObject spwanItem,circleObstacle2 ,coinObject;
	[HideInInspector]
	public int numberOflifes;

	List<GameObject> enemy = new List<GameObject>();

	public void CheckInitPos(GameObject _object)
	{

	}


	public void SpawnPower()
	{
	int radompower = Random.Range (0, 3);
	if (radompower == 0) {
		Instantiate (SpwanItemAB, new Vector3 (Random.Range (-2, 2), 10f, -5), Quaternion.identity);
		} else if (radompower == 1) {
		Instantiate (circleObstaleAB, new Vector3 (Random.Range (-2, 2), 10f, -5), Quaternion.identity) ;
		} else if (radompower == 2) {
		Instantiate (MutipleBallAB, new Vector3 (Random.Range (-2, 2), 10f, -5), Quaternion.identity) ;
		}
	}

    public long SetpriorityAccordingToWall(TypesOfWall typeofwall)
	{
		if (typeofwall == TypesOfWall.Wall0) {
			long value = 
                (long)(rate * (Random.Range (levelSystemData.wall0.ratioMin, levelSystemData.wall0.ratioMax)));
			return value ;
		}else if(typeofwall == TypesOfWall.WallSmall)
		{
            long value = (long) (rate * (Random.Range (levelSystemData.wallSmall.ratioMin, levelSystemData.wallSmall.ratioMax)));
			return value ;
		}else if(typeofwall == TypesOfWall.WallMedium)
		{
            long value = (long) (rate * (Random.Range (levelSystemData.wallMedium.ratioMin, levelSystemData.wallMedium.ratioMax)));
			return value ;
		}else if(typeofwall == TypesOfWall.WallLarge)
		{
            long value = (long) (rate * (Random.Range (levelSystemData.wallLarge.ratioMin, levelSystemData.wallLarge.ratioMax)));
			return value ;
		}
		return 0 ;
	}

	public void SetPriority(GameObject _lasttPlatfrom,int numberOfitem,int itemNumber)
	{

		_lasttPlatfrom.transform.GetComponent<Platfrom> ().typeofwall = GetTypeOfWall (itemNumber);
		long priotiyNUmber = SetpriorityAccordingToWall (_lasttPlatfrom.transform.GetComponent<Platfrom> ().typeofwall);

		if (priotiyNUmber > 0) {
			_lasttPlatfrom.transform.GetComponent<Platfrom> ().priorityNumber = priotiyNUmber;
		} else {
			_lasttPlatfrom.transform.GetComponent<Platfrom> ().priorityNumber = 1;

		}
		_lasttPlatfrom.transform.GetComponent<Platfrom> ().SetValues ();

		}


	List<TypesOfWall> typesOfWall = new List<TypesOfWall>();

	public TypesOfWall GetTypeOfWall(int platfromNumber)
	{
		
		for (int i = 0; i < typesOfWall.Count; i++) {
			if (platfromNumber == i) {
				int r= GetSelectAndGenerateRandomPattern();
				previousPattern = r;
				//Debug.LogError ("ab" + r +i);
				return typesOfWall[r];
			}
		}
		return TypesOfWall.Wall0;
	}


	public void FiveBlockWave()
	{
		callFiveWave = true;
		callFourWave = false;
		StartCoroutine ("ZoomOutCamera");
		 
			dragSestivity = increaseDragValue;

		if (TankController._instance) {
			TankController._instance.SetPosTank ();
			TankController._instance.SetCurrentTankAbility ();
		}

		randomPattern.Clear ();
		for (int i = 0; i < 5; i++) {
			randomPattern.Add (i);
		}
	}

	IEnumerator ZoomOutCamera()
	{

		while (Camera.main.orthographicSize < 6.21f) {
			Camera.main.orthographicSize += Time.deltaTime * 1;
			yield return new WaitForEndOfFrame ();
		}
		GameObject[] part = GameObject.FindGameObjectsWithTag ("R");
		for (int j = 0; j < part.Length; j++) {
			part [j].transform.GetComponent<ResolutionFixer> ().SetPosition ();
		}
	}
	IEnumerator ZoomINCamera()
	{

		while (Camera.main.orthographicSize > 5.0f) {
			Camera.main.orthographicSize -= Time.deltaTime * 1;
			yield return new WaitForEndOfFrame ();
		}
		GameObject[] part = GameObject.FindGameObjectsWithTag ("R");
		for (int j = 0; j < part.Length; j++) {
			part [j].transform.GetComponent<ResolutionFixer> ().SetPosition ();
		}
	}
	public void FourWaveBlock()
	{
		callFiveWave = false;
		callFourWave = true;
		StartCoroutine (ZoomINCamera ());
		StopCoroutine ("ZoomOutCamera");
	
		randomPattern.Clear ();
		dragSestivity = startDragValue;
		if (TankController._instance) {
			TankController._instance.SetPosTank ();
			TankController._instance.SetCurrentTankAbility ();
		}
		for (int i = 0; i < 4; i++) {
			randomPattern.Add (i);
		}

	}


	int skipThisLevel ;

	public void SetTypesOfWall()
	{
		typesOfWall.Clear ();

		for (int i = 0; i < levelSystemData.numberOfLevels.Length; i++) {
			if (PlatfromController.waveScore >= levelSystemData.numberOfLevels [i].MinScore && PlatfromController.waveScore <= levelSystemData.numberOfLevels [i].MaxScore)
			{
				
				currentTypeOfLevel = levelSystemData.numberOfLevels [i].typeOfLevels;

				skipThisLevel = levelSystemData.numberOfLevels [i].skipThislevelNow;
				if (skipThisLevel > PlayerPrefs.GetInt ("SkipThisLevel")) {
					PlayerPrefs.SetInt ("SkipThisLevel", skipThisLevel);
				}

				for(int j = 0 ; j < levelSystemData.numberOfLevels [i].numberOfWalls.Length ; j++)
				{
					if(VoodooSauce.GetPlayerCohort()== StringConstant.ABNewlevelEasy && PlayerPrefs.GetInt ("easylevel") > 0 ) {
						float value = 0;
						float Finalvalue = 0;
						for(int e=0 ; e < PlayerPrefs.GetInt ("easylevel") ; e++)
						{
							 Finalvalue = UIManager._instance.ballData.blockRate - value;

							value += (Finalvalue / UIManager._instance.ballData.eveytimeReducePercentage);
							Debug.Log (" value" + value);

						}
						 Finalvalue = UIManager._instance.ballData.blockRate - value;

						Debug.Log ("Final value" + Finalvalue);

						rate = ((startBallPowerBackUp * startBallRateBackUp) * Finalvalue)
                            - ((startBallPowerBackUp * startBallRateBackUp) * UIManager._instance.ballData.reduceBlockRateBy);
					} else {

                        if (!levelEnded)
                        {
                            rate = ((startBallPowerBackUp * startBallRateBackUp) * UIManager._instance.ballData.blockRate)
                                - ((startBallPowerBackUp * startBallRateBackUp) * UIManager._instance.ballData.reduceBlockRateBy);
                        }else
                        {
                            rate = ((startBallPowerBackUp * startBallRateBackUp) * UIManager._instance.ballData.blockRate+(UIManager._instance.ballData.blockRate/2))
                                - ((startBallPowerBackUp * startBallRateBackUp) * UIManager._instance.ballData.reduceBlockRateBy);
                        }
					}

					typesOfWall.Add (levelSystemData.numberOfLevels [i].numberOfWalls [j]);
					//Debug.LogError ("rate:" + typesOfWall.Count);

					PlatfromController._instance.blockMoveSpeed = levelSystemData.numberOfLevels [i].blockMoveSpeed; 
				}
			}
		}
		if (typesOfWall.Count > 4) {
			FiveBlockWave ();
		} else {
			FourWaveBlock ();
		}
	}
	public void SetSpwanItemPriority(GameObject _lasttPlatfrom)
	{
		
	_lasttPlatfrom.transform.GetComponent<Platfrom> ().priorityNumber = 1;
	_lasttPlatfrom.transform.GetComponent<Platfrom> ().SetValues ();
		 
	}
	public void ReloadGame()
	{
		Time.timeScale = timescale;
	}



	GameObject patternBlock ;
    public void CreteHardStaticPattern()
    {
        int patternNumber = Random.Range(0,levelPatternsData.hardStaticPatterns.Length);
        float yIncrese = 0;
        int blocknumber = 0;
        int numberofrow = 3;
        for (int i = 0; i < 5; i++)
        {
            numberofrow = levelPatternsData.hardStaticPatterns[patternNumber].column[i].row.Length;

            for (int j = 0; j < numberofrow; j++)
            {
                patternBlock = EasyObjectPool.instance.GetObjectFromPool("Obstacle", new Vector3(obstaclePosRight5[i].position.x, obstaclePosRight5[i].position.y + yIncrese, -5f), Quaternion.identity);

                //patternBlock = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5 [j].position.x, obstaclePosRight5 [j].position.y+yIncrese , -5f), Quaternion.identity) as GameObject;
                patternBlock.transform.SetParent(spriteParent);
                patternBlock.GetComponent<Platfrom>().typeOfPlatfrom = Platfromtype.RightBlock;
                patternBlock.GetComponent<Platfrom>().blockCounted = true;
                patternBlock.GetComponent<BoxCollider>().enabled = true;
                patternBlock.GetComponent<Platfrom>().onceSeted = false;
                patternBlock.GetComponent<Platfrom>().bonusBlock = false;
                patternBlock.GetComponent<Platfrom>().showStar = false;
                patternBlock.GetComponent<Platfrom>().bossblock = false;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = false;

                patternBlock.GetComponent<Platfrom>().lineNumber = lineNumber;

                patternBlock.GetComponent<Platfrom>().blockMoveType = BlockMoveType.None;
                patternBlock.GetComponent<Platfrom>().distanceToMove = 0f;
                patternBlock.GetComponent<Platfrom>().sideMovespeed = 0f;
                int value = levelPatternsData.hardStaticPatterns[patternNumber].column[i].row[j];
                if (value == 0)
                {
                    Destroy(patternBlock);
                }
                else if (value == 2)
                {
                    if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewPatternSystem)
                    {
                        patternBlock.GetComponent<Platfrom>().nonDestroyBlock = true;
                    }
                    else
                    {
                        SetPriority(patternBlock, i * j, blocknumber);
                        blocknumber++;

                        if (blocknumber > 4)
                        {
                            blocknumber = 0;
                        }
                    }
                    Debug.Log("aaaaaa");
                }
                else
                {


                    SetPriority(patternBlock, i * j, blocknumber);
                    blocknumber++;

                    if (blocknumber > 4)
                    {
                        blocknumber = 0;
                    }
                }



                patternBlock.transform.localPosition = new Vector3(obstaclePosRight5[i].position.x, obstaclePosRight5[i].position.y + yIncrese, -5f);
                //lineNumber++;
                patternBlock.transform.GetComponent<Platfrom>().SetValues();
                patternBlock.transform.localScale = Vector3.one;
                yIncrese += 1.18f;

            }
            yIncrese = 0;

        }
    }
	public void CretePattern()
	{
		int patternNumber = GetSelectAndGenerateRandomLevelPattern();
		previousLevelPattern = patternNumber;
		float yIncrese = 0;
		int blocknumber = 0;
        int numberofrow = 3;
		for (int i = 0; i < 5; i++) {
            numberofrow = levelPatternsData.numberofPatterns[patternNumber].column[i].row.Length;

            for (int j = 0; j < numberofrow; j++) {
				patternBlock = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y+yIncrese , -5f), Quaternion.identity);

				//patternBlock = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5 [j].position.x, obstaclePosRight5 [j].position.y+yIncrese , -5f), Quaternion.identity) as GameObject;
				patternBlock.transform.SetParent (spriteParent);
				patternBlock.GetComponent<Platfrom> ().typeOfPlatfrom = Platfromtype.RightBlock;
				patternBlock.GetComponent<Platfrom> ().blockCounted = true;
				patternBlock.GetComponent<BoxCollider> ().enabled = true;
				patternBlock.GetComponent<Platfrom> ().onceSeted = false;
				patternBlock.GetComponent<Platfrom> ().bonusBlock = false;
				patternBlock.GetComponent<Platfrom> ().showStar = false;
				patternBlock.GetComponent<Platfrom> ().bossblock = false;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = false;

                patternBlock.GetComponent<Platfrom> ().lineNumber = lineNumber; 

				patternBlock.GetComponent<Platfrom> ().blockMoveType = BlockMoveType.None;
				patternBlock.GetComponent<Platfrom> ().distanceToMove = 0f;
				patternBlock.GetComponent<Platfrom> ().sideMovespeed = 0f;
				int value = levelPatternsData.numberofPatterns [patternNumber].column [i].row [j];
				if (value == 0) {
					Destroy (patternBlock);
                }else if(value == 2) 
                {
                    if (VoodooSauce.GetPlayerCohort() == StringConstant.ABNewPatternSystem)
                    {
                        patternBlock.GetComponent<Platfrom>().nonDestroyBlock = true;
                    }else{
                        SetPriority(patternBlock, i * j, blocknumber);
                        blocknumber++;

                        if (blocknumber > 4)
                        {
                            blocknumber = 0;
                        }
                    }
                    Debug.Log("aaaaaa");
                }
                else  {


					SetPriority (patternBlock,i*j,blocknumber);
					blocknumber++;

					if (blocknumber > 4) {
						blocknumber = 0;
					}
				}



				patternBlock.transform.localPosition = new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y + yIncrese, -5f);
				//lineNumber++;
				patternBlock.transform.GetComponent<Platfrom> ().SetValues ();
				patternBlock.transform.localScale = Vector3.one;
                yIncrese += 1.18f;

			}
            yIncrese = 0;

		}
	}

	[HideInInspector]
	public List<int> numberOfBonusBlocks = new List<int>();

	public void CreteBonusPattern()
	{
		numberOfBonusBlocks.Clear ();
		float row = 5;
		float column = 2;

		int priorityNumber = 1;
		long bonuslevelNumber = UIManager._instance.GetPlayerPrefsValue ("BonusLevelNumber");
		Debug.LogError ("value" + UIManager._instance.GetPlayerPrefsValue ("BonusLevelNumber"));

		for (int i = 0; i < UIManager._instance.ballData.numberOfBonusLevels.Length; i++) {
			if (bonuslevelNumber >= UIManager._instance.ballData.numberOfBonusLevels[i].levelNumberMin && bonuslevelNumber <= UIManager._instance.ballData.numberOfBonusLevels[i].levelNumberMax)
			{
				column = UIManager._instance.ballData.numberOfBonusLevels [i].numberOfRows;
			}
		}

		for (int j = 0; j < bonuslevelNumber; j++)
		{
			for (int i = 0; i < UIManager._instance.ballData.numberOfBonusLevels.Length; i++)
			{
				if (j >= UIManager._instance.ballData.numberOfBonusLevels[i].levelNumberMin &&
					j <= UIManager._instance.ballData.numberOfBonusLevels[i].levelNumberMax) 
				{
					
					priorityNumber += UIManager._instance.ballData.numberOfBonusLevels [i].pointIncreaseForEachLevel;

					Debug.LogError ("value" + UIManager._instance.ballData.numberOfBonusLevels [i].pointIncreaseForEachLevel);
				}
			}
		}
		float yIncrese = 0;
		float blocknumber = 0;

	for (int i = 0; i < column; i++) {
		 for (int j = 0; j < row; j++) {
				patternBlock = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight5Bonus [i].position.x, obstaclePosRight5Bonus [i].position.y+yIncrese, -5f), Quaternion.identity);

			//	patternBlock = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5Bonus [j].position.x, obstaclePosRight5Bonus [j].position.y+yIncrese , -5f), Quaternion.identity) as GameObject;
				patternBlock.transform.SetParent (spriteParent);
				patternBlock.GetComponent<Platfrom> ().typeOfPlatfrom = Platfromtype.RightBlock;
				patternBlock.GetComponent<Platfrom> ().showStar = false;
				patternBlock.GetComponent<Platfrom> ().bossblock = false;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = false;

				patternBlock.GetComponent<Platfrom> ().blockCounted = true;
				patternBlock.GetComponent<Platfrom> ().bonusBlock = true;
				patternBlock.GetComponent<Platfrom> ().onceSeted = true;
				patternBlock.GetComponent<BoxCollider> ().enabled = true;
				patternBlock.GetComponent<Platfrom> ().blockMoveType = BlockMoveType.None;
				patternBlock.GetComponent<Platfrom> ().distanceToMove = 0f;
				patternBlock.GetComponent<Platfrom> ().sideMovespeed = 0f;
				SpriteRenderer objectColor = patternBlock.transform.GetComponent<Platfrom> ().mainObject;
				float colorValue = (blocknumber)/(column * row);
				patternBlock.transform.GetComponent<Platfrom> ().mainObject.color = Color.Lerp (PlatfromController._instance.levelSystemData.wall0.wallColor, PlatfromController._instance.levelSystemData.wallSmall.wallColor,colorValue/1.1f);
			
				numberOfBonusBlocks.Add ((int)blocknumber);
				patternBlock.transform.GetComponent<Platfrom> ().priorityNumber = priorityNumber + blocknumber;

				blocknumber++;
				lineNumber++;

				patternBlock.transform.GetComponent<Platfrom> ().SetValues ();
				patternBlock.transform.localScale = new Vector3 (0.82f, 0.85f, 0.82f);
				patternBlock.transform.localPosition = new Vector3 (obstaclePosRight5Bonus [j].position.x, obstaclePosRight5Bonus [j].position.y+yIncrese , -2f);
	      }
			yIncrese += 1f;
		}
	}


    public void CreteHardMovingPattern()
    {
        int patternNumber = Random.Range(0, levelPatternsData.hardMovingPatterns.Length);
        Debug.Log(patternNumber + "thi");

        float yIncrese = 0;
        int blocknumber = 0;
        int numberofrow = 3;
        for (int i = 0; i < 5; i++)
        {
            numberofrow = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row.Length;

            for (int j = 0; j < numberofrow; j++)

            {
                patternBlock = EasyObjectPool.instance.GetObjectFromPool("Obstacle", new Vector3(obstaclePosRight5[i].position.x, obstaclePosRight5[i].position.y + yIncrese, -5f), Quaternion.identity);

                //patternBlock = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5 [j].position.x, obstaclePosRight5 [j].position.y+yIncrese , -5f), Quaternion.identity) as GameObject;
                patternBlock.transform.SetParent(spriteParent);
                patternBlock.GetComponent<Platfrom>().typeOfPlatfrom = Platfromtype.RightBlock;
                patternBlock.GetComponent<Platfrom>().showStar = false;
                patternBlock.GetComponent<Platfrom>().bossblock = false;

                patternBlock.GetComponent<Platfrom>().blockCounted = true;
                patternBlock.GetComponent<BoxCollider>().enabled = true;
                patternBlock.GetComponent<Platfrom>().onceSeted = false;
                patternBlock.GetComponent<Platfrom>().bonusBlock = false;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = false;

                patternBlock.transform.localPosition = new Vector3(obstaclePosRight5[i].position.x, obstaclePosRight5[i].position.y + yIncrese, -5f);
                patternBlock.transform.localScale = Vector3.one;
                patternBlock.GetComponent<Platfrom>().lineNumber = lineNumber;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row[j].nonDestryBlock;

                bool value = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row[j].showBlock;
                if (!value)
                {
                    Destroy(patternBlock);
                }
                else
                {
                    patternBlock.GetComponent<Platfrom>().blockMoveType = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row[j].blockMoveType;
                    patternBlock.GetComponent<Platfrom>().distanceToMove = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row[j].distanceToMove;
                    patternBlock.GetComponent<Platfrom>().sideMovespeed = levelPatternsData.hardMovingPatterns[patternNumber].column[i].row[j].moveSpeed;

                    SetPriority(patternBlock, i * j, blocknumber);
                    blocknumber++;

                    if (blocknumber > 4)
                    {
                        blocknumber = 0;
                    }
                }



                //lineNumber++;
                patternBlock.transform.GetComponent<Platfrom>().SetValues();
                yIncrese += 1.18f;

            }
            yIncrese = 0;
        }
    }
	public void CreteMovingPattern()
	{
		int patternNumber = GetSelectAndGenerateRandomMovingPattern();
		Debug.Log (patternNumber + "thi");
		previousMovingPattern = patternNumber;
		float yIncrese = 0;
		int blocknumber = 0;
        int numberofrow = 3;
        for (int i = 0; i < 5; i++)
        {
            numberofrow = levelPatternsData.numberofMovingLevels[patternNumber].column[i].row.Length ;

            for (int j = 0; j < numberofrow; j++)

			 {
				patternBlock = 	EasyObjectPool.instance.GetObjectFromPool ("Obstacle", new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y+yIncrese, -5f), Quaternion.identity);

				//patternBlock = Instantiate (obstacleRight, new Vector3 (obstaclePosRight5 [j].position.x, obstaclePosRight5 [j].position.y+yIncrese , -5f), Quaternion.identity) as GameObject;
				patternBlock.transform.SetParent (spriteParent);
				patternBlock.GetComponent<Platfrom> ().typeOfPlatfrom = Platfromtype.RightBlock;
				patternBlock.GetComponent<Platfrom> ().showStar = false;
				patternBlock.GetComponent<Platfrom> ().bossblock = false;

				patternBlock.GetComponent<Platfrom> ().blockCounted = true;
				patternBlock.GetComponent<BoxCollider> ().enabled = true;
				patternBlock.GetComponent<Platfrom> ().onceSeted = false;
				patternBlock.GetComponent<Platfrom> ().bonusBlock = false;
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = false;

				patternBlock.transform.localPosition = new Vector3 (obstaclePosRight5 [i].position.x, obstaclePosRight5 [i].position.y + yIncrese, -5f);
				patternBlock.transform.localScale = Vector3.one;
                patternBlock.GetComponent<Platfrom> ().lineNumber = lineNumber; 
                patternBlock.GetComponent<Platfrom>().nonDestroyBlock = levelPatternsData.numberofMovingLevels[patternNumber].column[i].row[j].nonDestryBlock;

				bool value = levelPatternsData.numberofMovingLevels [patternNumber].column [i].row [j].showBlock;
				if (!value) {
					Destroy (patternBlock);
				} else  {
					patternBlock.GetComponent<Platfrom> ().blockMoveType = levelPatternsData.numberofMovingLevels [patternNumber].column [i].row [j].blockMoveType;
					patternBlock.GetComponent<Platfrom> ().distanceToMove = levelPatternsData.numberofMovingLevels [patternNumber].column [i].row [j].distanceToMove;
					patternBlock.GetComponent<Platfrom> ().sideMovespeed = levelPatternsData.numberofMovingLevels [patternNumber].column [i].row [j].moveSpeed;

					SetPriority (patternBlock,i*j,blocknumber);
					blocknumber++;

					if (blocknumber > 4) {
						blocknumber = 0;
					}
				}

			

				//lineNumber++;
				patternBlock.transform.GetComponent<Platfrom> ().SetValues ();
                yIncrese += 1.18f;

			}
            yIncrese = 0;
		}
	}

}
