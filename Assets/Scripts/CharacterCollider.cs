using System;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    private CharacterControl _character;

    private void Start()
    {
        _character = transform.parent.GetComponent<CharacterControl>();
        Debug.Log(_character.name);
    }

    // DamageCollider
    private void OnCollisionEnter2D(Collision2D other)
    {
        _character.OnCollisionEnterInChildren(other);
    }

    // ObjectCollider
    private void OnTriggerEnter2D(Collider2D other)
    {
        _character.OnTriggerEnterInChildren(other);
    }
}
