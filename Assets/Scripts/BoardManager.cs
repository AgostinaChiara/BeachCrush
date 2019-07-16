using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    pause,
    win,
    lose
}

public class BoardManager : MonoBehaviour
{
    public GameState currentGameState = GameState.inGame;

    public World world;
    public int level;

    public static BoardManager sharedInstance;
    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCandy;
    public int xSize, ySize;
    private SoundManager soundManager;
    private GoalManager goalManager;
    private GameData gameData;
    public int[] scoreGoals;
    private int numberStars;

    public GameObject[,] candies;

    public bool isShifting { get; set; }

    private Candy selectedCandy;

    public const int MinCandiesToMatch = 2;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }

        if(world != null)
        {
            if(level < world.levels.Length)
            {
                if (world.levels[level] != null )
                {
                    xSize = world.levels[level].width;
                    ySize = world.levels[level].heigth;
                    scoreGoals = world.levels[level].scoreGoals;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance == null) {
            sharedInstance = this;
        } else {
            Destroy(gameObject);
        }

        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
        currentGameState = GameState.pause;

        soundManager = FindObjectOfType<SoundManager>();
        goalManager = FindObjectOfType<GoalManager>();
        gameData = FindObjectOfType<GameData>();

        if (gameData != null)
        {
            gameData.Load();
        }
    }

    private void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[xSize, ySize];
        
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        int idx = -1;

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                GameObject newCandy = Instantiate(
                    currentCandy, 
                        new Vector3(startX + (offset.x * x), 
                                    startY + (offset.y * y), 
                                    0), 
                        currentCandy.transform.rotation
                    );
                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);
                
                do {
                    idx = Random.Range(0, prefabs.Count);
                }while((x > 0 && idx == candies[x - 1, y].GetComponent<Candy>().id) || 
                        (y > 0 && idx == candies[x, y - 1].GetComponent<Candy>().id));
                    
                
                Sprite sprite = prefabs[idx];
                newCandy.GetComponent<SpriteRenderer>().sprite = sprite;
                newCandy.GetComponent<Candy>().id = idx;

                newCandy.transform.parent = this.transform;
                candies[x, y] = newCandy;
            }                                          
        }
    }

    public IEnumerator FindNullCandies()
    {
        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                if(candies [x, y].GetComponent<SpriteRenderer>().sprite == null) {
                    yield return StartCoroutine(MakeCandiesFall(x, y));
                    break;
                }
            }
        }

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                candies[x, y].GetComponent<Candy>().FindAllMatches();
            }
        }

        if (currentGameState != GameState.pause)
        {
            currentGameState = GameState.inGame;
        }
    }

    private IEnumerator MakeCandiesFall(int x, int yStart, float shiftDelay = 0.05f)
    {
        isShifting = true;

        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        int nullCandies = 0;

        for(int y = yStart; y < ySize; y++) {
            SpriteRenderer spriteRenderer = candies[x, y].GetComponent<SpriteRenderer>();
            if(spriteRenderer.sprite == null) {
                nullCandies++;
            }
            renderers.Add(spriteRenderer);
        }

        if(soundManager != null)
        {
            soundManager.PlayDestroyNoise();
        }

        for (int i = 0; i < nullCandies; i++) {
            GUIManager.sharedInstance.Score += 10;
            for(int n = 0; n < scoreGoals.Length; n++)
            {
                if(GUIManager.sharedInstance.Score > scoreGoals[n] && numberStars < n + 1)
                {
                    numberStars++;
                }
            }

            if(gameData != null)
            {
                int highScore = gameData.saveData.highScores[level];
                if(GUIManager.sharedInstance.Score > highScore)
                {
                    gameData.saveData.highScores[level] = GUIManager.sharedInstance.Score;
                }

                int currentStars = gameData.saveData.stars[level];
                if (numberStars > currentStars)
                {
                    gameData.saveData.stars[level] = numberStars;
                }
                gameData.Save();
            }

            yield return new WaitForSeconds(shiftDelay);
            for(int j = 0; j < renderers.Count-1; j++) {
                renderers[j].sprite = renderers[j + 1].sprite;
                renderers[j + 1].sprite = GetNewCandy(x, ySize - 1);
            }
        }

        isShifting = false;
    }

    private Sprite GetNewCandy(int x, int y)
    {
        List<Sprite> possibleCandies = new List<Sprite>();
        possibleCandies.AddRange(prefabs);

        if(x > 0) {
            possibleCandies.Remove(candies[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1) {
            possibleCandies.Remove(candies[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if(y > 0) {
            possibleCandies.Remove(candies[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleCandies[Random.Range(0, possibleCandies.Count)];
    }
}
