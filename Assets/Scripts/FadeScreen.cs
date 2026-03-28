using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float fadeDuration = 2f;
    public Color fadeColor = Color.black;
    public string colorPropertyName = "_BaseColor";

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        if (fadeOnStart) FadeIn();
    }

    public void FadeIn() => Fade(1f, 0f);
    public void FadeOut() => Fade(0f, 1f);

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        rend.enabled = true;
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            rend.material.SetColor(colorPropertyName, newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor(colorPropertyName, finalColor);

        if (alphaOut == 0f) rend.enabled = false;
    }
}
