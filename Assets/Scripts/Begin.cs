using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Begin : MonoBehaviour
{
    GameObject Scores;
	
	void Start ()
    {
        Scores = GameObject.Find("Score");
        if (Scores != null) Scores.GetComponent<Transform>().position = Vector2.zero;
        // Смещение объекта Score в серидину экрана
    }	
	
	void Update ()
    {   // Запуск игры при нажатии пробела
        if (Input.GetKey(KeyCode.Space))
            StartStage();
        // Запуск игры при касании
        else if (Input.touchCount > 0)        
            if (Input.touches[0].phase == TouchPhase.Ended)            
                StartStage();        
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();         // Закрытие игры при нажатии Escape
    }
    // Метод запуска игры
    void StartStage()
    {
        if (Scores != null) Destroy(Scores);
        // Уничтожение объекта Scores
        // Загрузка первого уровня при нажатии пробела
        Lives.Stage = 0;
        SceneManager.LoadScene("Stage");
    }
}
