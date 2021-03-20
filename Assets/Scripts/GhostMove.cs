using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour {

    float MaxSpeed;            // Максимальная скорость движения призрака
    int score = 100;           // Очки за съедание призрака

    readonly Vector2[] TargPoses = new Vector2[6]   // Целевые точки
    {
         new Vector2(0f,1f),    // Дом
         new Vector2(3f,3f),    // Выход из дома
         new Vector2(9f,15f),   // Разбегание Blinky
         new Vector2(-9f,15f),  // Разбегание Pinky
         new Vector2(9f,-15f),  // Разбегание Inky
         new Vector2(-9f,-15f)  // Разбегание Clyde
    };
    
    public enum Mode
    {
        Chase,          // Преследование
        Scatter,        // Разбегание
        Frightened,     // Испуг 
        Home,           // Возврат в дом
        Exit            // Выход из дома
    }
    public Mode MoveMode;                   // Переменная режима движения призрака
    Mode TempMode, TempMoveMode;            // Временное сохранение режима движения
    Vector2 Nextpos;                        // Координаты следующего положения призрака
    Vector2 direction;                      // Направление движения
    Vector2 TargPos;                        // Целевая точка призрака
    GameObject blinky, pacman, dots, energizers;    // Ссылки на объекты Блинки, Пакман, Dots и Energizers
    int RemainedDots;                       // Несъеденные точки
    

    void Start()
    {
        // Получение ссылкок на объекты Блинки, Пакман, Dots и Energizers
        dots = GameObject.Find("Dots");
        energizers = GameObject.Find("Energizers");
        blinky = GameObject.Find("Blinky");
        pacman = GameObject.FindGameObjectWithTag("Pacman");
        
        RemainedDots = dots.transform.childCount + energizers.transform.childCount;
        // Вычисление количества несъеденных точек
        // Получение начальных параметров движения
        Nextpos = (Vector2)transform.position;
        direction = Vector2.zero;
        MaxSpeed = 4.5f + Lives.Stage;
        TargPos = transform.position; 
        
        StartCoroutine("StartStage");                       
        //Запуск корутины начала уровня
        
    }


    void Update()
    {        
        float PropRemainedDots = ((float)(dots.transform.childCount + 
            energizers.transform.childCount)) / (float)RemainedDots;
        
        switch (transform.name)     // Выбор призрака
        {
            case "Blinky":
                switch (MoveMode)   //Выбор режима движения Blinky
                {                   // Целевых точек и анимации
                    case Mode.Scatter:
                        TargPos = TargPoses[2];
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                    case Mode.Frightened:
                        TargPos = TargPoses[2];
                        GetComponent<Animator>().SetBool("EGhost", true);
                        break;
                    case Mode.Chase:
                        TargPos = (Vector2)pacman.GetComponent<Transform>().position;
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                }
                break;
            case "Pinky":
                if (PropRemainedDots > 0.95f) // Момент выхода Pinky из дома
                    MaxSpeed = 0;
                else MaxSpeed = 4.5f + Lives.Stage;
                switch (MoveMode)         //Выбор режима движения Pinky
                {                         // Целевых точек и анимации
                    case Mode.Scatter:
                        TargPos = TargPoses[3];
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                    case Mode.Frightened:
                        TargPos = TargPoses[3];
                        GetComponent<Animator>().SetBool("EGhost", true);
                        break;
                    case Mode.Chase:
                        TargPos = (Vector2)pacman.GetComponent<Transform>().position +
                            4 * pacman.GetComponent<PacmanMove>().directionP;
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                }        
                
                break;
            case "Inky":
                if (PropRemainedDots > 0.9f) // Момент выхода Inky из дома
                    MaxSpeed = 0;
                else MaxSpeed = 4.5f + Lives.Stage;
                switch (MoveMode)        //Выбор режима движения Inky
                {                        // Целевых точек и анимации
                    case Mode.Scatter:
                        TargPos = TargPoses[4];
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                    case Mode.Frightened:
                        TargPos = TargPoses[4];
                        GetComponent<Animator>().SetBool("EGhost", true);
                        break;
                    case Mode.Chase:
                        TargPos = 2 * ((Vector2)pacman.GetComponent<Transform>().position +
                            2 * pacman.GetComponent<PacmanMove>().directionP) -
                            (Vector2)blinky.GetComponent<Transform>().position;
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                }
                
                break;
            case "Clyde":
                if (PropRemainedDots > 0.8f) // Момент выхода Clyde из дома
                    MaxSpeed = 0;
                else MaxSpeed = 4.5f + Lives.Stage;
                switch (MoveMode)        //Выбор режима движения Clyde
                {                        // Целевых точек и анимации
                    case Mode.Scatter:
                        TargPos = TargPoses[5];
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                    case Mode.Frightened:
                        TargPos = TargPoses[5];
                        GetComponent<Animator>().SetBool("EGhost", true);
                        break;
                    case Mode.Chase:
                        Vector2 Dist = (Vector2)pacman.GetComponent<Transform>().position - (Vector2)transform.position;
                        if (Dist.magnitude > 8) TargPos = (Vector2)pacman.GetComponent<Transform>().position;
                        else TargPos = TargPoses[5];
                        GetComponent<Animator>().SetBool("EGhost", false);
                        GetComponent<Animator>().SetBool("Died", false);
                        break;
                }
                
                break;
        }
         
        ChoiceDirection();
        
        // Изменение параметров анимации
        GetComponent<Animator>().SetFloat("DirX", direction.x);
        GetComponent<Animator>().SetFloat("DirY", direction.y);
    }
    void FixedUpdate()
    {
        float speed = MaxSpeed;
        if (MoveMode == Mode.Frightened || MoveMode == Mode.Exit)
            speed = MaxSpeed / 2;   // Уменьшение скорости в режиме испуга и выхода из дома
        // Движение к следующей позиции
        Vector2 p = Vector2.MoveTowards(transform.position, Nextpos, speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }
    // Выбор направления движения
    void ChoiceDirection ()
    {
        if ((Vector2)transform.position == Nextpos)
        {
            Nextpos = Transition(direction);
            if ((Vector2)transform.position == Vector2.zero)
                StartCoroutine("Exit");
            // Выход из дома при нулевых координатах призрака
            if (TempMode != MoveMode && TempMode != Mode.Exit) direction = -direction;
            // Изменение направления на противоположное при смене режима  
            
                Vector2 LastDir = direction;        // Запоминание текущего направления

                if (FreeWay(Vector2.up))
                {
                    direction = OptimalDirection(Vector2.up, Nextpos, TargPos, LastDir);
                    Nextpos = (Vector2)transform.position + direction;
                }
                if (FreeWay(Vector2.right))
                {
                    direction = OptimalDirection(Vector2.right, Nextpos, TargPos, LastDir);
                    Nextpos = (Vector2)transform.position + direction;
                }
                if (FreeWay(Vector2.down))
                {
                    direction = OptimalDirection(Vector2.down, Nextpos, TargPos, LastDir);
                    Nextpos = (Vector2)transform.position + direction;
                }
                if (FreeWay(Vector2.left))
                {
                    direction = OptimalDirection(Vector2.left, Nextpos, TargPos, LastDir);
                    Nextpos = (Vector2)transform.position + direction;
                }
                
            TempMode = MoveMode;    // Запоминание режима движения для проверки его изменения
        }
        
    }
    // Проверка на наличие перехода
    Vector2 Transition(Vector2 dir)
    {
        Vector2 pos = (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        if (hit.transform.tag == "Transition")
            return (transform.position = -pos);
        else return pos;
    }
    // Проверка наличия препятствий в направлении движения
    bool FreeWay(Vector2 dir)
    {
        Vector2 pos = (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.transform.tag != "Border");
    }

    // Проверка направления движения на оптимальность
    Vector2 OptimalDirection(Vector2 Dir, Vector2 NextP, Vector2 Target, Vector2 Lastdir)
    {
        Vector2 Dist = Target - (Vector2)transform.position - Dir;
        Vector2 DistCurr = Target - NextP;
        if ((Dist.magnitude < DistCurr.magnitude || NextP == (Vector2)transform.position)
            && Dir != -Lastdir) return Dir;
        else if (Target == (Vector2)transform.position)
            return (Vector2.zero);
        else return (NextP - (Vector2)transform.position);        
    }
    IEnumerator StartStage()    // Корутина запука начала уровня
                                // и начала игры после потери жизни пакманом
    {
        yield return new WaitForSeconds(4.5f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        // Запуск движения призрака
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Pacman" && MoveMode == Mode.Frightened) 
            // Столкновение с пакманом в режиме испуга
            StartCoroutine("Death");       
    }
    IEnumerator Death()     // Корутина смерти призрака
    {
        Score.SCORE += score;                    // Увеличение количества очков
        GetComponent<AudioSource>().Play();      // Проигрывание звука съедания призрака;
        // Изменение параметров анимации
        GetComponent<Animator>().SetBool("EGhost", false);
        GetComponent<Animator>().SetBool("Died", true);         
        yield return StartCoroutine (Home());
        // Продолжение после возврата в дом
        GetComponent<Animator>().SetBool("Died", false);
        StartCoroutine("Exit");         // Запуск корутины выхода из дома
    }
    
    IEnumerator Home ()     // Корутина возврата в дом
    {        
        TargPos = TargPoses[0]; // Целевая точка - дом
        MoveMode = Mode.Home;   // Включение режима возврата в дом
        while ((Vector2)transform.position != TargPos)
        {
            //выполнение до достижения целевой точки
            yield return null;
        }
    }
     IEnumerator Exit()     // Корутина выхода из дома
    {
        TempMoveMode = MoveMode;
        // Сохранение текущего режима движения
        TargPos = TargPoses[0]+Vector2.up;
        // Первая точка маршрута выхода из дома
        MoveMode = Mode.Exit;   // Включение режима выхода из дома
        while ((Vector2)transform.position != TargPos)
        {
            //выполнение до достижения целевой точки
            yield return null;
        }
        TargPos = TargPoses[1];
        // Вторая точка маршрута выхода из дома
        while ((Vector2)transform.position != TargPos)
        {
            //выполнение до достижения целевой точки
            yield return null;
        }
        if (MoveMode == Mode.Frightened || TempMoveMode == Mode.Frightened)
        // Проверка включался ли режим испуга до выхода или во время выхода из дома
        MoveMode = Mode.Frightened;      // Включение режима испуга
        else MoveMode = Mode.Scatter;    // Включение режима разбегания
    }
}
