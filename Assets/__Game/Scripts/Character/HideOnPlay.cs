using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShowHideSymnol(false);
    }

    public void ShowHideSymnol(bool isShow)
    {
        // Show or hide the symbol based on the isShow parameter
        gameObject.SetActive(isShow);
    }
}
