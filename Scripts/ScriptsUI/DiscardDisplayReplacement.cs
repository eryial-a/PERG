using System.Collections.Generic;
using UnityEngine;

public class DiscardDisplay : MonoBehaviour
{
    public HandManager handManager;
    public GameObject cardPrefab;
    public Transform discardArea;
    public DynamicGridScaler scaler;
    public CardSpriteDatabase spriteDatabase;
    public void Show(List<Card> cards)
    {
        foreach (Transform child in discardArea)
            Destroy(child.gameObject);

        scaler.UpdateGrid(cards.Count);
        foreach (Card card in cards)
        {
            GameObject obj = Instantiate(cardPrefab, discardArea);
            Sprite sprite = spriteDatabase.GetSprite(card);
            obj.GetComponent<CardUI>().Setup(card, handManager, sprite);
        }
    }
}