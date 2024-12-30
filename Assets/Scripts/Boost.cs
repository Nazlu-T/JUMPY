using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] private float boostForce = 10f; // Yukar� do�ru uygulanacak kuvvet
    [SerializeField] private float effectDuration = 3f; // Etkinin s�resi (saniye)
    [SerializeField] private AudioClip cherrySound; // Kiraz �arp��ma sesi (iste�e ba�l�)
    private bool isCollected = false; // Power-up'�n bir kez kullan�lmas�n� sa�lamak i�in

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return; // E�er kiraz zaten topland�ysa bir �ey yapma

        if (collision.CompareTag("Player")) // Oyuncuyla �arp��ma kontrol�
        {
            isCollected = true; // Kiraz�n topland���n� i�aretle

            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // Mevcut d���� h�z�n� s�f�rla
                playerRb.AddForce(Vector2.up * boostForce, ForceMode2D.Impulse); // Yukar� kuvvet uygula
                StartCoroutine(ResetPlayerAfterDuration(playerRb)); // Belirli s�re sonra oyuncuyu s�f�rla
            }

            if (cherrySound != null)
            {
                AudioSource.PlayClipAtPoint(cherrySound, transform.position); // Ses �al
            }

            GetComponent<SpriteRenderer>().enabled = false; // Kiraz g�rselini gizle
            GetComponent<Collider2D>().enabled = false; // �arp��may� devre d��� b�rak
        }
    }

    private System.Collections.IEnumerator ResetPlayerAfterDuration(Rigidbody2D playerRb)
    {
        yield return new WaitForSeconds(effectDuration); // Belirtilen s�reyi bekle
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0); // Oyuncuyu eski duruma d�nd�r
    }
}
