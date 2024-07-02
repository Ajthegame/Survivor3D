using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image image;
    [SerializeField] TMP_Text popUpMsg;
    public GameObject startGamePanel;
    public GameObject gameOverPanel;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;
    UnityAction yesAction;
    UnityAction noAction;
    public static UIManager uiManager;

    [SerializeField] TMP_Text coinsCollected;
    [SerializeField] TMP_Text enemyKilled;

    // Start is called before the first frame update
    void Start()
    { 
        if (uiManager == null)
        {
            uiManager = this;
        }
        Time.timeScale = 0;

        DisplayPopUp(startGamePanel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        DisplayPopUp();
        Time.timeScale = 1.0f;
    }

    public void UpdatePlayerStats(int coins = -1,int enemiesKilled = -1)
    {
        if(coins > 0)
            coinsCollected.text = "Coins - " + coins.ToString();

        if(enemiesKilled>0)
            enemyKilled.text = "Kills - " + enemiesKilled.ToString();
    }

    public void DisplayPopUp(GameObject popUpToDisplay = null)
    {
        startGamePanel.SetActive(popUpToDisplay?popUpToDisplay.Equals(startGamePanel):false);
        gameOverPanel.SetActive(popUpToDisplay ? popUpToDisplay.Equals(gameOverPanel) : false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
