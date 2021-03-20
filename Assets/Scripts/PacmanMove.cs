using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    float speed;                        // Скорость движения пакмана
    Vector2 Nextpos;                    // Координаты следующего положения пакмана
    public Vector2 directionP;          // Направление движения
    private Vector2 startPos;
    private Vector2 swipeDir;
    bool isSwipe = false;
        
    void Start ()
    {
        // Начальные параметры движения пакмана
        Nextpos = (Vector2)transform.position;
        swipeDir = directionP = Vector2.left;
        speed = 5f + Lives.Stage;
        // Запуск корутины начала движения пакмана
        StartCoroutine("StartStage");
    }
    
    void Update()
    {
        //-------------------------------------------------------------------------
        if (Input.touchCount > 0)                                   //  
        {   // если тач только начался (палец коснулся экрана)      //  д
            if (Input.touches[0].phase == TouchPhase.Began)         //  л
                // запоминаем позицию                               //  я
                startPos = Input.touches[0].position;               //
            // если тач окончен (палец оторвался от экрана)         //  a
            if (Input.touches[0].phase == TouchPhase.Moved)         //  n
            {   // вычисляем вектор сдвига                          //  d
                Vector2 swipe = Input.touches[0].position - startPos;// r
                // получаем направление движения                    //  o
                if(swipe.magnitude > 50)                            //  i
                    swipeDir = GetDirection(swipe, directionP);     //  d
                isSwipe = true;
            }                                                       //
        }                                                           //
        //-------------------------------------------------------------------------
        // Выбор направления движения с помощью клавиатуры
        if ((Vector2)transform.position == Nextpos)
        {
            Animator anim = GetComponent<Animator>();
            anim.enabled = false;               // Выключение анимации пакмана
            //-------------------------------------------------------------------------
            if (FreeWay(swipeDir) && isSwipe)       //
            {                                       //
                isSwipe = false;                    // для Android
                directionP = swipeDir;              // 
            }                                       //
            //-------------------------------------------------------------------------
            if (Input.GetKey(KeyCode.UpArrow) && FreeWay(Vector2.up))       //
                directionP = Vector2.up;                                    //  д
            if (Input.GetKey(KeyCode.RightArrow) && FreeWay(Vector2.right)) //  л
                directionP = Vector2.right;                                 //  я
            if (Input.GetKey(KeyCode.DownArrow) && FreeWay(Vector2.down))   //
                directionP = Vector2.down;                                  //  P
            if (Input.GetKey(KeyCode.LeftArrow) && FreeWay(Vector2.left))   //  C
                directionP = Vector2.left;                                  //
            //-------------------------------------------------------------------------
            Nextpos = Transition(directionP);
            if (FreeWay(directionP)) Nextpos = (Vector2)transform.position + directionP;
            if ((Vector2)transform.position != Nextpos) anim.enabled = true;
            // Включение анимации пакмана при движении
        }

        // Изменение параметров анимации
        
        GetComponent<Animator>().SetFloat("DirX", directionP.x);
        GetComponent<Animator>().SetFloat("DirY", directionP.y);
    }
    void FixedUpdate()
    {
        // Движение к следующей позиции
        Vector2 p = Vector2.MoveTowards((Vector2)transform.position, Nextpos, speed*Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }

    // Выбор направления при свайпе
    Vector2 GetDirection(Vector2 swip, Vector2 dir)
    {
        if (Mathf.Abs(swip.x) > Mathf.Abs(swip.y))
        {
            if (swip.x > 0) dir = Vector2.right;
            else dir = Vector2.left;
        }
        else if (swip.x > 0) dir = Vector2.up;
        else dir = Vector2.down;
        return dir;
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
    IEnumerator StartStage()    // Корутина начала движения пакмана
    {        
        yield return new WaitForSeconds(4.5f);
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        // Запуск движения пакмана
    }
}
