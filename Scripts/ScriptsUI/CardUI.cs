using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Image cardImage;
    private Card card;
    private HandManager handManager;
    private RectTransform rt;
    public float selectedHeight = 40f;
    // awake
    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }
    // sets ups 
    public void Setup(Card newCard, HandManager manager, Sprite sprite)
    {
        if (rt == null)
            rt = GetComponent<RectTransform>();
        // grabs values
        card = newCard;
        handManager = manager;
        cardImage.sprite = sprite;
        // refreshes visuals
        RefreshVisual();
    }
    // on click
    public void OnClick()
    {
        if (handManager.selectedCards.Contains(card))
            handManager.UnselectCard(card);
        else
            handManager.SelectCard(card);

        handManager.handDisplay.RefreshHand();
    }
    // refreshes visuals
    public void RefreshVisual()
    {
        if (rt == null)
            return;
        // positioning
        Vector2 pos = rt.anchoredPosition;
        pos.y = IsSelected() ? selectedHeight : 0f;
        rt.anchoredPosition = pos;
    }
    // is selected
    public bool IsSelected()
    {
        return handManager != null &&
               handManager.selectedCards.Contains(card);
    }
}