using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class PointSystem : MonoBehaviour
{
     
    public Transform player; // Assign your player transform in the Inspector
    [SerializeField] int pointsPerUnit = 12; // Points per unit of upward movement
    public TMP_Text scoreText;
    private float highestY; // Tracks the highest Y position reached by the player
    public static int score = 0; // Current score

    void Start()
    {
        score = 0;
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned in the Inspector!");
            return;
        }

        // Record the player's initial Y position as the starting point
        highestY = player.position.y;

        UpdateScoreText();

    }

    void Update()
    {
        if (player == null) return;

        
        if (player.position.y > highestY)
        {
            
            float distanceTraveled = player.position.y - highestY;
            int pointsToAdd = Mathf.FloorToInt(distanceTraveled * pointsPerUnit);
            score += pointsToAdd;

            
            highestY = player.position.y;

            UpdateScoreText();

        }
    }

    void UpdateScoreText()
    {
        
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            Debug.LogError("Score Text UI element is not assigned in the Inspector!");
        }
    }


}
