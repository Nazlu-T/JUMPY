using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private bool isStepped = false; // To prevent multiple triggers
   
    

   

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player and the platform hasn't been stepped on yet
        if (collision.gameObject.CompareTag("Player") && !isStepped)
        {
            isStepped = true; // Mark the platform as stepped on

           

            // Destroy the platform after the animation plays (adjust delay to match animation duration)
            Destroy(gameObject, 0.6f); // 1 second delay for fade-out
        }

        
    }
    
    
}
