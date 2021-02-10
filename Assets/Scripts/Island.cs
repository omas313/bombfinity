using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour, IHealth
{
    public event Action<Island> Destroyed;

    public event Action<int, int> HealthChanged;
    public int KillScore => _maxHealth + GetComponentInChildren<Gun>().KillScore;

    [SerializeField] int _maxHealth = 3;
    [SerializeField] GameObject _gunPrefab;
    [SerializeField] Collider2D _gunArea;
    [SerializeField] ParticleSystem _destructionParticles;
    [SerializeField] Sprite[] _sprites;
    [SerializeField] AudioClip _explosionClip;

        
    int _currentHealth;

    public void SetStats(int level)
    {
        _currentHealth = _maxHealth = UnityEngine.Random.Range(1, level);
        SpawnGuns(level);
    }

    void SpawnGuns(int level)
    {
        int numGunsToSpawn = level < 3 ? 1 : UnityEngine.Random.Range(1, (int)(level * 0.5));
        for (int i = 0; i < numGunsToSpawn; i++)
            SpawnGun(level);
    }

    void SpawnGun(int level)
    {
        var position = new Vector2(
            UnityEngine.Random.Range(_gunArea.bounds.min.x, _gunArea.bounds.max.x),
            UnityEngine.Random.Range(_gunArea.bounds.min.y, _gunArea.bounds.max.y)
        );

        var gun = Instantiate(_gunPrefab, position, Quaternion.identity, _gunArea.transform).GetComponent<Gun>();
        gun.SetStats(level);
    }

    void Awake()
    {
        _currentHealth = _maxHealth;    
        SetRandomSprite();
    }

    void SetRandomSprite()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Length)];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        _currentHealth = Mathf.Max(_currentHealth - 1, 0);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
            return;
        }

        var guns = GetComponentsInChildren<Gun>();
        if (guns.Length > 1 && _maxHealth > 2 && _currentHealth <= _maxHealth / 2)
            DestroyOneGun(guns);
    }

    void DestroyOneGun(Gun[] guns) => guns[UnityEngine.Random.Range(0, guns.Length)].Release();

    void Die()
    {
        if (_destructionParticles != null)
            Instantiate(_destructionParticles, transform.position, Quaternion.identity);
        
        Destroyed?.Invoke(this);
        ReleaseGuns();
        StartCoroutine(PlaySoundAndDie());
    }

    IEnumerator PlaySoundAndDie()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);

        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionClip);

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(gameObject);
    }

    void ReleaseGuns()
    {
        foreach (var gun in GetComponentsInChildren<Gun>())
            gun.Release();
    }
}
