using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action<GameState> OnStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStateChanged?.Invoke(GameState.Play);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToScene(string scene)
    {
        switch (scene)
        {
            case "Main Menu":
                SceneManager.LoadScene(0);
                break;
            case "Level 1":
                SceneManager.LoadScene(1);
                break;
            case "Level 2":
                SceneManager.LoadScene(2);
                break;
            case "Level 3":
                SceneManager.LoadScene(3);
                break;
        }
    }

    public void OnFinish()
    {
        
    }

    public void OnFail()
    {
        Debug.Log("HerwadaAWDAD");
        OnStateChanged?.Invoke(GameState.Failed);
    }

    public void ResetLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

public enum GameState
{
    Play,
    Failed,
    Paused
}