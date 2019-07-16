using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPanel : MonoBehaviour
{
    public Image thisImage;
    public Sprite thisSprite;
    public Text thisText;
    public string thisString;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void Update()
    {
        transform.localScale = new Vector3(1, 1);
    }

    void Setup()
    {
        thisImage.sprite = thisSprite;
        thisText.text = thisString;
    }
}
