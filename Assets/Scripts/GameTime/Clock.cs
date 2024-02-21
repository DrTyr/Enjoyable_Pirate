using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI clockText;
    private float realTimeElapsed; // Temps écoulé en temps réel
    private const float gameDayDurationInSeconds = 1200f; // Durée d'une journée de jeu en secondes (20 minutes * 60 secondes/minute)

    void Start()
    {
        GameTime.gameTimeInSeconds = 0f;
        realTimeElapsed = 0f;
        clockText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Mettre à jour le temps de jeu en fonction du temps réel écoulé
        realTimeElapsed += Time.deltaTime;
        GameTime.gameTimeInSeconds = realTimeElapsed * (gameDayDurationInSeconds / 60f); // Échelle de temps

        // Convertir le temps de jeu en heures et minutes
        int hours = Mathf.FloorToInt(GameTime.gameTimeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt(GameTime.gameTimeInSeconds % 3600f / 60f);

        // Formater l'heure au format numérique (hh:mm)
        string timeText = string.Format("{0:00}:{1:00}", hours, minutes);

        //Debug.Log(realTimeElapsed);

        //Debug.Log(timeText);

        // Mettre à jour le texte de l'horloge
        if (clockText != null)
        {
            clockText.text = timeText;
        }
    }
}

public static class GameTime
{
    public static float gameTimeInSeconds;
}