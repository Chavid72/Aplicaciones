using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject englishSelected; 
    [SerializeField] GameObject spanishSelected;

    [SerializeField] GameObject darkModeSelected;
    [SerializeField] GameObject clearModeSelected;

    [SerializeField] AudioMixer ambientMixer; 
    [SerializeField] AudioMixer effectMixer;

    [SerializeField] Slider ambientSlider;
    [SerializeField] Slider effectSlider;

    [SerializeField] Image background;
    [SerializeField] Sprite backgroundDark;
    [SerializeField] Sprite backgroundClear;

    [SerializeField] Sprite buttonDark; 
    [SerializeField] Sprite buttonClear; 
    [SerializeField] List<Button> buttons; 
    [SerializeField] List<TextMeshProUGUI> buttonsTexts; 




    void Start()
    {
        int ID = PlayerPrefs.GetInt("LocalKey",0);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        selectedLenguage(ID);

        float volume = PlayerPrefs.GetFloat("Ambient");
        setAmbientVolume(volume);
        ambientSlider.value = volume;

        float volume2 = PlayerPrefs.GetFloat("Effects");
        setEffectsVolume(volume2);
        effectSlider.value = volume2;

        selectedMode(PlayerPrefs.GetInt("Mode",0));
    }

    public void setLanguage(int ID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        selectedLenguage(ID);
        PlayerPrefs.SetInt("LocalKey", ID);
    }

    public void selectedLenguage(int ID)
    {
        if (ID == 0)
        {
            englishSelected.SetActive(true);
            spanishSelected.SetActive(false);
        }
        else
        {
            englishSelected.SetActive(false);
            spanishSelected.SetActive(true);
        }

    }

    public void selectedMode(int i)
    {
        if (i == 0)
        {
            clearModeSelected.SetActive(true);
            darkModeSelected.SetActive(false);
            background.sprite = backgroundClear;
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
            clearModeSelected.SetActive(false);
            darkModeSelected.SetActive(true);
            background.sprite = backgroundDark;
            foreach (var b in buttons)
            {
                b.image.sprite = buttonDark;
            }
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

    public void setEffectsVolume(float volume)
    {
        PlayerPrefs.SetFloat("Effects", volume);
        effectMixer.SetFloat("VolumeEffects", volume);
    }
    public void setAmbientVolume(float volume)
    {
        PlayerPrefs.SetFloat("Ambient", volume);
        effectMixer.SetFloat("VolumeAmbient", volume);
    }

    public void setMode(int i)
    {
        selectedMode(i);
        PlayerPrefs.SetInt("Mode", i);
    }
}

