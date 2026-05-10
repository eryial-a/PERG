using UnityEngine;

public class RGBCycler : MonoBehaviour
{
    Renderer targetRenderer;
    float timer = 0f;
    int channel = 0;
    float value = 0f;
    bool increasing = true;

    void Start()
    {
        GameObject obj = GameObject.Find("Test Triangle");
        if (obj != null)
        {
            targetRenderer = obj.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("TestTriangle not found in scene!");
        }
    }

    void Update()
    {
        if (targetRenderer == null) return;

        timer += Time.deltaTime * 100f;

        if (timer >= 1f)
        {
            timer = 0f;

            value += increasing ? 0.01f : -0.01f;

            if (value >= 1f)
            {
                value = 1f;
                increasing = false;
                channel = (channel + 1) % 3;
            }
            else if (value <= 0f)
            {
                value = 0f;
                increasing = true;
                channel = (channel + 1) % 3;
            }

            Color c = targetRenderer.material.color;

            if (channel == 0) c.r = value;
            if (channel == 1) c.g = value;
            if (channel == 2) c.b = value;

            targetRenderer.material.color = c;
        }
    }
}