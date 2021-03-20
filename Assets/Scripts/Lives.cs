using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour {
    public static int Stage;                // Переменная уровня                     
    public static GameObject LiveInstance;  
    // Переменная для сохранения объекта Lives из предыдущей сцены
    
    void Start()
    {
        if (Stage != 0)
           if (LiveInstance != gameObject)
                Destroy(gameObject);
            // Уничтожение повторно созданного объекта Lives 
        
        DontDestroyOnLoad(gameObject);
        // Запрет на уничтожение объекта Lives при загрузке сцены
    }           
    
}
