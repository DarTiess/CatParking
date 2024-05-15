using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Infrastructure.Level.EventsBus.Signals
{
    public struct ParkLinkedToLine
    {
        private Route _currentRoute;
        private List<Vector3> _currentLinePoints;

        public Route Route => _currentRoute;
        public List<Vector3> LinePoints => _currentLinePoints;

        public ParkLinkedToLine(Route currentRoute, List<Vector3> currentLinePoints)
        {
            _currentRoute = currentRoute;
            _currentLinePoints = currentLinePoints;
        }
    }
}