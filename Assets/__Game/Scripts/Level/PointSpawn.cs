using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    [SerializeField] private Level level;
    public bool isHaveCharacter = false;
    private bool lastState = false;

    private void Update()
    {
        if (isHaveCharacter != lastState)
        {
            if (isHaveCharacter)
            {
                level.RemovePoint(this);
            }
            else
            {
                level.AddPoint(this);
            }

            lastState = isHaveCharacter;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            var character = Cache.GetCharacter(other);
            if (character != null)
            {
                isHaveCharacter = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            var character = Cache.GetCharacter(other);
            if (character != null)
            {
                isHaveCharacter = false;
            }
        }
    }
}
