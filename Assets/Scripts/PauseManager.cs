using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private BoardManager board;
    private SoundManager sound;

    public Image soundButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = musicOffSprite;
            }
            else
            {
                soundButton.sprite = musicOnSprite;
            }
        }
        else
        {
            soundButton.sprite = musicOnSprite;
        }

        pausePanel.SetActive(false);
        board = GameObject.FindWithTag("BoardManager").GetComponent<BoardManager>();
        sound = FindObjectOfType<SoundManager>();
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

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = musicOnSprite;
                PlayerPrefs.SetInt("Sound", 1);
                sound.adjustVolume();
            }
            else
            {
                soundButton.sprite = musicOffSprite;
                PlayerPrefs.SetInt("Sound", 0);
                sound.adjustVolume();
            }
        }
        else
        {
            soundButton.sprite = musicOffSprite;
            PlayerPrefs.SetInt("Sound", 1);
            sound.adjustVolume();
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void ExitGame()
    {
        //SceneManager.LoadScene("Splash");
    }
}
