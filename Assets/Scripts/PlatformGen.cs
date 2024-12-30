using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGen : MonoBehaviour
{
    public GameObject platformPrefab; // Regular platform prefab
    public GameObject movingplatformPrefab; // Moving platform prefab
    public GameObject breakingplatformPrefab; // Breaking platform prefab
    public GameObject coinPrefab; // Coin prefab
    [SerializeField] float platformSpacing = 2.5f; // Distance between platforms
    private float highestPlatformY = -5f; // Highest platform Y position
    private float minX, maxX; // platformlarýn ekranýn sol ve sað sýnýrlarý içinde kalmasýný saðlar
    public LayerMask platformLayerMask; // LayerMask to define which layers should be checked for overlap
    public GameObject cherryPrefab;
    //bool isObjectSpawned = false; // Platformda obje spawn edildi mi?

    void Start()
    {
        // Generate initial platforms
        for (int i = 0; i < 5; i++)
        {
            GeneratePlatform();
        }

        // Calculate screen bounds
        CalculateScreenBounds();
    }

    void Update()
    {
        // Generate new platforms if player moves higher
        if (Camera.main.transform.position.y > highestPlatformY - 10f)
        {
            GeneratePlatform();
        }
    }

    void CalculateScreenBounds()
    {
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize * 2f; // Full height of the screen in world units
        float screenWidth = screenHeight * mainCamera.aspect; // Width based on aspect ratio

        // Set left and right screen boundaries
        minX = -screenWidth / 2f + 0.6f; // Left edge with small buffer
        maxX = screenWidth / 2f - 0.6f; // Right edge with small buffer
    }

    void GeneratePlatform()
    {
        bool isMovingPlatformSpawned = false; // Flag to check if moving platform is spawned
        int platformCount = Random.Range(1, 3); // Random number of platforms to spawn (1-4)
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), highestPlatformY + platformSpacing, 0);

        // Loop through and spawn platforms
        for (int i = 0; i < platformCount; i++)
        {
            Vector3 spawnPos = spawnPosition + new Vector3(i * 1.3f, 0, 0); //her platform için X pozisyonunu bir miktar kaydýrarak, platformlar arasýna boþluk býrakýr.

            // Ensure platforms don't spawn outside screen bounds
            if (spawnPos.x >= minX && spawnPos.x <= maxX)
            {
                // Check if position is occupied (no overlap with other platforms)
                if (IsPositionOccupied(spawnPos, new Vector2(1.5f, 0.5f), platformLayerMask))
                {
                    continue; // If position is occupied, try the next position
                }

                // If moving platform is not yet spawned, attempt to spawn it
                if (!isMovingPlatformSpawned)
                {
                    GameObject newPlatform = SpawnPlatform(spawnPos);
                    // Spawn coin only for regular platforms
                    if (newPlatform != null && newPlatform.CompareTag("RegularPlatform"))
                    {
                        bool isObjectSpawned = false; // Bu platform için obje spawn edilmedi
                        TrySpawnCoin(newPlatform, ref isObjectSpawned); // Coin dene
                        TrySpawnCherry(newPlatform, ref isObjectSpawned); // Kiraz dene
                    }

                    // If it's a moving platform, set the flag to prevent others from spawning
                    if (newPlatform != null && newPlatform.CompareTag("MovingPlatform"))
                    {
                        isMovingPlatformSpawned = true; // Allow only one moving platform to spawn
                        break;
                    }
                }  
            }
        }

        highestPlatformY += platformSpacing;
    }

    GameObject SpawnPlatform(Vector3 spawnPosition)
    {
        float rand = Random.Range(0f, 1f); // Random number between 0 and 1
        GameObject platformToSpawn;

        if (rand < 0.7f) // 80% chance for a regular platform
        {
            platformToSpawn = platformPrefab;
        }
        else if (rand < 0.85f) // 15% chance for a moving platform
        {
            platformToSpawn = movingplatformPrefab;
        }
        else // 15% chance for a breaking platform
        {
            platformToSpawn = breakingplatformPrefab;
        }

        GameObject spawnedPlatform = Instantiate(platformToSpawn, spawnPosition, Quaternion.identity);

        return spawnedPlatform;
    }

    void TrySpawnCoin(GameObject platform, ref bool isObjectSpawned)
    {
        if (isObjectSpawned) return; // Zaten obje spawn edilmiþse çýk
        
        float coinSpawnChance = 0.3f; // 30% chance to spawn a coin

        if (Random.value < coinSpawnChance)
        {
            // Platformun X boyutunu alýyoruz
            float platformWidth = platform.transform.localScale.x;

            // Coin'in spawnlanacaðý pozisyonu ayarlýyoruz
            Vector3 coinPosition = platform.transform.position + new Vector3(0, platform.transform.localScale.y / 2f + 0.3f, 0); // Platformun üstüne 0.5 birim ekliyoruz

            // Coin'in X pozisyonu platformun tam ortasýnda olacak þekilde ayarlanýyor
            coinPosition.x = platform.transform.position.x; // Platformun ortasý

            // Coin'i platformun üzerine yerleþtiriyoruz
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            isObjectSpawned = true;
        }
    }

    void TrySpawnCherry(GameObject platform, ref bool isObjectSpawned)
    {
        float cherrySpawnChance =0.05f; // %5 þans
        if (isObjectSpawned) return; // Zaten obje spawn edilmiþse çýk

        if (Random.value < cherrySpawnChance)
        {
            float platformWidth = platform.transform.localScale.x;
            Vector3 cherryPosition = platform.transform.position + new Vector3(0, platform.transform.localScale.y / 2f + 0.3f, 0);

            cherryPosition.x = platform.transform.position.x; // Platformun ortasýnda spawnla

            Instantiate(cherryPrefab, cherryPosition, Quaternion.identity); // Kiraz objesini oluþtur
            isObjectSpawned = true;
        }
    }


    public void UpdateHighestPlatformY(float platformY)
    {
        // Update the highest platform position when a platform is destroyed
        if (platformY >= highestPlatformY)
        {
            highestPlatformY = platformY - platformSpacing;
        }
    }

    // Function to check if the position is already occupied by another platform
    bool IsPositionOccupied(Vector3 position, Vector2 size, LayerMask mask)
    {
        // Use OverlapBox to check for collisions with platforms in the specified LayerMask
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, 0, mask);
        return colliders.Length > 0; // Return true if there's any collision
    }
}
