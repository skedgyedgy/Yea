using UnityEngine;

public abstract class Visualizer : MonoBehaviour {
    public float Loudness { get; protected set; } = 0;
}

[RequireComponent (typeof (SpriteRenderer))]
public class FunnyVisualizer : Visualizer {
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

    private SpriteRenderer baseSprite;
    private Transform[] bands;
    private float[] maxValues;

    [SerializeField]
    private Sprite spriteOverride;

    private void Start () {
        baseSprite = GetComponent<SpriteRenderer> ();

        baseSprite.enabled = false;
        Texture2D tex = baseSprite.sprite.texture;
        Rect rect = baseSprite.sprite.rect;
        float bandRectWidth = (float)rect.width / (bandCount - 1);
        float bandTransformWidth = bandRectWidth / baseSprite.sprite.pixelsPerUnit;

        spectrum = new float[bandCount];
        maxValues = new float[bandCount - 1];
        bands = new Transform[bandCount - 1];

        for (int i = 0; i < bandCount - 1; i++) {
            Sprite newSprite = spriteOverride != null ? spriteOverride : Sprite.Create (tex, new Rect (rect.x + (bandRectWidth * i), rect.y, bandRectWidth, rect.height), Vector2.zero, baseSprite.sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            Transform newBand = new GameObject ("Band" + i, typeof (SpriteRenderer)).transform;
            newBand.SetParent (transform, false);
            newBand.localPosition = new (bandTransformWidth * i, 0);
            newBand.localScale = new (1, 1);
            newBand.GetComponent<SpriteRenderer> ().drawMode = baseSprite.drawMode;
            newBand.GetComponent<SpriteRenderer> ().sprite = newSprite;

            bands[i] = newBand;
        }
    }

    float[] spectrum;
    private void Update () {
        AudioListener.GetSpectrumData (spectrum, 0, fftWindow);

        float total = 0;
        for (int i = 0; i < spectrum.Length - 1; i++) {
            float val = spectrum[i + 1];
            float max = maxValues[i];
            if (val > max) { maxValues[i] = val; max = val; }
            float oldHeight = bands[i].localScale.y;
            float newHeight = Mathf.Lerp (oldHeight, Mathf.Min (val * heightMod / (max > 0 ? max : 1), 1), val > oldHeight ? Time.deltaTime * attackSpeed : Time.deltaTime * decaySpeed);
            bands[i].localScale = new (1, newHeight);
            total += newHeight;
        }

        Loudness = total / (spectrum.Length - 1);
    }
}
