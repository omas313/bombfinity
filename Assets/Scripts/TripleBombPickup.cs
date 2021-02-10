using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBombPickup : Pickup
{
    [SerializeField] GameObject _bombPrefab;
    [SerializeField] Vector2 _offsetFromMainBomb = new Vector2(5f, -2f);
    [SerializeField] float _duration = 5f;

    PlayerController _playerController;
    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }
    
    protected override void ActivateEffect()
    {
        StartCoroutine(TripleBomb());
    }

    IEnumerator TripleBomb()
    {
        _playerController.BombSpawned += OnBombSpawnedByPlayer;
        yield return new WaitForSeconds(5f);
        _playerController.BombSpawned -= OnBombSpawnedByPlayer;
        Destroy(gameObject);
    }

    void OnBombSpawnedByPlayer(Vector3 position)
    {
        Instantiate(_bombPrefab, (Vector2)position + _offsetFromMainBomb, Quaternion.identity);

        var offsetLeft = new Vector2(-_offsetFromMainBomb.x, _offsetFromMainBomb.y);
        Instantiate(_bombPrefab, (Vector2)position + offsetLeft, Quaternion.identity);
    }
}
