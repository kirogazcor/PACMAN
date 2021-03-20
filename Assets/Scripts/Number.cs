using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
        
	void Update ()
    {
        
        int number;     // Переменная для хранения очков
        // Выбор отрисовки текущих очков или лучшего результата
        if (transform.parent.gameObject.name == "High") number = Score.HighScore;
        else number = Score.SCORE;
        switch(gameObject.name)
        // Выбор отрисовки разрядов в числе очков
        {
            case "10":
                number = number / 10 - (number / 100) * 10;
                break;
            case "100":
                number = number / 100 - (number / 1000) * 10;
                break;
            case "1000":
                number = number / 1000 - (number / 10000) * 10;
                break;
            case "10000":
                number = number / 10000 - (number / 100000) * 10;
                break;
            case "100000":
                number = number / 100000;
                break;
        }
        // Изменение параметра анимации
        if (gameObject.name != "1") GetComponent<Animator>().SetFloat("Number", number);
        
    }
}
