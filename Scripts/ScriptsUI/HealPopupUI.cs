using UnityEngine;
using TMPro;
using System.Collections;
// heal popup
public class HealPopupUI : MonoBehaviour
{
    public GameObject popupRoot;
    public TextMeshProUGUI healText;
    // Duration
    public float duration = 1.2f;
    // show
    public void Show(int amount)
    {
        if (popupRoot == null || healText == null)
        {
            Debug.LogWarning("HealPopupUI not assigned!");
            return;
        }
        // sets popup to active
        popupRoot.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(amount));
    }
    // While its active make it wiggle and float upward
    IEnumerator ShowRoutine(int amount)
    {
        healText.text = "Healed for: " + amount;
        // reset position & rotation
        RectTransform rect = popupRoot.GetComponent<RectTransform>();
        Vector3 startPos = rect.localPosition;
        rect.localRotation = Quaternion.identity;
        //
        float t = 0f;
        // Values for simple animation
        float floatDistance = 90f;
        float wiggleSpeed = 4f;
        float wiggleAmount = 6f;
        // while time is lest than duration
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            // floats upward
            float yOffset = Mathf.Lerp(0, floatDistance, progress);
            rect.localPosition = startPos + new Vector3(0, yOffset, 0);
            // wiggles panel
            float angle = Mathf.Sin(t * wiggleSpeed) * wiggleAmount;
            rect.localRotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }
        // unactive spopup
        popupRoot.SetActive(false);
        // resets position
        rect.localPosition = startPos;
        rect.localRotation = Quaternion.identity;
    }
}