using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
}

public class GUIManager : MonoBehaviour
{
    public Text movesText, scoreText;
    public GameObject winPanel, losePanel;
    private int moveCounter;
    private int score;
    private float timerSeconds;

    public int currentCounterValue;

    public EndGameRequirements requirements;
    private GoalManager goalManager;

    public SceneChanger sceneChanger;

    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
        }
    }

    public int MoveCounter
    {
        get { return moveCounter; }
        set
        {
            moveCounter = value;
            if (requirements.gameType == GameType.Moves)
            {
                movesText.text = "Moves: " + moveCounter;
            }
            else
            {
                movesText.text = "Time: " + currentCounterValue;
            }

            if (moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }

    public static GUIManager sharedInstance;

    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetGameType();
        SetupGame();

        goalManager = FindObjectOfType<GoalManager>();
        score = 0;
        moveCounter = requirements.counterValue;
        scoreText.text = "Score: " + score;

        if (requirements.gameType == GameType.Moves)
        {
            movesText.text = "Moves: " + requirements.counterValue;
        }
        else
        {
            movesText.text = "Time: " + requirements.counterValue;
        }
    }

    void Update()
    {
        if (requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0)
            {
                DecreaseTime();
                timerSeconds = 1;
            }
        }
    }

    void SetGameType()
    {
        if (BoardManager.sharedInstance.world != null)
        {
            if (BoardManager.sharedInstance.level < BoardManager.sharedInstance.world.levels.Length)
            {
                if (BoardManager.sharedInstance.level < BoardManager.sharedInstance.world.levels.Length)
                {
                    if (BoardManager.sharedInstance.world.levels[BoardManager.sharedInstance.level] != null)
                    {
                        requirements = BoardManager.sharedInstance.world.levels[BoardManager.sharedInstance.level].endGameRequirements;
                    }
                }
            }
        }
    }

    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;
        if (requirements.gameType == GameType.Time)
        {
            timerSeconds = 1;
        }
    }

    public void DecreaseTime()
    {
        if (BoardManager.sharedInstance.currentGameState != GameState.pause)
        {
            currentCounterValue--;
            movesText.text = "Time: " + currentCounterValue;
            if (currentCounterValue <= 0)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(0.50f);
        goalManager.SetCandiesLeft();
        losePanel.SetActive(true);
        BoardManager.sharedInstance.currentGameState = GameState.lose;
        currentCounterValue = 0;
    }

    public IEnumerator WinGame()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(0.50f);
        winPanel.SetActive(true);
        BoardManager.sharedInstance.currentGameState = GameState.win;
        currentCounterValue = 0;
    }
}
