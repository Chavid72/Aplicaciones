using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject pointsUI;
    private TextMeshProUGUI pointsText;

    public int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        pointsText = pointsUI.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            points += 10;
            //pointsText = points;
        }
    }
}
