using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class AltVisualizer : Visualizer {
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private int bandCount = 64;
    [SerializeField]
    private FFTWindow fftWindow = FFTWindow.Rectangular;
    [SerializeField]
    private float heightMod = 1;
    [SerializeField]
    private float decaySpeed = 5;
    [SerializeField]
    private float attackSpeed = 20;
    [SerializeField]
    private float minHeight = 0.5f;
    [SerializeField]
    private float threshold = 0.2f;
    [SerializeField]
    private float flashSpeed = 2f;

    private SpriteRenderer baseSprite;
    private SpriteRenderer[] bands;
    // private float[] maxValues;

    private float bandTransformWidth;
    private float vizBarHeight;

    public static float FlashColorTime { get; private set; } = -1000;
    public void FlashColors () {
        FlashColorTime = Time.timeSinceLevelLoad;
    }

    private void Start () {
        baseSprite = GetComponent<SpriteRenderer> ();
        baseSprite.enabled = false;
        bandTransformWidth = baseSprite.size.x;
        vizBarHeight = baseSprite.size.y;

        spectrum = new float[bandCount];
        // maxValues = new float[bandCount - 1];
        bands = new SpriteRenderer[bandCount - 1];

        for (int i = 0; i < bandCount - 1; i++) {
            Transform newBand = new GameObject ("Band" + i, typeof (SpriteRenderer)).transform;
            newBand.SetParent (transform, false);
            newBand.localPosition = new (bandTransformWidth * i, 0);

            SpriteRenderer ren = newBand.GetComponent<SpriteRenderer> ();
            ren.size = new Vector2 (bandTransformWidth, minHeight);
            ren.sprite = baseSprite.sprite;
            ren.drawMode = baseSprite.drawMode;
            bands[i] = ren;
        }
    }

    float[] spectrum;
    private void Update () {
        AudioListener.GetSpectrumData (spectrum, 0, fftWindow);

        float total = 0;
        for (int i = 0; i < spectrum.Length - 1; i++) {
            float val = spectrum[i + 1];
            // float max = maxValues[i];
            // if (val > max) { maxValues[i] = val; max = val; }
            float oldHeight = bands[i].size.y;
            float temp = Mathf.Min (val * heightMod, 1);
            if (temp < threshold) temp = 0;
            float newHeight = Mathf.Lerp (oldHeight, temp * vizBarHeight, val > oldHeight ? Time.deltaTime * attackSpeed : Time.deltaTime * decaySpeed);
            bands[i].size = new (bandTransformWidth, Mathf.Max (minHeight, newHeight));
            bands[i].color = Color.HSVToRGB (Mathf.Repeat ((Time.timeSinceLevelLoad * 0.1f) + (float)i / spectrum.Length, 1),
                Mathf.Clamp ((Time.timeSinceLevelLoad - FlashColorTime) * flashSpeed, 0, 0.8f), 1);
            total += newHeight;
        }

        Loudness = total / (spectrum.Length - 1) / vizBarHeight;
    }
}
