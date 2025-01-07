using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.CloudSave;

public class LoseSceneController : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI debug_text;
    public int points;

    public TextMeshProUGUI mejoresPuntajesText;

    Dictionary<string, object> data = new Dictionary<string, object>();
    int[] mejoresPuntajes = new int[3];

    void Start()
    {
        points = GameController.points;
        pointsText.text = "Points: " + points;
        LoadData();
        // Guardar el nuevo mejor puntaje si aplica y obtener el índice del puntaje batido.
        int indiceBatido = GuardarMejorPuntaje(points);

        // Mostrar los mejores puntajes.
        string mejoresPuntajes = "";
        for (int i = 0; i < 3; i++)
        {
            int puntaje = PlayerPrefs.GetInt("MejorPuntaje" + i, 0);
            if (i == indiceBatido)
            {
                mejoresPuntajes += $"<size=36><color=orange>{i + 1}. {puntaje}</color></size>\n";
            }
            else
            {
                mejoresPuntajes += $"{i + 1}. {puntaje}\n";
            }
        }
        mejoresPuntajesText.text = mejoresPuntajes;
    }

    int GuardarMejorPuntaje(int nuevoPuntaje)
    {
        //int[] mejoresPuntajes = new int[3];
        int indiceBatido = -1;

        // Cargar los mejores puntajes actuales.
        for (int i = 0; i < 3; i++)
        {
            mejoresPuntajes[i] = PlayerPrefs.GetInt("MejorPuntaje" + i, 0);
        }
        //LoadData();

        // Comparar el nuevo puntaje con los mejores.
        for (int i = 0; i < 3; i++)
        {
            if (nuevoPuntaje > mejoresPuntajes[i])
            {
                // Desplazar los puntajes hacia abajo.
                for (int j = 2; j > i; j--)
                {
                    mejoresPuntajes[j] = mejoresPuntajes[j - 1];
                }
                // Insertar el nuevo puntaje.
                mejoresPuntajes[i] = nuevoPuntaje;
                indiceBatido = i;
                break;
            }
        }

        // Guardar los mejores puntajes actualizados en PlayerPrefs.
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetInt("MejorPuntaje" + i, mejoresPuntajes[i]);

            if (data.ContainsKey("MejorPuntaje" + i))
            {
                // Actualizar el valor si la clave ya existe
                data["MejorPuntaje" + i] = mejoresPuntajes[i];
            }
            else
            {
                // Agregar una nueva clave si no existe
                data.Add("MejorPuntaje" + i, mejoresPuntajes[i]);
            }
        }

        // Guardar los cambios en PlayerPrefs.
        PlayerPrefs.Save();

        SaveData();

        return indiceBatido; // Devolver el índice del puntaje batido.
    }
    public async void SaveData()
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async void LoadData()
    {
        
        var keysToLoad = new HashSet<string>()
        {
            "MejorPuntaje0",
            "MejorPuntaje1",
            "MejorPuntaje2",
        };
        Debug.Log("Te imprimo el mejor puntaje !!!!!");
        /*
        foreach (var key in keysToLoad) 
        {
            Debug.Log($"{key}");
        }
        */

        var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);
        var punt0 = loadedData["MejorPuntaje0"];
        Debug.Log("1er Puntaje: " + punt0);
        foreach (var key in loadedData)
        {
            Debug.Log($"{key}");
        }

        /*
        for (int i = 0; i < mejoresPuntajes.Length; i++) 
        {
            mejoresPuntajes[i] = loadedData["MejorPuntaje" + i].GetHashCode();
        }
        */

        var mejorPuntj0 = loadedData["MejorPuntaje0"];
        
        Debug.Log(mejorPuntj0);


        /*
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "keyName" });
        for (int i = 0;i < 3; i++)
        {
            playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "MejorPuntaje" + i });
            if (playerData.TryGetValue("MejorPuntaje" + i, out var keyName))
            {
                data["MejorPuntaje" + i] = keyName.Value.GetAs<string>();
                Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
            }
        }
        */




        //var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "MejorPuntaje1" });

        //debug_text.text = playerData.Values.ToString();
        //debug_text.text = playerData["MejorPuntaje1"].ToString();
        /*
        if (playerData.TryGetValue("keyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
            debug_text.text = ($"keyName: {keyName.Value.GetAs<string>()}");
        }
        */
    }
}
