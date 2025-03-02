using UnityEngine;

public class SpectramLineRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float waveLength = 20.0f;
    [SerializeField] private float yLength = 10f;
    [SerializeField] private AudioSource source;

    private float[] spectram = null;
    private Vector3[] points = null;
    private const int FFT_RESOLUTION = 2048;

    private void Start()
    {
        this.spectram = new float[FFT_RESOLUTION];
        this.points = new Vector3[FFT_RESOLUTION];
    }

    public void FixedUpdate()
    {
        Render();
    }

    private void Render()
    {
        source.GetSpectrumData(spectram, 0, FFTWindow.BlackmanHarris);
        var xStart = 0;
        var xStep = waveLength / spectram.Length;
        for (var i = 0; i < points.Length; i++)
        {
            var y = spectram[i] * yLength;
            var x = xStart + xStep * i;
            var p = new Vector3(x, y, 0);
            points[i] = p;
        }

        Render(points);
    }

    private void Render(Vector3[] points)
    {
        if (points == null) return;
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private void Reset()
    {
        var x = 0;
        Render(new[]
        {
            new Vector3(-x, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(x, 0, 0),
        });
    }
}
