using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    private Transform destroyPoint; // Reference to the destroy point

    void Start()
    {
        // Find the DestroyPoint object in the scene (or assign it manually if needed)
        destroyPoint = GameObject.Find("DestroyPoint").transform;
    }

    void Update()
    {
        if (destroyPoint != null && transform.position.y < destroyPoint.position.y)
        {
            // Reference to PlatformGen to update the highest platform y
            PlatformGen platformGen = FindObjectOfType<PlatformGen>();
            if (platformGen != null)
            {
                platformGen.UpdateHighestPlatformY(transform.position.y);
            }

            // Destroy the platform if it's below the destroy point
            Destroy(gameObject);
        }
    }


}