using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject Collectibles;
    public GameObject Time;
    // Start is called before the first frame update
    void Start()
    {
        float bestTime = float.MaxValue;
        int bestCollectibles = 0;
        switch (GameManager.Level)
        {
            case Level.Level1:
                if (PlayerPrefs.HasKey("Level1Time"))
                    bestTime = PlayerPrefs.GetFloat("Level1Time");
                if (PlayerPrefs.HasKey("Level1Col"))
                    bestCollectibles = PlayerPrefs.GetInt("Level1Col");
                break;
            case Level.Level2:
                if (PlayerPrefs.HasKey("Level2Time"))
                    bestTime = PlayerPrefs.GetFloat("Level2Time");
                if (PlayerPrefs.HasKey("Level2Col"))
                    bestCollectibles = PlayerPrefs.GetInt("Level2Col");
                break;
            case Level.Level3:
                if (PlayerPrefs.HasKey("Level3Time"))
                    bestTime = PlayerPrefs.GetFloat("Level3Time");
                if (PlayerPrefs.HasKey("Level3Col"))
                    bestCollectibles = PlayerPrefs.GetInt("Level3Col");
                break;
        }
        float time = (float)decimal.Round((decimal)GameManager.LevelTime, 2);
        if (time < bestTime)
        {
            switch (GameManager.Level)
            {
                case Level.Level1:
                    PlayerPrefs.SetFloat("Level1Time", time);
                    break;
                case Level.Level2:
                    PlayerPrefs.SetFloat("Level2Time", time);
                    break;
                case Level.Level3:
                    PlayerPrefs.SetFloat("Level3Time", time);
                    break;
            }
            PlayerPrefs.Save();
            Time.GetComponent<TextMeshProUGUI>().text = $"Time: {time} (New Best!)";
        }
        else
        {
            Time.GetComponent<TextMeshProUGUI>().text = $"Time: {time} (Best: {bestTime})";
        }

        if (bestCollectibles < GameManager.Collectibles)
        {
            switch (GameManager.Level)
            {
                case Level.Level1:
                    PlayerPrefs.SetInt("Level1Col", GameManager.Collectibles);
                    break;
                case Level.Level2:
                    PlayerPrefs.SetInt("Level2Col", GameManager.Collectibles);
                    break;
                case Level.Level3:
                    PlayerPrefs.SetInt("Level3Col", GameManager.Collectibles);
                    break;
            }
            PlayerPrefs.Save();
            Collectibles.GetComponent<TextMeshProUGUI>().text = $"Collectibles: {GameManager.Collectibles}/{GameManager.TotalCollectibles} (New Best!)";
        }
        else
        {
            Collectibles.GetComponent<TextMeshProUGUI>().text = $"Collectibles: {GameManager.Collectibles}/{GameManager.TotalCollectibles} (Best: {bestCollectibles}/{GameManager.TotalCollectibles})";
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
