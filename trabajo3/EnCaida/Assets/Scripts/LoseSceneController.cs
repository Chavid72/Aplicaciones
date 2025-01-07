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

        data.Add("MejorPuntaje0", 0);
        data.Add("MejorPuntaje1", 0);
        data.Add("MejorPuntaje2", 0);

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

        Debug.Log("Te imprimo el mejor puntaje !!!!!");

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
        debug_text.text = "" + data["MejorPuntaje0"];
    }
}
