using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject englishSelected; 
    [SerializeField] GameObject spanishSelected; 


    void Start()
    {
        int ID = PlayerPrefs.GetInt("LocalKey",0);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        selectedLenguage(ID);
    }

    public void setLanguage(int ID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[ID];
        selectedLenguage(ID);
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

}

