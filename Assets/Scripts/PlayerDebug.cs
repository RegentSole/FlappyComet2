using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[TRIGGER ENTER] Object='{other.name}', " +
                  $"Tag='{other.tag}', Layer='{LayerMask.LayerToName(other.gameObject.layer)}', " +
                  $"IsTrigger={other.isTrigger}", other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[COLLISION ENTER] Object='{collision.collider.name}', " +
                  $"Tag='{collision.collider.tag}', " +
                  $"Layer='{LayerMask.LayerToName(collision.collider.gameObject.layer)}'");
    }
}