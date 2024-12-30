using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private float boostForce = 10f; // Yukarý doðru uygulanacak kuvvet
    [SerializeField] private float effectDuration = 3f; // Etkinin süresi (saniye)
    [SerializeField] private AudioClip cherrySound; // Kiraz çarpýþma sesi (isteðe baðlý)
    private bool isCollected = false; // Power-up'ýn bir kez kullanýlmasýný saðlamak için

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return; // Eðer kiraz zaten toplandýysa bir þey yapma

        if (collision.CompareTag("Player")) // Oyuncuyla çarpýþma kontrolü
        {
            isCollected = true; // Kirazýn toplandýðýný iþaretle

            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // Mevcut düþüþ hýzýný sýfýrla
                playerRb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse); // Yukarý kuvvet uygula
                StartCoroutine(ResetPlayerAfterDuration(playerRb)); // Belirli süre sonra oyuncuyu sýfýrla
            }

            if (cherrySound != null)
            {
                AudioSource.PlayClipAtPoint(cherrySound, transform.position); // Ses çal
            }

            GetComponent<SpriteRenderer>().enabled = false; // Kiraz görselini gizle
            GetComponent<Collider2D>().enabled = false; // Çarpýþmayý devre dýþý býrak
        }
    }

    private System.Collections.IEnumerator ResetPlayerAfterDuration(Rigidbody2D playerRb)
    {
        yield return new WaitForSeconds(effectDuration); // Belirtilen süreyi bekle
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // Oyuncuyu eski duruma döndür
    }
}
