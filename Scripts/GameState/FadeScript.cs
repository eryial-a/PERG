using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    // fade color
    public Color fadeColor = Color.black;
    public UnityEngine.UI.Image fadeImage;
    // instances
    private static SceneFade _instance;
    public static SceneFade Instance
    {
        get
        {
            if (_instance == null || _instance.Equals(null))
            {
                _instance = FindFirstObjectByType<SceneFade>();
                // if instance is null log and ignore
                if (_instance == null)
                {
                    Debug.LogWarning("SceneFade missing — creating fallback.");
                    GameObject obj = new GameObject("SceneFade");
                    _instance = obj.AddComponent<SceneFade>();
                    DontDestroyOnLoad(obj);
                }
            }
            // return instance
            return _instance;
        }
    }
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;
    // used to check for transitions
    private bool isTransitioning = false;
    // awake
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // duplicate instance
        _instance = this;
        DontDestroyOnLoad(gameObject);
        // search
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        fadeImage = GetComponentInChildren<UnityEngine.UI.Image>();
        // check for nulls in values of fade scene
        if (canvasGroup == null || fadeImage == null)
        {
            Debug.LogError("SceneFade prefab is missing CanvasGroup or Image!");
        }
    }
    // start
    void Start()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponentInChildren<CanvasGroup>();

        canvasGroup.alpha = 1f; // force black screen first
        // fade out
        StartCoroutine(FadeOut());
    }
    // load scene
    public void LoadScene(string sceneName)
    {
        if (isTransitioning) return;
        // check for null
        if (canvasGroup == null)
        {
            Debug.LogWarning("SceneFade invalid — loading without fade.");
            SceneManager.LoadScene(sceneName);
            return;
        }
        // set fade color
        if (fadeImage != null)
        {
            fadeImage.color = (sceneName == "GameOver")
                ? new Color(0.5f, 0f, 0f, 1f)
                : Color.black;
        }
        // prepare for transition
        isTransitioning = true;
        StopAllCoroutines();
        StartCoroutine(FadeAndLoad(sceneName));
    }
    // loads scene then fades
    IEnumerator FadeAndLoad(string sceneName)
    {
        Debug.Log("FadeAndLoad started");
        // fade in
        yield return StartCoroutine(FadeIn());
        // loads
        SceneManager.LoadScene(sceneName);
        yield return null;
        // start fade out
        yield return StartCoroutine(FadeOut());
        // if image is null set to black
        if (fadeImage != null)
            fadeImage.color = Color.black;
        // set to false for fade in
        isTransitioning = false;
    }
    // fade in
    IEnumerator FadeIn()
    {
        float t = 0f;
        // during duration increase alpha
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
    }
    // fades out
    IEnumerator FadeOut()
    {
        Debug.Log("FadeOut started");
        // duration to decrease alpha
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        // log
        Debug.Log("FadeOut finished");
    }
    }