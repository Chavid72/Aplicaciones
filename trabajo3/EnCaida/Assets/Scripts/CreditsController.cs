using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public string NestorWeb = "https://nbgonzalez.github.io/";
    public string CrisWeb = "https://cristinamartinez24.github.io/";
    public string DavidWeb = "";
    public string SergioWeb = "";
    public string LuisWeb = "https://www.instagram.com/lsui_23/";
    public string LolaWeb = "https://www.instagram.com/solum.studios/";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenNestorWeb()
    {
        PlayButtonSound();
        Application.OpenURL(NestorWeb);
    }
    public void OpenCrisWeb()
    {
        PlayButtonSound();
        Application.OpenURL(CrisWeb);
    }

    public void OpenLolaWeb()
    {
        PlayButtonSound();
        Application.OpenURL(LolaWeb);
    }

    public void OpenLuisWeb()
    {
        PlayButtonSound();
        Application.OpenURL(LuisWeb);
    }

    public void PlayButtonSound()
    {
        AudioManager.PlaySound(SoundType.MenuBotones, 1f);
    }
}
