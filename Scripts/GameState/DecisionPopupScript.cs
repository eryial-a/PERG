using TMPro;
using UnityEngine;
using System.Collections;

public class DecisionPopupUI : MonoBehaviour
{
    public static DecisionPopupUI Instance;
    // canvas group
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI popupText;
    // awake
    private void Awake()
    {
        Instance = this;
        // set to inactive before decision is made
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
    // popup appears
    public IEnumerator ShowPopup(string message, float duration = 1.5f)
    {
        gameObject.SetActive(true);
        // message
        popupText.text = message;
        // fade in
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(duration);
        // fade out
        canvasGroup.alpha = 0;
        // deactives game object
        gameObject.SetActive(false);
    }
}