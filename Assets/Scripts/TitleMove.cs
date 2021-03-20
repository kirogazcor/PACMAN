using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMove : MonoBehaviour {	
	// Скрипт движения пакмана на начальной сцене
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = 10*Vector2.right;
        if (transform.position.x > 25) transform.position += 50 * Vector3.left;
    }
}
