using UnityEngine;

public class CollisionIgnore : MonoBehaviour
{
    public LayerMask hookableLayer;

    void Start()
    {
        // игнорируем столкновения между игроком и Hookable объектами
        Physics.IgnoreLayerCollision(
            gameObject.layer,
            LayerMask.NameToLayer("Hookable")
        );
    }
}
