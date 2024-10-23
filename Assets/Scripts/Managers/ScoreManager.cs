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
        float time = (float)decimal.Round((decimal)GameManager.LevelTime, 2);
        Time.GetComponent<TextMeshProUGUI>().text = $"Time: {time}";
        Collectibles.GetComponent<TextMeshProUGUI>().text = $"Collectibles: {GameManager.Collectibles}/{GameManager.TotalCollectibles}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
