using System;
using UnityEngine;

public class Collectible_Item : MonoBehaviour
{
    public static event Action AddKey;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(AudioManager.CodeSFX.key_collected);
            AddKey?.Invoke();
            Destroy(gameObject);
        }
    }
}
