using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class background : MonoBehaviour
{

    [SerializeField] SpriteRenderer backgroundGame;
    [SerializeField] Sprite backgroundDark;
    [SerializeField] Sprite backgroundClear;


    [SerializeField] Sprite buttonDark;
    [SerializeField] Sprite buttonClear;
    [SerializeField] List<Button> buttons;
    [SerializeField] List<TextMeshProUGUI> buttonsTexts;

    // Start is called before the first frame update
    void Start()
    {
        selectedMode(PlayerPrefs.GetInt("Mode", 0));
    }

    public void selectedMode(int i)
    {
        if (i == 0)
        {
            backgroundGame.sprite = backgroundClear;
            foreach (var b in buttons)
            {
                b.image.sprite = buttonClear;
            }
            foreach (var b in buttonsTexts)
            {
                b.color = Color.black;
            }
        }
        else
        {
           backgroundGame.sprite = backgroundDark;
            foreach (var b in buttons)
            {
                b.image.sprite = buttonDark;
            }
            foreach (var b in buttonsTexts)
            {
                b.color = Color.white;
            }
        }

    }
}
