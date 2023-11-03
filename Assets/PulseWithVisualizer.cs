using UnityEngine;

public class PulseWithVizualizer : MonoBehaviour {
    [SerializeField]
    private Visualizer visualizer;
    [SerializeField]
    private float minScale = 1;
    [SerializeField]
    private float maxScale = 1.3f;
    [SerializeField]
    private float speed = 4;

    private Vector3 initScale;

    private void Start () {
        initScale = transform.localScale;
    }

    private void Update () {
        transform.localScale = initScale * Mathf.Lerp (maxScale, minScale, (Time.timeSinceLevelLoad - AltVisualizer.FlashColorTime) * speed);
    }
}