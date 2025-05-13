using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PentagonDrawer : MonoBehaviour
{
    public float[] radius;
    public float width;
    public Color color;
    public Canvas canvas;
    private int vertexCount;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        vertexCount = radius.Length;
        lineRenderer.positionCount = vertexCount + 1;
        DrawPentagon();
    }

    public void DrawPentagon()
    {
        Vector3[] positions = new Vector3[vertexCount + 1];

        for(int i=0; i<vertexCount; i++)
        {
            float angle = (i * Mathf.PI * 2f) / vertexCount;
            float x = Mathf.Cos(angle + Mathf.PI / 2f) * radius[i];
            float y = Mathf.Sin(angle + Mathf.PI / 2f) * radius[i];
            Vector2 point = new Vector2(x,y);
            positions[i] = convertWorldToCanvas(point, canvas);
        }
        positions[vertexCount] = positions[0];

        lineRenderer.SetPositions(positions);
    } 

    private Vector3 convertWorldToCanvas(Vector2 pos, Canvas _canvas) 
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform> ();
        pos.x += canvasRect.transform.position.x;
        pos.y += canvasRect.transform.position.y;
        return new Vector3(pos.x, pos.y, canvasRect.transform.position.z + 10.0f);
    }
}
