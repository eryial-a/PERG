using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyAttackPopupUI : MonoBehaviour
{
    public GameObject popupRoot;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI damageText;
    // amount of time popup appears
    public float showTime = 0.9f;
    // start
    void Start()
    {
        popupRoot.SetActive(false);
    }
    // visually shows popup
    public void Show(int damage)
    {
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(ShowRoutine(damage));
    }
    IEnumerator ShowRoutine(int damage)
    {
        popupRoot.SetActive(true);
        // enemy attack text
        titleText.text = "ENEMY ATTACK";
        damageText.text = "-" + damage;
        yield return new WaitForSeconds(showTime);
        popupRoot.SetActive(false);
    }
}