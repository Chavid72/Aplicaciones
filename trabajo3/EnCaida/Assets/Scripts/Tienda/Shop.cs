using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int monedas;
    public TMPro.TextMeshProUGUI monedasText;

    public GameObject[] cromas;
    public GameObject[] fondos;

    private int cromaActual = 0;
    private int fondosActual = 0;
    
    void Start()
    {
        monedas = PlayerPrefs.GetInt("Monedas", 0);
        cromaActual = PlayerPrefs.GetInt("Croma", 0);
        fondosActual = PlayerPrefs.GetInt("Fondo", 0);

        ActualizarInterfaz();
    }

    public void ComprarMonedas(int cantidad)
    {
        Debug.Log($"ComprarMonedas llamado con cantidad: {cantidad}");
        monedas += cantidad;
        GuardarDatos();
        ActualizarInterfaz();
    }

    public void SeleccionarCromaParacaidista(int index)
    {
        if (index < cromas.Length)
        {
            cromaActual = index;
            GuardarDatos();
            ActualizarInterfaz();
        }
    }

    public void SeleccionarFondo(int index)
    {
        if(index < fondos.Length)
        {
            fondosActual = index;
            GuardarDatos();
            ActualizarInterfaz();
        }
    }

    private void ActualizarInterfaz()
    {
        monedasText.text = "Monedas: " + monedas;
    }

    private void GuardarDatos()
    {
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.SetInt("Croma", cromaActual);
        PlayerPrefs.SetInt("Fondo", fondosActual);
        PlayerPrefs.Save();
    }

}
