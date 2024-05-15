﻿using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Line: MonoBehaviour
    {
        [SerializeField] private float minPointsDistance;

        public List<Vector3> Points => _points;
        public int PointsCount => _pointsCount;
        public float Lenght => _lenght;

        private LineRenderer _lineRenderer;
        private List<Vector3> _points = new List<Vector3>();
        private int _pointsCount = 0;
        private float _pointFixedYAxis;
        private float _lenght=0;
        private Vector3 _prevPoint;

        private void Start()
        {
            _pointFixedYAxis = _lineRenderer.GetPosition(0).y;
            Clear();
        }
        public void ShowLine()
        {
            gameObject.SetActive(true);
        }

        public void Clear()
        {
            gameObject.SetActive(false);
            _lineRenderer.positionCount = 0;
            _pointsCount = 0;
            _points.Clear();
            _lenght = 0;
        }

        public void AddPoints(Vector3 newPoint)
        {
            newPoint.y = _pointFixedYAxis;
            if (_pointsCount >= 1 
                && Vector3.Distance(newPoint, GetLastPoint()) < minPointsDistance)
                return;
            if (_pointsCount == 0)
                _prevPoint = newPoint;
            

            _points.Add(newPoint);
            _pointsCount++;

            _lenght += Vector3.Distance(_prevPoint, newPoint);
            _prevPoint = newPoint;

            _lineRenderer.positionCount = _pointsCount;
            _lineRenderer.SetPosition(_pointsCount-1, newPoint);

        }

        private Vector3 GetLastPoint()
        {
            return _lineRenderer.GetPosition(_pointsCount - 1);
        }

        private void OnValidate()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void SetColor(Color color)
        {
            _lineRenderer.sharedMaterials[0].color = color;
        }

        public void SetStartEndPosition(Vector3 startPosition, Vector3 endPosition)
        {
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
            
        }
    }
}