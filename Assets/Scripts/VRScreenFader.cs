using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VRScreenFader : MonoBehaviour
{
    [Header("Fade Settings")]
    public Material fadeMaterial;
    public Color fadeColor = Color.black;
    public float fadeDuration = 1f;
    public bool fadeInOnStart = true;

    private MeshRenderer _renderer;
    private float _fadeAlpha;
    private float _targetAlpha;
    private float _fadeSpeed;
    private bool _isFading;

    void Start()
    {
        CreateFaderScreen();

        if (fadeInOnStart)
        {
            _fadeAlpha = 1f;
            FadeIn();
        }
    }

    void CreateFaderScreen()
    {
        var faderGO = new GameObject("Fader screen");
        faderGO.transform.SetParent(transform, false);
        faderGO.transform.localPosition = new Vector3(0f, 0f, 0.3f);
        faderGO.transform.localRotation = Quaternion.identity;
        faderGO.transform.localScale = Vector3.one;

        var meshFilter = faderGO.AddComponent<MeshFilter>();
        meshFilter.mesh = BuildQuadMesh();

        _renderer = faderGO.AddComponent<MeshRenderer>();

        if (fadeMaterial != null)
        {
            _renderer.material = new Material(fadeMaterial);
        }
        else
        {
            _renderer.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            _renderer.material.SetFloat("_Surface", 1); // transparent
            _renderer.material.SetFloat("_Blend", 0);   // alpha
            _renderer.material.SetFloat("_ZWrite", 0);
            _renderer.material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _renderer.material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _renderer.material.renderQueue = 3000;
        }

        _renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _renderer.receiveShadows = false;

        ApplyAlpha();
    }

    static Mesh BuildQuadMesh()
    {
        var mesh = new Mesh { name = "FaderQuad" };
        mesh.vertices = new[]
        {
            new Vector3(-1f, -1f, 0f),
            new Vector3( 1f, -1f, 0f),
            new Vector3( 1f,  1f, 0f),
            new Vector3(-1f,  1f, 0f)
        };
        mesh.uv = new[]
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f)
        };
        mesh.triangles = new[] { 0, 2, 1, 0, 3, 2 };
        mesh.RecalculateNormals();
        return mesh;
    }

    void Update()
    {
        if (!_isFading) return;

        _fadeAlpha = Mathf.MoveTowards(_fadeAlpha, _targetAlpha, _fadeSpeed * Time.deltaTime);
        ApplyAlpha();

        if (Mathf.Approximately(_fadeAlpha, _targetAlpha))
            _isFading = false;
    }

    void ApplyAlpha()
    {
        if (_renderer == null) return;
        var c = fadeColor;
        c.a = _fadeAlpha;
        _renderer.material.color = c;
        _renderer.enabled = _fadeAlpha > 0.001f;
    }

    public void FadeOut(float duration = -1f)
    {
        StartFade(1f, duration > 0f ? duration : fadeDuration);
    }

    public void FadeIn(float duration = -1f)
    {
        StartFade(0f, duration > 0f ? duration : fadeDuration);
    }

    void StartFade(float target, float duration)
    {
        _targetAlpha = target;
        _fadeSpeed = 1f / Mathf.Max(duration, 0.001f);
        _isFading = true;
    }
}
