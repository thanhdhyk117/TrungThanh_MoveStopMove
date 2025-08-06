using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Character owner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Consts.TAG_CHARACTER))
        {
            var triggerCharacter = Cache.GetCharacter(other);

            if (triggerCharacter != owner)
                owner.AddCharacterTarget(triggerCharacter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Consts.TAG_CHARACTER))
        {
            var triggerCharacter = Cache.GetCharacter(other);
            if (triggerCharacter != owner)
                owner.RemoveCharacter(triggerCharacter);
        }
    }

}
