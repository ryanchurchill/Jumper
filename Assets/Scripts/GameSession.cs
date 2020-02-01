using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // config
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] float scorePerSecond = 1.0f;
    [SerializeField] int updateDisplayAfterMs = 1000;
    [SerializeField] Player player;
    [SerializeField] GameObject deathPanel;

    // state
    bool isPlayerAlive = true;
    float score = 0;
    DateTime timeOfLastScoreDisplayUpdate = DateTime.Now;

    // Start is called before the first frame update
    void Start()
    {
        deathPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerAlive)
        {
            IncreaseScore(scorePerSecond * Time.deltaTime);
        }
    }

    void IncreaseScore(float pointsToAdd)
    {
        UpdateScore(score + pointsToAdd);
    }

    void UpdateScore(float newScore, bool forceDisplayUpdate = false)
    {
        score = newScore;

        if (forceDisplayUpdate || DateTime.Now.Subtract(timeOfLastScoreDisplayUpdate).TotalMilliseconds > updateDisplayAfterMs)
        {
            timeOfLastScoreDisplayUpdate = DateTime.Now;
            scoreText.text = String.Format("{0:#,###0}", score);
        }
        
    }

    public void PlayerDied()
    {
        isPlayerAlive = false;
        deathPanel.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
