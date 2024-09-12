using UnityEngine;

namespace ImmersiveGames.ObjectManagers.DetectManagers
{
    public class DetectPlayerApproach
    {
        private readonly Vector3 _positionReference;
        private readonly float _detectDistance;
        public DetectPlayerApproach(Vector3 refPosition, float patrolDistance)
        {
            _positionReference = refPosition;
            _detectDistance = patrolDistance;
        }

        
        public Transform TargetApproach<T>(LayerMask targetLayer, int maxColliders = 3)
        {
            var results = new Collider[maxColliders];
            var size = Physics.OverlapSphereNonAlloc(_positionReference, _detectDistance, results, targetLayer);
            //Debug.Log("Size:" + size +" Referencia: "+_positionReference+" Distancia: "+_detectDistance);
            if (size < 1) return null;

            for (var i = 0; i < size; i++)
            {
                //Debug.Log("COLLIDES: "+ results[i] +" Referencia: "+_positionReference+" Distancia: "+_detectDistance);
                var master = results[i].GetComponentInParent<T>();
                if (master == null) continue;
                return results[i].transform.root;
            }
            return null;
        }
    }
}