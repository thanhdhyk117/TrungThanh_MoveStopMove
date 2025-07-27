using System.Collections.Generic;
using UnityEngine;

public class Cache
{
    private static Dictionary<Collider, Character> _characters = new Dictionary<Collider, Character>();

    public static Character GetCharacter(Collider collider)
    {
        if (!_characters.ContainsKey(collider))
        {
            _characters.Add(collider, collider.GetComponent<Character>());
        }

        return _characters[collider];
    }
}