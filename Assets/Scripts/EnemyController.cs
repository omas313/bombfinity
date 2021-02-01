using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour, IHealth
{
    public event Action<int> Destroyed;
    public event Action<int, int> HealthChanged;

    [SerializeField] int _maxHealth = 2;
    [SerializeField] Collider2D _gunsAreaCollider;
    [SerializeField] GameObject _gunPrefab;
    [SerializeField] Transform _gunsParent;
    [SerializeField] AudioClip _explosionSound;

    int _score => CalculateScore();

    int CalculateScore() => _guns.Sum(g => g.Score) + _maxHealth;

    Gun[] _guns;
    int _currentHealth;

    public void TakeDamage(int damage)
    {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
        // Debug.Log($"{name} taking damage");

        if (_currentHealth == 0)
            Die();
    }

    public void SetStats(int level)
    {
        _maxHealth = UnityEngine.Random.Range(1, level);
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        var gunsCount = level + (int)Mathf.Floor(UnityEngine.Random.Range(0f, level * 0.25f));
        for (int i = 0; i < gunsCount; i++)
        {
            var gun = Instantiate(_gunPrefab, GetRandomPositionInGunArea(), Quaternion.identity, _gunsParent).GetComponent<Gun>();
            gun.SetStats(level);
        }
        _guns = GetComponentsInChildren<Gun>();

    }

    Vector3 GetRandomPositionInGunArea() => new Vector3(
            UnityEngine.Random.Range(_gunsAreaCollider.bounds.min.x, _gunsAreaCollider.bounds.max.x),
            UnityEngine.Random.Range(_gunsAreaCollider.bounds.min.y, _gunsAreaCollider.bounds.max.y),
            0f);

    void Die()
    {
        ReleaseGuns();
        StartCoroutine(PlaySoundAndDie());
    }

    IEnumerator PlaySoundAndDie()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(_explosionSound);

        Destroyed?.Invoke(_score);
        yield return new WaitUntil(() => !audioSource.isPlaying);

        Destroy(gameObject);
    }

    void ReleaseGuns()
    {
        foreach (var gun in _guns)
            gun.Release();
    }

    void Awake()
    {
        _currentHealth = _maxHealth;    
        _guns = GetComponentsInChildren<Gun>();
    }
}
