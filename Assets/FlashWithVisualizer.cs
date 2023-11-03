using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class FlashWithVizualizer : MonoBehaviour {
    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private Visualizer visualizer;
    [SerializeField]
    private Color offColor;
    [SerializeField]
    private Color onColor;
    [SerializeField]
    private float speed = 4;

    private void Awake () {
        SpriteRenderer = GetComponent<SpriteRenderer> ();
    }

    private void Update () {
        SpriteRenderer.color = Color.Lerp (onColor, offColor, (Time.timeSinceLevelLoad - AltVisualizer.FlashColorTime) * speed);
    }
}