using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] ParticleSystem _explosionParticlesPrefab;
    [SerializeField] ParticleSystem _waterParticlesPrefab;
    [SerializeField] AudioClip _explosionClip;

    void OnCollisionEnter2D(Collision2D collision)
    {
        bool isWater = collision.gameObject.CompareTag("Water");

        Instantiate(isWater ? _waterParticlesPrefab : _explosionParticlesPrefab, collision.GetContact(0).point, Quaternion.identity);

        if (isWater)
            Destroy(gameObject);
        else
            StartCoroutine(PlaySoundAndDie());
    }

    IEnumerator PlaySoundAndDie()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionClip);

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(gameObject);
    }
}
