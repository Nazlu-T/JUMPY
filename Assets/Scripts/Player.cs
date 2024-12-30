using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))] 

public class Player : MonoBehaviour

{
    public GameManager gameManager;
    [SerializeField] float movespeed = 5f;
    [SerializeField] float jumpForce = 8f;
    //[SerializeField] GameObject[] skins;
    [SerializeField] SpriteRenderer playerImage;
    Rigidbody2D rb;
    Camera mainCamera;
    [SerializeField] AudioClip jumpSound;
    AudioSource audioSource;

   

    //float movement = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; // Get reference to the main camera
        // AudioSource'ý baþlat
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Oyuncu doðrudan sesi çalmasýn

        ChangePlayerSkin();

    }

    public void StartGame()
    {

        Jump();
    }


    void Update()
    {
        // Telefonun saða veya sola eðimini okuyun
        float tilt = Input.acceleration.x;

        // Tilt'i movespeed ile çarparak hareket deðerini ayarlayýn
        float movement = tilt * movespeed;

        // Karakterin hýzýný belirleyin
        Vector2 velocity = rb.velocity;
        velocity.x = movement;
        rb.velocity = velocity;

        // Clamp player's horizontal position within camera bounds
        ClampPlayerHorizontalPosition();

        // Check if the player is out of the camera's vertical view
        if (IsOutOfVerticalCameraView())
        {
            Debug.Log("Player has gone out of the camera view vertically! Ending game...");
            GameManager.Instance.EndGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.IsGameActive) return;
        // Sadece platformlarla etkileþime geç
        if ((collision.gameObject.CompareTag("RegularPlatform") ||
             collision.gameObject.CompareTag("MovingPlatform") ||
             collision.gameObject.CompareTag("Platform") ||
             collision.gameObject.CompareTag("Ground")) && rb.velocity.y <= 0f)
        {
            Vector2 velocity = rb.velocity;
            // Ses çal
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }

            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                velocity.y = jumpForce;
            }
            else if (collision.gameObject.CompareTag("Platform"))
            {    
                velocity.y = jumpForce;
            }
            else 
            {
                velocity.y = jumpForce; 
            }

            rb.velocity = velocity;
        }
    }
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        
    }
    void ClampPlayerHorizontalPosition()
    {
        // Get the camera's world boundaries
        Vector3 leftBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 rightBoundary = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0));

        // Clamp the player's x position within the boundaries
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, leftBoundary.x, rightBoundary.x);
        transform.position = clampedPosition;
    }

    bool IsOutOfVerticalCameraView()
    {
        // Convert player's position to viewport coordinates
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the player is below the camera's view
        return viewportPosition.y < 0;
    }

    void ChangePlayerSkin() 
    { 
        Character character = GameDataManager.GetSelectedCharacter();
        if (character.image != null) 
        { 
            playerImage.sprite = character.image;
        }
    
    }
}

