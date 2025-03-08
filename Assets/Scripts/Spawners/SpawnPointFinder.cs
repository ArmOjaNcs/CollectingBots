using System.Collections.Generic;
using UnityEngine;

public class SpawnPointFinder
{
    private List<Vector3> _positionsInRing = new();
    private List<Vector3> _positionsInQuad = new();
    private float _possibleCount;

    public List<Vector3> FindPlaceWithRing(Vector3 originPoint, float originRadius, float agentDiameter, float spawnRadius)
    {
        _possibleCount = GameUtils.Two;
        _positionsInRing.Clear();
        Vector3 point = originPoint;
        float distanceOfRing = GameUtils.Two * GameUtils.Pi * spawnRadius;
        float angle = GameUtils.FullAngleInDegrees * Mathf.Deg2Rad;
        float minRadius = originRadius + agentDiameter;
        _possibleCount += distanceOfRing / (agentDiameter * GameUtils.Two);

        for (int i = 1; i <= _possibleCount; i++)
        {
            float z = originPoint.z + Mathf.Cos(angle / _possibleCount * i) * (spawnRadius);
            float x = originPoint.x + Mathf.Sin(angle / _possibleCount * i) * (spawnRadius);
            point.x = x;
            point.y = originPoint.y;
            point.z = z;
            _positionsInRing.Add(point);
        }

        return _positionsInRing;
    }

    public List<Vector3> FindPlaceWithQuad(float minXPosition, float minZPosition, float maxXPosition,
        float maxZPosition, float agentDiameter)
    {
        _positionsInQuad.Clear();
        Vector3 point = Vector3.zero;

        for (float x = minXPosition; x < maxXPosition;)
        {
            for (float z = minZPosition; z < maxZPosition;)
            {
                point.x = x;
                point.z = z;

                _positionsInQuad.Add(point);
                z += agentDiameter;
            }

            x += agentDiameter;
        }
        
        return _positionsInQuad;
    }
}