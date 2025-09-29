using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour
{
    [SerializeField] Image[] keys;

    void OnEnable()
    {
        Collectible_Item.AddKey += LightKey;
        DoorManager.OpenDoor += DarkKeys;
    }

    void OnDisable()
    {
        Collectible_Item.AddKey -= LightKey;
        DoorManager.OpenDoor -= DarkKeys;
    }

    private void LightKey()
    {
        foreach (var key in keys)
        {
            if (key.color != Color.white)
            {
                key.color = Color.white;
                return;
            }
        }
    }

    private void DarkKeys()
    {
        foreach (var key in keys)
        {
            key.color = Color.black;
        }
    }
}
