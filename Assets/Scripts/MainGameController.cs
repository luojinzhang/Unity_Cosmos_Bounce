using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;

public class MainGameController : MonoBehaviour
{
    #region Variables

    [Header("Buttons")] public Button BtnPause;
    public Button BtnAudio;
    public Button BtnContinue;
    public Button BtnQuit;
    public Button BtnYes;
    public Button BtnNo;
    public Button BtnMute;

    [Header("Popups")] public GameObject PopupPaused;
    public GameObject Notice;
    public GameObject PopupEndgame;

    [Header("Texts")] public Text TxtGem;
    public Text TxtDistance;
    public Text TxtHighScore;

    [Header("Islands Properties")] public float IslandOffsetX;
    public float IslandDistance;

    [Header("Roots")] public RectTransform IslandsRoot;
    public RectTransform AsteroidRoot;

    [Header("Islands Respawn Point")] public System.Collections.Generic.List<Transform> ListRespawnPoint;
    public System.Collections.Generic.List<Transform> ListRespawnPointAsteroid;

    [Header("Prefabs")] public System.Collections.Generic.List<IslandController> ListIslandPrefab;
    public System.Collections.Generic.List<AsteroidController> ListAsteroidPrefab;
    public GemController GemPrefab;
    public LaserController LaserPrefab;

    [Header("Gameobjets")] public GameObject TutorialObject;
    
    public float TimeRespawn = 2f;

    public MinimapController MiniMap;
    public static float StaticIslandOffsetX { get; set; }
    public static float StaticIslandDistance { get; set; }
    public static bool TutorialDone { get; set; }
    public AudioSource AudioBG;
    private static AudioSource staticAudioBG;
    private static int _staticTotalGems;
    private static int _staticTotalDistance;
    private static GameObject _staticPopupEndgame;
    private Button _btnMute;

    private float _elapsedTime = 0f;
    private float _time = 0f;

    private GameObject _astronaut;
    private List<GameObject> _listIslands;
    private List<GameObject> _listAsteroids;
    private List<int> _poolEasy, _poolNormal, _poolHard;
    private MenuSceneController _audio;
    static bool audioplay = false;

    #endregion

    #region UnityFunctions

