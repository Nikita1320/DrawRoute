using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private CanvasRenderer canvasRenderer;
    [SerializeField] private Material drawMaterial;
    [SerializeField] private List<Vector2> points = new();
    [SerializeField] private RectTransform drawingObject;
    [SerializeField] private float lineThickness = 5;
    [SerializeField] private float minDistance = 5;

    private Coroutine drawCoroutine;
    private Mesh mesh;
    private Vector3 lastMousePosition;
    private float lengthLine = 0;

    public List<Vector2> Points { get => points; }
    public float LengthLine
    {
        get
        {
            if (lengthLine == 0)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    lengthLine += Vector2.Distance(points[i], points[i + 1]);
                }
            }
            return lengthLine;
        }
    }

    public void StartDrawing()
    {
        // Mouse Pressed
        mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = GetRectTransformMousePosition();
        vertices[1] = GetRectTransformMousePosition();
        vertices[2] = GetRectTransformMousePosition();
        vertices[3] = GetRectTransformMousePosition();

        uv[0] = Vector2.zero;
        uv[1] = Vector2.zero;
        uv[2] = Vector2.zero;
        uv[3] = Vector2.zero;

        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;

        triangles[3] = 1;
        triangles[4] = 3;
        triangles[5] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.MarkDynamic();

        canvasRenderer.SetMesh(mesh);
        canvasRenderer.SetMaterial(drawMaterial, null);

        lastMousePosition = GetRectTransformMousePosition();

        drawCoroutine = StartCoroutine(DrawingCoroutine());
    }
    public void StopDrawing()
    {
        if (drawCoroutine != null)
        {
            StopCoroutine(drawCoroutine);
        }
    }
    public void ClearLine()
    {
        canvasRenderer.SetMesh(null);
    }

    private IEnumerator DrawingCoroutine()
    {
        points.Clear();
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Draw();
        }
    }
    private void Draw()
    {
        // Mouse held down
        Vector3 mousePosition = GetRectTransformMousePosition();
        if (Vector3.Distance(mousePosition, lastMousePosition) > minDistance)
        {
            points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            Vector3[] vertices = new Vector3[mesh.vertices.Length + 2];
            Vector2[] uv = new Vector2[mesh.uv.Length + 2];
            int[] triangles = new int[mesh.triangles.Length + 6];

            mesh.vertices.CopyTo(vertices, 0);
            mesh.uv.CopyTo(uv, 0);
            mesh.triangles.CopyTo(triangles, 0);

            int vIndex = vertices.Length - 4;
            int vIndex0 = vIndex + 0;
            int vIndex1 = vIndex + 1;
            int vIndex2 = vIndex + 2;
            int vIndex3 = vIndex + 3;

            Vector3 mouseForwardVector = (mousePosition - lastMousePosition).normalized;
            Vector3 normal2D = new Vector3(0, 0, -1f);

            Vector3 newVertexUp = mousePosition + Vector3.Cross(mouseForwardVector, normal2D) * lineThickness;
            Vector3 newVertexDown = mousePosition + Vector3.Cross(mouseForwardVector, normal2D * -1f) * lineThickness;

            vertices[vIndex2] = newVertexUp;
            vertices[vIndex3] = newVertexDown;

            uv[vIndex2] = Vector2.zero;
            uv[vIndex3] = Vector2.zero;

            int tIndex = triangles.Length - 6;

            triangles[tIndex + 0] = vIndex0;
            triangles[tIndex + 1] = vIndex2;
            triangles[tIndex + 2] = vIndex1;

            triangles[tIndex + 3] = vIndex1;
            triangles[tIndex + 4] = vIndex2;
            triangles[tIndex + 5] = vIndex3;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            lastMousePosition = mousePosition;
            canvasRenderer.SetMesh(mesh);
        }
    }
    public Vector3 GetRectTransformMousePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingObject, Input.mousePosition, Camera.main, out Vector2 vec);
        return new Vector3(vec.x, vec.y, 0);
    }
    public void Reset()
    {
        canvasRenderer.SetMesh(null);
        points.Clear();
    }
}
