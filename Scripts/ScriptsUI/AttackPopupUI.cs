using UnityEngine;
using TMPro;
using System.Collections;

public class AttackPopupUI : MonoBehaviour
{
    public TextMeshProUGUI popupText;
    public CanvasGroup canvasGroup;
    // turn invisible apon start
    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    // shows popup 
    public void Show(string handName, float damage)
    {
        StopAllCoroutines();
        StartCoroutine(AnimatePopup(handName, damage));
    }
    // animates popup
    IEnumerator AnimatePopup(string handName, float damage)
    {   
        int Damagerounded = Mathf.RoundToInt(damage);
        // popup text
        popupText.text = handName + "\n" + Damagerounded + " DAMAGE";
        canvasGroup.alpha = 1f;
        // vector variables for animation
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 115f, 0);
        // duration
        float t = 0f;
        float duration = 1f;
        // Animation
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            // move upward
            transform.localPosition =
                Vector3.Lerp(startPos, endPos, lerp);
            // fade out
            canvasGroup.alpha =
                Mathf.Lerp(1f, 0f, lerp);
            // random shake rotation 
            float zRot = Mathf.Sin(Time.time * 10f) * 15f;
            transform.localRotation = Quaternion.Euler(0f, 0f, zRot);

            yield return null;
        }
        // rest to 0
        canvasGroup.alpha = 0f;
        // reset transform
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.identity;
    }
}