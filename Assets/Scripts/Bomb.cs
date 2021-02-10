using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] ParticleSystem _explosionParticlesPrefab;
    [SerializeField] ParticleSystem _waterParticlesPrefab;
    [SerializeField] AudioClip _explosionClip;
    [SerializeField] AudioClip _waterHitClip;

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool isWater = collision.gameObject.CompareTag("Water");

        Instantiate(isWater ? _waterParticlesPrefab : _explosionParticlesPrefab, collision.GetContact(0).point, Quaternion.identity);

        StartCoroutine(PlaySoundAndDie(isWater));
    }

    IEnumerator PlaySoundAndDie(bool isWater)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(isWater ? _waterHitClip : _explosionClip);

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(gameObject);
    }
}
