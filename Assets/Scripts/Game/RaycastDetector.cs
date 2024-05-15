using UnityEngine;

namespace Game
{
    public class RaycastDetector
    {
        public ContactInfo RayCast(int layerMask)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, 1 << layerMask);

            return new ContactInfo
            {
                Contacted = isHit,
                Point = hit.point,
                Collider = hit.collider,
                Transform = hit.transform
            };
        }
    }
}