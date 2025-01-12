using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    public string NestorWeb = "https://nbgonzalez.github.io/";
    public string CrisWeb = "https://cristinamartinez24.github.io/";
    public string DavidWeb = "";
    public string SergioWeb = "";
    public string LuisWeb = "";
    public string LolaWeb = "";
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
        Application.OpenURL(NestorWeb);
    }
    public void OpenCrisWeb()
    {
        Application.OpenURL(CrisWeb);
    }
}
