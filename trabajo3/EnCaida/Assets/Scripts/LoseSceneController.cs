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

    }

    public async void SaveData()
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async void LoadData()
    {

        Debug.Log("Te imprimo el mejor puntaje !!!!!");

        // Cargo los puntajes de la nube
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> {
          "MejorPuntaje0", "MejorPuntaje1", "MejorPuntaje2"
        });

        if (playerData.TryGetValue("MejorPuntaje0", out var firstKey))
        {
            Debug.Log($"firstKeyName value: {firstKey.Value.GetAs<int>()}");
            data["MejorPuntaje0"] = firstKey.Value.GetAs<int>();
            mejoresPuntajes[0] = firstKey.Value.GetAs<int>();
        }

        if (playerData.TryGetValue("MejorPuntaje1", out var secondKey))
        {
            Debug.Log($"secondKey value: {secondKey.Value.GetAs<int>()}");
            data["MejorPuntaje1"] = secondKey.Value.GetAs<int>();
            mejoresPuntajes[1] = secondKey.Value.GetAs<int>();
        }

        if (playerData.TryGetValue("MejorPuntaje2", out var thirdKey))
        {
            Debug.Log($"thirdKey value: {thirdKey.Value.GetAs<int>()}");
            data["MejorPuntaje2"] = thirdKey.Value.GetAs<int>();
            mejoresPuntajes[2] = thirdKey.Value.GetAs<int>();
        }
        debug_text.text = "" + data["MejorPuntaje0"] + " array " + mejoresPuntajes[0];

        // Comparar el nuevo puntaje con los mejores.
        for (int i = 0; i < 3; i++)
        {
            if (points > mejoresPuntajes[i])
            {
                // Desplazar los puntajes hacia abajo.
                for (int j = 2; j > i; j--)
                {
                    mejoresPuntajes[j] = mejoresPuntajes[j - 1];
                    data["MejorPuntaje" + j] = mejoresPuntajes[j];
                }
                // Insertar el nuevo puntaje.
                mejoresPuntajes[i] = points;
                data["MejorPuntaje" + i] = points;
                //indiceBatido = i;
                break;
            }
        }

        //Guardo puntajes
        SaveData();

        // Mostrar los mejores puntajes.
        string mejoresPuntajest = "";
        for (int i = 0; i < 3; i++)
        {
            mejoresPuntajest += $"{i + 1}. {mejoresPuntajes[i]}\n";
        }
        mejoresPuntajesText.text = mejoresPuntajest;
        
    }
}
