using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private BoardManager board;

    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("BoardManager").GetComponent<BoardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(paused && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            board.currentGameState = GameState.pause;
        }
        if(!paused && pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            board.currentGameState = GameState.inGame;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }
}
