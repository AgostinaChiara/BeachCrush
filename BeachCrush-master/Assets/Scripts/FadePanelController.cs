using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour
{
    public Animator panelAnim;
    public Animator gameInfoAnim;

    public void Ok()
    {
        if(panelAnim != null && gameInfoAnim != null)
        {
            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }

    public void GameOver()
    {
        panelAnim.SetBool("Out", false);
    }

    public IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        BoardManager board = FindObjectOfType<BoardManager>();
        board.currentGameState = GameState.inGame;
    }
}
