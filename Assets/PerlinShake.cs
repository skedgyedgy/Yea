using UnityEngine;

public class PerlinShake : MonoBehaviour {
    [SerializeField]
    private Vector2 positionAmplitude = new (3, 1.8f);
    [SerializeField]
    private Vector2 positionFrequency = new (0.5f, 0.3f);
    [Space (4)]
    [SerializeField]
    private float rotationAmplitude = 10;
    [SerializeField]
    private float rotationFrequency = 0.3f;

    private float offset;
    private float Tick => Time.timeSinceLevelLoad + offset;

    private Vector3 initPos;
    private Vector3 initEul;
    private void Start () {
        offset = Random.Range (-1000f, 1000f);
        initPos = transform.localPosition;
        initEul = transform.localEulerAngles;
    }
    private void Update () {
        transform.localPosition = initPos + new Vector3 (((Mathf.PerlinNoise (Tick * positionFrequency.x, 0) * 2) - 1) * positionAmplitude.x,
            ((Mathf.PerlinNoise (0, Tick * positionFrequency.y) * 2) - 1) * positionAmplitude.y);
        transform.localEulerAngles = initEul + (((Mathf.PerlinNoise1D (Tick * -rotationFrequency) * 2) - 1) * rotationAmplitude * Vector3.forward);
    }
}