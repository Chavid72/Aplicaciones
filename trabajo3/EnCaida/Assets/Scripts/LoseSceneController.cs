using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoseSceneController : MonoBehaviour
{
    public TextMeshProUGUI pointsText;
    public int points;

    public TextMeshProUGUI mejoresPuntajesText;

    void Start()
    {
        points = GameController.points;
        pointsText.text = "Points: " + points;

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
        int[] mejoresPuntajes = new int[3];
        int indiceBatido = -1;

        // Cargar los mejores puntajes actuales.
        for (int i = 0; i < 3; i++)
        {
            mejoresPuntajes[i] = PlayerPrefs.GetInt("MejorPuntaje" + i, 0);
        }

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
        }

        // Guardar los cambios en PlayerPrefs.
        PlayerPrefs.Save();

        return indiceBatido; // Devolver el índice del puntaje batido.
    }
}
