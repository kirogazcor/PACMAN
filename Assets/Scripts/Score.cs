using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public static int SCORE,            // Количество очков
                      HighScore;        // Лучший результат
    public static GameObject ScoreInstance;
    // Переменная для сохранения объекта Score из предыдущей сцены
    void Start()
    {        
        
        if (Lives.Stage != 0)
            if (ScoreInstance != gameObject)
                Destroy(gameObject);
        // Уничтожение повторно созданного объекта Score
        DontDestroyOnLoad(gameObject);
        // Запрет на уничтожение объекта Score при загрузке сцены
        if (Lives.Stage == 0) SCORE = 0;
        // Обнуление очков
        HighScore = PlayerPrefs.GetInt("HighScore");
        // Загрузка лучшего результата
    }
    void Update ()
    {
        if (SCORE >= HighScore)
        {
            HighScore = SCORE;
            // Обновление лучшего результата
            PlayerPrefs.SetInt("HighScore", HighScore);
            // Сохранение лучшего результата
        }
    }
}
