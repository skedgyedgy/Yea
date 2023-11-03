using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBounce : MonoBehaviour {
    [SerializeField]
    private Visualizer visualizer;
    [SerializeField]
    private float minScale = 1;
    [SerializeField]
    private float maxScale = 1.5f;

    private Vector3 initScale;

    private void Start () {
        initScale = transform.localScale;
    }

    private void Update () {
        transform.localScale = initScale * Mathf.Lerp (minScale, maxScale, visualizer.Loudness);
    }
}