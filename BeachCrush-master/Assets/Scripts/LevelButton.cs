using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public bool isActive;
    public Sprite activeSprite, lockedSprite;
    private int starsActive;

    private Image buttonImage;
    private Button button;

    public Image[] stars;
    public Text levelText;
    public int level;
    public GameObject confirmPanel;

    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        gameData = FindObjectOfType<GameData>();

        ShowLevel();
        LoadData();
        ActivateStars();
        DecideSprite();
    }

    void LoadData()
    {
        if(gameData != null)
        {
            if(gameData.saveData.isActive[level - 1])
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }

            starsActive = gameData.saveData.stars[level - 1];
        }
    }

    void ActivateStars()
    {
        for(int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    void DecideSprite()
    {
        if (isActive)
        {
            buttonImage.sprite = activeSprite;
            button.enabled = true;
            levelText.enabled = true;
        }
        else
        {
            buttonImage.sprite = lockedSprite;
            button.enabled = false;
            levelText.enabled = false;
        }
    }

    void ShowLevel()
    {
        levelText.text = "" + level;
    }

    public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }
}