    private void Awake()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 1;
    }

    void Start()
    {
        BtnYes.onClick.AddListener(this.OnClickBtnYes);
        BtnNo.onClick.AddListener(this.OnClickBtnNo);
        BtnPause.onClick.AddListener(this.OnClickBtnPause);
        BtnAudio.onClick.AddListener(this.OnClickBtnAudio);
        BtnContinue.onClick.AddListener(this.OnClickBtnContinue);
        BtnQuit.onClick.AddListener(this.OnClickBtnQuit);
        BtnMute.onClick.AddListener(this.OnClickBtnMute);
        
        StaticIslandOffsetX = IslandOffsetX;
        StaticIslandDistance = IslandDistance;
        _staticTotalGems = 0;
        _staticTotalDistance = 0;
        _staticPopupEndgame = PopupEndgame;
        staticAudioBG = AudioBG;
        Time.timeScale = 1;
        
        FadeTutorialAway();
        SaveLoad.Load();

        AudioBG.Play();
        if (audioplay == true)
        {
            BtnMute.gameObject.SetActive(true);
            BtnAudio.interactable = false;
            AudioBG.Stop();
        }
        else if (audioplay == false)
        {
            BtnMute.gameObject.SetActive(false);
            BtnAudio.interactable = true;
            AudioBG.Play();
        }


        //        _staticListIslands = new List<IslandController>();
        _astronaut = GameObject.FindGameObjectWithTag("Astranaut");

        var arrayIslands = GameObject.FindGameObjectsWithTag("Islands");
        _listIslands = arrayIslands.ToList();
        var arrayAsteroids = GameObject.FindGameObjectsWithTag("Asteroids");
        _listAsteroids = arrayAsteroids.ToList();

        _poolEasy = new List<int>() {0, 4, 8};
        _poolNormal = new List<int>() {0, 3, 4, 5, 7, 8};
        _poolHard = new List<int>() {1, 2, 3, 5, 6, 7};

        BtnPause.onClick.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUi();
        UpdateMinimap();
        UpdateIslands();
        UpdateAsteroids();
    }

    #endregion

    #region MainGameFunctions

    private void UpdateMinimap()
    {
        if (this._astronaut)
        {
            MiniMap.UpdatePlayerIcon(_astronaut.transform.position);

            var listVector3 = new List<Vector3>();
            var islandDestroyed = _listIslands.FindIndex(island => island == null);
            MiniMap.RemoveIslandIconAt(islandDestroyed);

            _listIslands = _listIslands.Where(item => item != null).ToList();
            _listIslands.ForEach(island => { listVector3.Add(island.transform.position); });

            MiniMap.UpdateListIslandIcon(listVector3);
        }
    }

    private void UpdateIslands()
    {
        //        if (!MainGameController.TutorialDone)
        //        {
        //            return;
        //        }

        _elapsedTime += Time.deltaTime;
        TimeRespawn = 2f;
        if (_elapsedTime >= TimeRespawn)
        {
            var newPos = RandomPosition();
            var randIslandPrefabIndex = GenerateIslandIndex();
            var newIslandPrefab =
                Instantiate(ListIslandPrefab[randIslandPrefabIndex].gameObject, IslandsRoot.transform);
            newIslandPrefab.transform.position = newPos.position;

            InstantiateGem(newIslandPrefab);
            InstantiateLaser(newIslandPrefab);

            _elapsedTime = 0f;

            _listIslands.Add(newIslandPrefab);
            MiniMap.CreateNewIslandIcon();
        }
    }

    private int GenerateIslandIndex()
    {
        var pool = new List<int>();
        if (_staticTotalDistance <= 10)
        {
            pool = _poolEasy;
        }
        else if (_staticTotalDistance > 10 && _staticTotalDistance <= 50)
        {
            pool = _poolNormal;
        }
        else if (_staticTotalDistance > 50)
        {
            pool = _poolHard;
        }

        var index = Random.Range(0, pool.Count);
        return pool[index];
    }

    private void InstantiateLaser(GameObject newIslandPrefab)
    {
        var isLaserAvailable = Random.Range(0, 9);
        if (isLaserAvailable == 1)
        {
            var newLaser = Instantiate(LaserPrefab.gameObject, newIslandPrefab.transform);
            newLaser.transform.localPosition = new Vector3(3, 2, 0);
        }
    }

    private void InstantiateGem(GameObject newIslandPrefab)
    {
        var isGemAvailable = Random.Range(0, 2);
        if (isGemAvailable == 1)
        {
            var newGem = Instantiate(GemPrefab.gameObject, newIslandPrefab.transform);
            newGem.transform.localPosition = Vector3.up;
        }
    }

    private void UpdateAsteroids()
    {
        if (_staticTotalDistance < 10)
        {
            return;
        }

        _time += Time.deltaTime;
        TimeRespawn = 3f;
        if (_time >= TimeRespawn)
        {
            var newPos = RandomPositionAsteroid();
            var randAsteroidPrefabIndex = Random.Range(0, ListAsteroidPrefab.Count - 1);
            var newAsteroidPrefab =
                Instantiate(ListAsteroidPrefab[randAsteroidPrefabIndex].gameObject, AsteroidRoot.transform);
            newAsteroidPrefab.transform.position = newPos.position;


            _time = 0f;


            _listAsteroids.Add(newAsteroidPrefab);
            //MiniMap.CreateNewIslandIcon();
        }
    }

    private Transform RandomPosition()
    {
        var index = Random.Range(0, ListRespawnPoint.Count);
        var randTransform = ListRespawnPoint[index];
        return randTransform;
    }

    private Transform RandomPositionAsteroid()
    {
        var index = Random.Range(0, ListRespawnPointAsteroid.Count - 1);
        var randTransform = ListRespawnPointAsteroid[index];
        return randTransform;
    }


    private void OnClickBtnMute()
    {
        BtnMute.gameObject.SetActive(false);
        BtnAudio.interactable = true;
        //AudioBG.Play();

        audioplay = false;
    }

    private void OnClickBtnAudio()
    {
        BtnMute.gameObject.SetActive(true);
        BtnAudio.interactable = false;
        //AudioBG.Stop();
        audioplay = true;
    }

    private void OnClickBtnPause()
    {
        PopupPaused.SetActive(true);
        var highScore = _staticTotalDistance > SaveLoad.HighScore ? _staticTotalDistance : SaveLoad.HighScore;
        TxtHighScore.text = "HIGH SCORE: " + highScore;
        PauseGame();
    }

    private void OnClickBtnContinue()
    {
        PopupPaused.SetActive(false);
        ContinueGame();
    }

    private void OnClickBtnQuit()
    {
        Notice.SetActive(true);
    }

    private void OnClickBtnNo()
    {
        Notice.SetActive(false);
    }

    private void OnClickBtnYes()
    {
        SaveLoad.Save(_staticTotalDistance);
        SceneManager.LoadScene("MenuScene");
    }

    private void UpdateUi()
    {
        TxtGem.text = _staticTotalGems.ToString();
        TxtDistance.text = _staticTotalDistance.ToString();
    }


    private void FadeTutorialAway()
    {
        var imgHand = TutorialObject.GetComponentInChildren<Image>();
        var txtText = TutorialObject.GetComponentInChildren<Text>();
        imgHand.CrossFadeAlpha(0f, 5f, false);
        txtText.CrossFadeAlpha(0f, 5f, false);
    }

    #endregion

    #region StaticFunctions

    private static void PauseGame()
    {
        Time.timeScale = 0;
        staticAudioBG.Stop();
    }

    private static void ContinueGame()
    {
        Time.timeScale = 1;
        if (audioplay == false)
        {
            staticAudioBG.Play();
        }
    }

    public static void EndGame()
    {
        SaveLoad.HighScore = Math.Max(SaveLoad.HighScore, _staticTotalDistance);
        PauseGame();
        _staticPopupEndgame.SetActive(true);
    }

    public static void Retry()
    {
        SaveLoad.Save(_staticTotalDistance);
        SceneManager.LoadScene("MainScene");
    }

    public static void Quit()
    {
        SaveLoad.Save(_staticTotalDistance);
        SceneManager.LoadScene("MenuScene");
    }

    public static void UpdateTotalGems()
    {
        _staticTotalGems++;
    }

    public static void UpdateTotalIslandPassed()
    {
        _staticTotalDistance++;
    }

    #endregion
}