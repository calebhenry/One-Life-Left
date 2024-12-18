using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action<GameState> OnStateChanged;
    public static event Action<Progress> OnProgress;

    public int EnemiesLeft = 0;
    public int TotalEnemies = 0;
    public float PlayerHealth = 5;
    public int Energy = 0;
    public static int Collectibles = 0;
    public static int TotalCollectibles = 0;
    public static float LevelTime = 0;

    public static Level Level = Level.Level1;
    private bool Completed = false;

    private AudioSource audioSource;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStateChanged?.Invoke(GameState.Play);
        audioSource = GameObject.Find("BackgroundMusic")?.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {  
        
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Level") && scene.name != "Level End")
        {
            TotalCollectibles = FindObjectsOfType<Collectible>().Count();
            TotalCollectibles += FindObjectsOfType<Breakable>().Count(obj => obj.ContainsCollectible);
            TotalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Count();
            EnemiesLeft = TotalEnemies;
            Collectibles = 0;
        }
    }

    public void GoToScene(string scene)
    {
        Completed = false;
        switch (scene)
        {
            case "Main Menu":
                SceneManager.LoadScene(0);
                break;
            case "Level 1":
                Level = Level.Level1;
                SceneManager.LoadScene(1);
                break;
            case "Level 2":
                Level = Level.Level2;
                SceneManager.LoadScene(2);
                break;
            case "Level 3":
                Level = Level.Level3;
                SceneManager.LoadScene(3);
                break;
            case "Level End":
                SceneManager.LoadScene(4);
                break;
            case "End Menu":
                SceneManager.LoadScene(5);
                break;
        }
    }

    public void OnFail()
    {
        OnStateChanged?.Invoke(GameState.Failed);
        audioSource?.Pause();
    }

    public void ResetLevel()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "Level End")
        {
            switch (Level)
            {
                case Level.Level1:
                    GoToScene("Level 1");
                    break;
                case Level.Level2:
                    GoToScene("Level 2");
                    break;
                case Level.Level3:
                    GoToScene("Level 3");
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene(scene.buildIndex);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EnemyDestroyed()
    {
        StartCoroutine("SetCheckEnemies");
    }

    public void UpdateEnergy(int energy)
    {
        Energy++;
    }

    public void UpdatePlayerHealth(int health)
    {
        PlayerHealth = health;
    }

    public void OnComplete()
    {
        OnProgress?.Invoke(Progress.Complete);
        Completed = true;
        
    }

    public void OnExit()
    {
        GoToScene("Level End");
    }

    public void NextLevel()
    {
        switch (Level)
        {
            case Level.Level1:
                GoToScene("Level 2");
                break;
            case Level.Level2:
                GoToScene("Level 3");
                break;
            case Level.Level3:
                GoToScene("End Menu");
                break;
        }
    }

    private IEnumerator SetCheckEnemies()
    {
        yield return new WaitForSeconds(0.05f);
        EnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Count();
        if (EnemiesLeft == 0 && !Completed)
        {
            OnProgress?.Invoke(Progress.BossRemaining);
        }
    }
}

public enum GameState
{
    Play,
    Failed,
    Paused
}

public enum Progress
{
    EnemiesRemaining,
    BossRemaining,
    Complete
}

public enum Level
{
    Level1,
    Level2,
    Level3
}