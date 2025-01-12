using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public int monedas;
    public TMPro.TextMeshProUGUI monedasText;
    public TMPro.TextMeshProUGUI mensajeText;

    public GameObject[] cromas;
    public int[] preciosCromas;
 
    private int cromaActual = 0;

 
    void Start()
    {
        monedas = PlayerPrefs.GetInt("Monedas", 0);
        cromaActual = PlayerPrefs.GetInt("Croma", 0);
        

        ActualizarInterfaz();
        MostrarMensaje("");
    }

    public void ComprarMonedas(int cantidad)
    {
        PlaySoundButton();
        monedas += cantidad;
        GuardarDatos();
        ActualizarInterfaz();
        MostrarMensaje($"Has comprado {cantidad} monedas.");
    }

    public void comprarCroma(int index)
    {
        if (index < cromas.Length && index < preciosCromas.Length)
        {
            PlaySoundButton();
            int precio = preciosCromas[index];

            if (monedas >= precio)
            {
                monedas -= precio;
                cromaActual = index;

                GuardarDatos();
                ActualizarInterfaz();
                MostrarMensaje($"¡Croma {index + 1} comprado por {precio} monedas!");
            }
            else 
            {
                MostrarMensaje("No tienes suficientes monedas para este croma.");
            }
        }
        else
        {
            MostrarMensaje("Croma no válido.");
        }
    }

    public void SeleccionarCroma(int index)
    {
        if (index < cromas.Length)
        {
            cromaActual = index;
            GuardarDatos();
            ActualizarInterfaz();
        }
    }

    private void ActualizarInterfaz()
    {
        monedasText.text = "Monedas: " + monedas;
    }

    private void MostrarMensaje(string mensaje)
    {
        if (mensajeText != null)
        {
            mensajeText.text = mensaje;
            CancelInvoke(nameof(LimpiarMensaje)); 
            Invoke(nameof(LimpiarMensaje), 3f); 
        }
    }

    private void LimpiarMensaje()
    {
        if (mensajeText != null)
        {
            mensajeText.text = "";
        }
    }

    private void GuardarDatos()
    {
        PlayerPrefs.SetInt("Monedas", monedas);
        PlayerPrefs.SetInt("Croma", cromaActual);
        PlayerPrefs.Save();
    }

    public void PlaySoundButton()
    {
        AudioManager.PlaySound(SoundType.MenuBotones, 1f);
    }

}
