using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    public static int points = 0;
    public int pointMultiplier = 1;
    [SerializeField]private int waitMultiplier = 10;
    [SerializeField]private int waitWind = 10;
    [SerializeField] public GameObject windRight;
    [SerializeField] public GameObject windLeft;
    private Coroutine MultiplierC;
    private float time = 0;
    public float windProb = 0;
    public float windIncrement = 0.03f;
    public bool wind = false;

    private static GameController instance;

    public static GameController Instance
    {
        get
        {
            return instance;
        }
    }

    public void SetMultiplier(int multiplier)
    {
        pointMultiplier = multiplier;
    }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 1 && !wind)
        {
            time = 0;
            if (UnityEngine.Random.Range(0,1f) < windProb)
            {
                int dir = UnityEngine.Random.Range(0, 2);
                float windSpeed;
                if (dir == 0)
                {
                    windLeft.SetActive(true);
                    windSpeed = UnityEngine.Random.Range(-0.2f, -0.05f);
                }
                else
                {
                    windRight.SetActive(true);
                    windSpeed = UnityEngine.Random.Range(0.05f, 0.2f);
                }

                //PlayerController.Instance.wind = windSpeed;
                windProb = 0;
                wind = true;
                AudioManager.PlaySound(SoundType.Wind, 0.5f);
                PlayerController.Instance.StartWind(windSpeed);
                //StartCoroutine(WindEvent());
            }

            else
            {
                windProb += windIncrement;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            //points += 10;
            GameController.points += (10 * pointMultiplier);
            pointsText.text = points.ToString();
        }
    }

    public void StartDoublePoints()
    {
        SetMultiplier(2);
        if (MultiplierC == null)
        {
            MultiplierC = StartCoroutine(Multiplier());
        }

        else
        {
            StopCoroutine(MultiplierC);
            MultiplierC = StartCoroutine(Multiplier());
        }
    }

    private IEnumerator Multiplier()
    {
        yield return new WaitForSeconds(waitMultiplier);
        SetMultiplier(1);
    }

    private IEnumerator WindEvent()
    {
        yield return new WaitForSeconds(waitWind);
        PlayerController.Instance.wind = 0;
        wind = false;
    }
}
