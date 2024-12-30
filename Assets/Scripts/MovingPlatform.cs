using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class MovingPlatform : MonoBehaviour
{
    public float speed = 2.0f; // Movement speed
    public float moveDistance = 2f; // Platformun saða ve sola hareket edeceði mesafe
    private Vector3 startPosition; // Baþlangýç pozisyonu
    private Vector3 target; // Current movement target

    void Start()
    {
        // Baþlangýç pozisyonunu kaydediyoruz
        startPosition = transform.position;

        // Baþlangýç hedefini saða doðru 2 birim ayarlýyoruz
        target = startPosition + new Vector3(moveDistance, 0f, 0f);
    }

    void Update()
    {
        // Platformu hedef pozisyona doðru hareket ettiriyoruz
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Eðer platform hedef pozisyona ulaþtýysa, yön deðiþtirecek
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Hedef pozisyonunu deðiþtiriyoruz
            target = (target == startPosition + new Vector3(moveDistance, 0f, 0f))
                ? startPosition - new Vector3(moveDistance, 0f, 0f)
                : startPosition + new Vector3(moveDistance, 0f, 0f);
        }

        
    }

    
}

