using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{

    int score = 200;          // Очки за съедание фрукта       
    public bool eat = false;         // Момент съедания фрукта
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Pacman")
        {
            GetComponent<Collider2D>().enabled = false; // Отключение коллайдера фрукта
            GetComponent<Renderer>().enabled = false;   // Отключение отрисовки фрукта
            Score.SCORE += score;                       // Увеличение количества очков
            eat = true;                                 // Момент съедания фрукта
            GetComponent<AudioSource>().Play();         // Проигрывание звука съедания точки
           
        }
    }
}
