using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{

    public static StartManager startManager;
    public GameObject level;
    private RectTransform startPanel, levelPanel;

    // Start is called before the first frame update

    private void Awake()
    {
        level.gameObject.SetActive(true);
        startPanel = GameObject.FindWithTag("HomeMenu").GetComponent<RectTransform>();
        levelPanel = GameObject.FindWithTag("LevelSelect").GetComponent<RectTransform>();
    }
    void Start()
    {
        startPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(false);
    }

    public void PlayGame()
    {
        startPanel.gameObject.SetActive(false);
        levelPanel.gameObject.SetActive(true);
    }

    public void Home()
    {
        startPanel.gameObject.SetActive(true);
        levelPanel.gameObject.SetActive(false);
    }
}
