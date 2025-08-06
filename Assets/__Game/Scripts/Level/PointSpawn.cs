using UnityEngine;

public class PointSpawn : MonoBehaviour
{
    [SerializeField] private Level level;
    public bool isHaveCharacter = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            isHaveCharacter = false;
            level.AddPoint(this);
        }
        else
        {
            var character = Cache.GetCharacter(other);
            if (character != null)
            {
                isHaveCharacter = true;

                level.RemovePoint(this);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            isHaveCharacter = false;
        }
        else
        {
            var character = Cache.GetCharacter(other);
            if (character != null)
            {
                isHaveCharacter = false;
            }
        }
    }
}
