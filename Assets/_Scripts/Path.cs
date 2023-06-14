using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    // we initialize this in the inspector
    [SerializeField]
    private Transform[] _nodes;

    // and use this for calculations
    public Vector3[] NodesPos { get; set; }
    public float[] Distances { get; set; }

    private float _maxDist;

    private void Start()
    {
        NodesPos = new Vector3[_nodes.Length];
        for (int i = 0; i < _nodes.Length; i++)
        {
            NodesPos[i] = _nodes[i].position;
        }
    }
    // Draws the path in the scene view
    private void Update()
    {
        for (int i = 0; i < _nodes.Length - 1; i++)
            Debug.DrawLine(NodesPos[i], NodesPos[i + 1], Color.cyan, 0.0f, false);

        CalcDistances();
    }


    // Loops through the path's nodes and determines how far each node in
    // the path is from the starting node
    public void CalcDistances()
    {
        Distances = new float[NodesPos.Length];
        Distances[0] = 0;

        for (int i = 0; i < NodesPos.Length - 1; i++)
            Distances[i + 1] = Distances[i] + Vector3.Distance(NodesPos[i], NodesPos[i + 1]);

        _maxDist = Distances[Distances.Length - 1];
    }


    public float GetParam(Vector3 position)
    {
        int closestSegment = GetClosestSegment(position);

        float param = Distances[closestSegment] + 
            GetParamForSegment(position, NodesPos[closestSegment], NodesPos[closestSegment + 1]);

        return param;
    }

    public int GetClosestSegment(Vector3 position)
    {
        // find the first point in the closest line segment to the path
        float closestDist = DistToSegment(position, NodesPos[0], NodesPos[1]);
        int closestSegment = 0;

        for (int i = 1; i < NodesPos.Length - 1; i++)
        {
            float dist = DistToSegment(position, NodesPos[i], NodesPos[i + 1]);

            if (dist <= closestDist)
            {
                closestDist = dist;
                closestSegment = i;
            }
        }

        return closestSegment;
    }

    // Given a param it gets the position on the path
    public Vector3 GetPosition(float param, bool pathLoop = false)
    {
        // Make sure the param is not past the beginning or end of the path
        if (param < 0)
            param = (pathLoop) ? param + _maxDist : 0;
        else if (param > _maxDist)
            param = (pathLoop) ? param - _maxDist : _maxDist;

        // Find the first node that is farther than given param
        int i = 0;
        for (; i < Distances.Length; i++)
            if (Distances[i] > param)
                break;

        // Convert it to the first node of the line segment that the param is in
        if (i > Distances.Length - 2)
            i = Distances.Length - 2;
        else
            i -= 1;

        // Get how far along the line segment the param is
        float t = (param - Distances[i]) / Vector3.Distance(NodesPos[i], NodesPos[i + 1]);

        // Get the position of the param
        return Vector3.Lerp(NodesPos[i], NodesPos[i + 1], t);
    }

    // Gives the distance of a point to a line segment
    // p is the point, v and w are the two points of the line segment
    float DistToSegment(Vector3 p, Vector3 v, Vector3 w)
    {
        Vector3 vw = w - v;

        float l2 = Vector3.Dot(vw, vw);
        if (l2 == 0)
            return Vector3.Distance(p, v);

        float t = Vector3.Dot(p - v, vw) / l2;
        if (t < 0)
            return Vector3.Distance(p, v);
        if (t > 1)
            return Vector3.Distance(p, w);

        Vector3 closestPoint = Vector3.Lerp(v, w, t);
        return Vector3.Distance(p, closestPoint);
    }

    // Finds the param for the closest point on the segment vw given the point p
    float GetParamForSegment(Vector3 p, Vector3 v, Vector3 w)
    {
        Vector3 vw = w - v;

        float l2 = Vector3.Dot(vw, vw);

        if (l2 == 0)
        {
            return 0;
        }

        float t = Vector3.Dot(p - v, vw) / l2;

        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }

        /* Multiple by (v - w).magnitude instead of Sqrt(l2) because we want the magnitude of the full 3D line segment */
        return t * (v - w).magnitude;
    }

    public void ReversePath()
    {
        System.Array.Reverse(NodesPos);
        CalcDistances();
    }
}
