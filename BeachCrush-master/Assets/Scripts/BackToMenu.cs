using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{

    private GameData gameData;
    private BoardManager board;

    public void WinButton()
    {
        if(gameData != null)
        {
            gameData.saveData.isActive[board.level + 1] = true;
            gameData.Save();
        }

        SceneManager.LoadScene("MenuScene");
    }

    public void LoseButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<BoardManager>();
    }
}
