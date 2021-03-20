using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanController : MonoBehaviour
{    
    GameObject[] ghosts;            // Ссылка на объекты призраков
    int Time;                       // Переменная времени таймера
    GhostMove[] GhostMoves;         // Ссылки на скрипты управления призраками
    bool EnergizerMode,             // Режим энерджайзера
         pause;                     // Режим паузы
    float KChase;                   // Коэффициент уменьшения времени режима разбегания
    float KScatter;                 // Коэффициент увеличения времени режима преследования

    void Start()
    {
        EnergizerMode = pause = false;
        KChase = Mathf.Pow(0.75f, (float)(Lives.Stage));
        // Коэффициент уменьшения времени режима разбегания
        KScatter = Mathf.Pow(1.5f, (float)(Lives.Stage));
        // Коэффициент увеличения времени режима преследования
        ghosts = GameObject.FindGameObjectsWithTag("Ghosts");
        // Ссылка на объекты призраков
        GhostMoves = new GhostMove[ghosts.Length];
        // Ссылки на скрипты GhostMoves призраков
        for (int i = 0; i < ghosts.Length; i++)
        {
            GhostMoves[i] = ghosts[i].GetComponent<GhostMove>();
            GhostMoves[i].MoveMode = GhostMove.Mode.Scatter;
            // Перевод призраков в режим разбегания
        }
        Time = 0;                // Обнуление таймера
        StartCoroutine("Timer"); // Запуск таймера
    }

    void Update()
    {
        if (!EnergizerMode)  // Исключение выбора режимов разбегание и преследование
        {                    // во время действия энерджайзера
            // Первая волна
            if (Time >= (int)(5f + 7f * KChase) && Time < (int)(5f + 7f * KChase + 20f * KScatter))
            {
               
                // Режим преследование по таймеру
                foreach (GhostMove g in GhostMoves)
                {
                    if (!(g.MoveMode == GhostMove.Mode.Home || g.MoveMode == GhostMove.Mode.Exit))
                    // Исключение выбора режимов разбегание и преследование
                    // во время возврата в дом и выхода из дома
                        g.MoveMode = GhostMove.Mode.Chase;
                }
            }
            // Вторая волна
            if (Time >= (int)(5f + 7f * KChase + 20f * KScatter) &&
                Time < (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase))
            {
                
                // Режим разбегание по таймеру
                foreach (GhostMove g in GhostMoves)
                {
                    if (!(g.MoveMode == GhostMove.Mode.Home || g.MoveMode == GhostMove.Mode.Exit))
                        // Исключение выбора режимов разбегание и преследование
                        // во время возврата в дом и выхода из дома
                        g.MoveMode = GhostMove.Mode.Scatter;
                }
            }
            if (Time >= (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase) &&
                Time < (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase + 20f * KScatter))
            {
                
                // Режим преследование по таймеру
                foreach (GhostMove g in GhostMoves)
                {
                    if (!(g.MoveMode == GhostMove.Mode.Home || g.MoveMode == GhostMove.Mode.Exit))
                        // Исключение выбора режимов разбегание и преследование
                        // во время возврата в дом и выхода из дома
                        g.MoveMode = GhostMove.Mode.Chase;
                }
            }
            // Третья волна
            if (Time >= (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase + 20f * KScatter) &&
                Time < (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase +
                20f * KScatter + 7f * KChase))
            {
                
                // Режим разбегание по таймеру
                foreach (GhostMove g in GhostMoves)
                {
                    if (!(g.MoveMode == GhostMove.Mode.Home || g.MoveMode == GhostMove.Mode.Exit))
                        // Исключение выбора режимов разбегание и преследование
                        // во время возврата в дом и выхода из дома
                        g.MoveMode = GhostMove.Mode.Scatter;
                }
            }
            if (Time >= (int)(5f + 7f * KChase + 20f * KScatter + 7f * KChase +
                20f * KScatter + 7f * KChase))
            {
                
                // Режим преследование по таймеру
                foreach (GhostMove g in GhostMoves)
                {
                    if (!(g.MoveMode == GhostMove.Mode.Home || g.MoveMode == GhostMove.Mode.Exit))
                        // Исключение выбора режимов разбегание и преследование
                        // во время возврата в дом и выхода из дома
                        g.MoveMode = GhostMove.Mode.Chase;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) Pause();       // Остановка игры при нажатии клавиши пробела

    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Ghosts")        // Столкновение с призраком
        {
            GhostMove GM = col.gameObject.GetComponent<GhostMove>();
            // Ссылка на скрипт GhostMoves призрака
            // с которым столкнулся пакман
            if (!(GM.MoveMode == GhostMove.Mode.Frightened || GM.MoveMode == GhostMove.Mode.Home))
                // Исключение смерти при столкновении
                // с испуганным или мертвым призраком
                StartCoroutine("Death");    // Запуск корутины смерти пакмана
        }
        if (col.tag == "Energizer")
        {
            StopCoroutine("Energizer");
            StartCoroutine("Energizer");    // Запуск корутины съедания энерджайзера            
        }
    }
    // Пауза
    void Pause()
    {
        if (pause)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            // Запуск движения пакмана
            foreach (GameObject g in ghosts)
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            // Запуск движения призраков
            pause = false;
        }
        else
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            // Остановка движения пакмана
            foreach (GameObject g in ghosts)
                g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            // Остановка движения призраков
            pause = true;
        }
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

    IEnumerator Death()     // Корутина смерти пакмана
    {
                
        GameObject GameController = GameObject.Find("GameController");
        // Ссылка на объект GameController
        GameObject[] lives = GameObject.FindGameObjectsWithTag("Lives");
        // Ссылка на объекты жизней

        for (int i = 1; i < lives.Length; i++)
        {
            // Выбор крайней левой жизни для стирания
            float sort = lives[0].GetComponent<Transform>().position.x;
            if (sort > lives[i].GetComponent<Transform>().position.x)
                lives[0] = lives[i];
        }
        Destroy(lives[0]);
        // Отнять жизнь
        StopSound(GameController);
        // Выключение сирены
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        // Остановка движения пакмана
        foreach (GameObject g in ghosts)
            g.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        // Остановка движения призраков

        foreach (GameObject g in ghosts) Destroy(g, 1f);
        // Уничтожение призраков                        
        Animator anim = GetComponent<Animator>();
        anim.enabled = false;                    // Выключение анимации пакмана 
        Destroy(gameObject, 3f);                 // Уничтожение пакмана через 3 секунды
        yield return new WaitForSeconds(1f);
        anim.SetBool("Died", true);              // Изменение параметра анимации пакмана
        anim.enabled = true;                     // Включение анимации уничтожения пакмана
        PlaySound(this.gameObject);              // Проигрывание звука уничтожения пакмана

    }
    IEnumerator Timer() // Корутина таймера
    {
        for (;;)
        {
            Time++;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator Energizer() // Корутина съедания энерджайзера
    {
        StopCoroutine("Timer");     // Остановка таймера
        EnergizerMode = true;       // Включение режима энерджайзера
        foreach (GameObject g in ghosts)
            if (g.GetComponent<GhostMove>().MoveMode != GhostMove.Mode.Home)
            {
                // Для призраков не бегущих домой 
                g.GetComponent<Animator>().SetBool("Died", false);
                // Выключение анимации окончания режима испуга призраков
                g.GetComponent<GhostMove>().MoveMode = GhostMove.Mode.Frightened;
                // Включение режима испуга для призраков
            }
        
        yield return new WaitForSeconds(5f * KChase);
        // Через 5 секунд
        foreach (GameObject g in ghosts)
            if (g.GetComponent<GhostMove>().MoveMode == GhostMove.Mode.Frightened ||
                // Для призраков в режиме испуга
                (g.GetComponent<GhostMove>().MoveMode == GhostMove.Mode.Exit && EnergizerMode))
                // или для призраков выходящих из дома, когда действует энерджайзер
            {   
                // включение анимации окончания режима испуга призраков
                g.GetComponent<Animator>().SetBool("EGhost", true);
                g.GetComponent<Animator>().SetBool("Died", true);
            }
        
        yield return new WaitForSeconds(2f * KChase);
        // Через 2 секунды
        StartCoroutine("Timer");    // Запуск таймера
        EnergizerMode = false;      // Выключение режима энерджайзера        
    }
    
}
