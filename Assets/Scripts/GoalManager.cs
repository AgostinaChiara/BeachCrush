using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    public GameObject goalLoseParent;

    private BoardManager board;
    private GUIManager guiManager;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<BoardManager>();
        guiManager = FindObjectOfType<GUIManager>();

        GetGoals();
        SetupGoals();
    }

    void GetGoals()
    {
        if (board != null)
        {
            if (board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        levelGoals = board.world.levels[board.level].levelGoals;
                        for(int i = 0; i < levelGoals.Length; i++)
                        {
                            levelGoals[i].numberCollected = 0;
                        }
                    }
                }
            }
        }
    }

    void SetupGoals()
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            //Create a new Goal Panel at the goalIntroParent position
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);
            //Set the image and the goal
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;

            //Create a new Goal Panel at the goalGameParent position
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform, true);

            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }


    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for(int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if(levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
        }
        if(goalsCompleted >= levelGoals.Length)
        {
            StartCoroutine(GUIManager.sharedInstance.WinGame());
        }
    }

    public void CollectCandy(string candyToCompare)
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if (candyToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }

    public void CompareGoals(string goalToCompare)
    { 
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if(goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
            
        }
    }

    public void SetCandiesLeft()
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            GameObject goal = Instantiate(goalPrefab, goalLoseParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalLoseParent.transform);
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = Mathf.Abs(levelGoals[i].numberNeeded - levelGoals[i].numberCollected).ToString();
            if(panel.thisString == "1" || panel.thisString == "-1")
            {
                panel.thisString = "0";
                if (panel.thisString == "-2" || panel.thisString == "-3")
                {
                    panel.thisString = "0";
                }
            }
        }
    }
}
