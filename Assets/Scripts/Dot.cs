using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

    int score;          // Очки за съедание точки
    void Start()
    {
        if (tag == "Energizer") score = 50;
        else score = 10;
    }

    void OnTriggerEnter2D(Collider2D col)
    {        
        if (col.name == "Pacman")                       // Столкновение точки и Пакмана
        {
            GetComponent<Collider2D>().enabled = false; // Отключение коллайдера точки
            GetComponent<Renderer>().enabled = false;   // Отключение отрисовки точки
            Score.SCORE += score;                    // Увеличение количества очков
            GetComponent<AudioSource>().Play();      // Проигрывание звука съедания точки
            if (gameObject.tag == "dots") Destroy(gameObject, 0.2f);    // Уничтожение точки
            else Destroy(gameObject, 0.6f);           // Более долгая задержка при уничтожения 
                                                      // энерджайзера для проигрывания звука съедания
            
        }
    }
}
