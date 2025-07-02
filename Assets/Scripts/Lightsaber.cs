using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    private const int NUM_VERTICES = 12;

    [SerializeField] private GameObject _tip;
    [SerializeField] private GameObject _base;
    [SerializeField] private GameObject _meshParent;
    [SerializeField] private int _trailFrameLength = 3;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;

    void Start()
    {
        _mesh = new Mesh();
        _mesh.name = "TrailMesh";
        _meshParent.GetComponent<MeshFilter>().mesh = _mesh;

        _vertices = new Vector3[_trailFrameLength * NUM_VERTICES];
        _triangles = new int[_vertices.Length];

        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;

        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);
    }

    void LateUpdate()
    {
        if (_frameCount == (_trailFrameLength * NUM_VERTICES))
        {
            _frameCount = 0;
        }

        Vector3 localTip = _meshParent.transform.InverseTransformPoint(_tip.transform.position);
        Vector3 localBase = _meshParent.transform.InverseTransformPoint(_base.transform.position);
        Vector3 localPrevTip = _meshParent.transform.InverseTransformPoint(_previousTipPosition);
        Vector3 localPrevBase = _meshParent.transform.InverseTransformPoint(_previousBasePosition);

        _vertices[_frameCount] = localBase;
        _vertices[_frameCount + 1] = localTip;
        _vertices[_frameCount + 2] = localPrevTip;
        _vertices[_frameCount + 3] = localBase;
        _vertices[_frameCount + 4] = localPrevTip;
        _vertices[_frameCount + 5] = localTip;
        _vertices[_frameCount + 6] = localPrevTip;
        _vertices[_frameCount + 7] = localBase;
        _vertices[_frameCount + 8] = localPrevBase;
        _vertices[_frameCount + 9] = localPrevTip;
        _vertices[_frameCount + 10] = localPrevBase;
        _vertices[_frameCount + 11] = localBase;

        for (int i = 0; i < NUM_VERTICES; i++)
        {
            _triangles[_frameCount + i] = _frameCount + i;
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 100f);

        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        _frameCount += NUM_VERTICES;
    }
}