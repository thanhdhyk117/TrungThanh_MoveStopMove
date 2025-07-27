using UnityEngine;

public abstract class GameUnit : MonoBehaviour
{
    private Transform _transform;

    public Transform TF
    {
        get
        {
            if (_transform == null)
                _transform = transform;
            return _transform;
        }
    }

    public abstract void OnInit();
    public abstract void OnDespawn();
}