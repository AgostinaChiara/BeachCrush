using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "World", menuName ="Level")]
public class Level : ScriptableObject
{
    [Header("Board Dimensions")]
    public int width;
    public int heigth;

    [Header ("Available candies")]
    public GameObject[] candies;

    [Header ("Score Goals")]
    public int[] scoreGoals;

    [Header("End Game Requirements")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
}
