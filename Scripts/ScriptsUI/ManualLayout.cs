using System.Collections.Generic;
using UnityEngine;

public class HandLayout : MonoBehaviour
{
    public float spacing = 130f;
    public float selectedLift = 40f;
    // arranges cards
    public void Arrange(List<CardUI> cards)
    {
        int count = cards.Count;
        float center = (count - 1) / 2f;
        // for loop for all unaccounted for cards in hand
        for (int i = 0; i < count; i++)
        {
            RectTransform rt = cards[i].GetComponent<RectTransform>();
            // is selected (will change verticality)
            float x = (i - center) * spacing;
            float y = cards[i].IsSelected() ? selectedLift : 0f;
            // anchors at spot
            rt.anchoredPosition = new Vector2(x, y);
            rt.localRotation = Quaternion.identity;
        }
    }
}