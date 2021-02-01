using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] int _damage = 1;
    [SerializeField] ParticleSystem _explosionPrefab;
    [SerializeField] AudioClip _explosionSound;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_explosionPrefab != null)
            Instantiate(_explosionPrefab, collision.contacts[0].point, Quaternion.identity);

        var enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy != null)
            enemy.TakeDamage(_damage);

        StartCoroutine(PlaySoundAndDie());
    }

    IEnumerator PlaySoundAndDie()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated= false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionSound);

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(gameObject);
    }
}
