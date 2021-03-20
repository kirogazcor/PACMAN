using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
       
    public GameObject[] Reloaded;   // Ссылки на перезагружаемые объекты
    
    int RemainedDots,               // Несъеденные точки
        IndexFruit,                 // Индекс фрукта
        AllDots;                    // Все точки
    GameObject[] ReloadedTemp;      // Временные cсылки на пакмана и призраков

    void Awake()
    {
        GameObject dots = GameObject.Find("Dots");
        GameObject energizers = GameObject.Find("Energizers");
        AllDots = energizers.transform.childCount + dots.transform.childCount;
        // Получение количества всех точек
        IndexFruit = 0;
        StartCoroutine("StartStage");   // Запуск корутины начала уровня
        ReloadedTemp = new GameObject[Reloaded.Length];
    }

    void Update()
    {
        // Ссылки на объекты fruit, Dots и Energizers
        GameObject fruits = GameObject.Find("fruit");
        GameObject dots = GameObject.Find("Dots");
        GameObject energizers = GameObject.Find("Energizers");        
        if (GameObject.FindGameObjectWithTag("Pacman") == null)     // Проверка жив ли пакман
        {            
            if (GameObject.FindGameObjectWithTag("Lives") != null) // Проверка наличия жизней
            {
                GameObject ghosts = GameObject.Find("Ghosts");          // Ссылка на объект Ghosts
                string[] nameReloaded = new string[Reloaded.Length];    // Массив имён перезагружаемых объектов              
                int i = 0;
                foreach (GameObject g in Reloaded)
                {
                    nameReloaded[i] = g.name;                  // Сохранение имен пакмана и призраков
                    ReloadedTemp[i++] = Instantiate(g);        // Загрузка пакмана и призраков                    
                }
                StartCoroutine("StartStage");

                // Присвоение пакману и призракам первоначальных имен
                for (i = 0; i < ReloadedTemp.Length; i++)
                {
                    ReloadedTemp[i].name = nameReloaded[i];
                    if (i > 0) ReloadedTemp[i].transform.parent = ghosts.transform;
                }

            }
            else GameOver();        // Завершение игры при проигрыше
        }
        
        RemainedDots = energizers.transform.childCount;
        if (RemainedDots == 0) StopSound(this.gameObject);   // Выключение сирены, если съедены все энерджайзеры
        RemainedDots += dots.transform.childCount;
        if (RemainedDots == 0) StartCoroutine("Win");       // Запуск корутины победы, если все точки съедены
        else
        {
            if (fruits.GetComponent<Fruit>().eat)           // В момент съедения фрукта
            {
                AllDots = RemainedDots;                     // сохранение количества всех оставшихся точек
                fruits.GetComponent<Fruit>().eat = false;   // выключение момента съедения фрукта
            }
            if (IndexFruit <= 4 && fruits.GetComponent<Collider2D>().enabled == false)
                // Проверка не съедены ли в игре все 5 фруктов,
                // а также нет ли на поле фрукта в настоящий момен
                if (RemainedDots == AllDots - (int)(AllDots / (6 - IndexFruit)))
                {   // Момент появления фрукта
                    fruits.GetComponent<Collider2D>().enabled = true; // включение коллайдера фрукта
                    fruits.GetComponent<Renderer>().enabled = true;   // включение отрисовки фрукта
                    AllDots = RemainedDots;          // сохранение количества всех оставшихся точек
                    // Изменение параметров анимации
                    fruits.GetComponent<Animator>().SetInteger("fruits", IndexFruit);
                    IndexFruit++;   // Переключение на индекс фрукта, который появится следующим
                }
        }
        if (Input.GetKey(KeyCode.Escape)) GameOver();       // Завершение игры при нажатии клавиши Escape

    }
    // Метод остановки аудио
    void StopSound(GameObject g)
    {
        AudioSource sound = g.GetComponent<AudioSource>();  
        sound.Stop();
    }
    // Метод запуска аудио
    void PlaySound(GameObject g)
    {
        AudioSource sound = g.GetComponent<AudioSource>();  
        sound.Play();
    }
    // Метод остановки анимации
    void StopAnimator(GameObject g)
    {
        Animator anim = g.GetComponent<Animator>();
        anim.enabled = false;
    }
    // Метод запуска анимации
    void PlayAnimator(GameObject g)
    {
        Animator anim = g.GetComponent<Animator>();
        anim.enabled = true;
    }
    // Метод завершения игры
    void GameOver()
    {        
        Destroy(GameObject.Find("Lives"));
        SceneManager.LoadScene("Begin");
    }
    
    IEnumerator Win()     // Корутина победы в уровне
    {
        PlayAnimator(GameObject.Find("BorderField"));
        // Включение анимации победы
        GameObject [] ghosts = GameObject.FindGameObjectsWithTag("Ghosts");
        ReloadedTemp[0] = GameObject.FindGameObjectWithTag("Pacman");
        for (int i = 1; i < ReloadedTemp.Length; i++) ReloadedTemp[i] = ghosts[i - 1];
        // Получение ссылок на пакмана и призраков
        foreach (GameObject g in ReloadedTemp)
            g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                                            // Остановка движения призраков и пакмана
        StopAnimator(ReloadedTemp[0]);      // Выключение анимации пакмана
        yield return new WaitForSeconds(3f);
        Lives.Stage++;                                  // Увеличение уровня
        Score.ScoreInstance = GameObject.Find("Score"); // Сохранение объекта Score
        Lives.LiveInstance = GameObject.Find("Lives");  // Сохранение объекта Lives
        StopAnimator(GameObject.Find("BorderField"));   // Выключение анимации победы
        SceneManager.LoadScene("Stage");                // Запуск следующего уровня
        
    }
    IEnumerator StartStage()    // Корутина запука начала уровня
                                // и начала и игры после потери жизни пакманом
    {
        GetComponent<SpriteRenderer>().enabled = true;  // Отображение надписи READY!
        yield return new WaitForSeconds(4.5f);
        GetComponent<SpriteRenderer>().enabled = false; // Выключение надписи READY!
        PlaySound(this.gameObject);             // Включение сирены
    }
}
