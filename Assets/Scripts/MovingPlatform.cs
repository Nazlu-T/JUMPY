using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // Movement speed
    public float moveDistance = 2f; // Platformun sa�a ve sola hareket edece�i mesafe
    private Vector3 startPosition; // Ba�lang�� pozisyonu
    private Vector3 target; // Current movement target

    void Start()
    {
        // Ba�lang�� pozisyonunu kaydediyoruz
        startPosition = transform.position;

        // Ba�lang�� hedefini sa�a do�ru 2 birim ayarl�yoruz
        target = startPosition + new Vector3(moveDistance, 0f, 0f);
    }

    void Update()
    {
        // Platformu hedef pozisyona do�ru hareket ettiriyoruz
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // E�er platform hedef pozisyona ula�t�ysa, y�n de�i�tirecek
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Hedef pozisyonunu de�i�tiriyoruz
            target = (target == startPosition + new Vector3(moveDistance, 0f, 0f))
                ? startPosition - new Vector3(moveDistance, 0f, 0f)
                : startPosition + new Vector3(moveDistance, 0f, 0f);
        }

        
    }

    
}

