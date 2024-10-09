using UnityEngine;

namespace ImmersiveGames.ObjectManagers.DetectManagers
{
    public class DetectPlayerApproach
    {
        private readonly float _detectDistance;

        public DetectPlayerApproach(float patrolDistance)
        {
            _detectDistance = patrolDistance;
        }
        
        // Atualizamos o método TargetApproach para usar a posição da mina dinamicamente
        public Transform TargetApproach<T>(Vector3 currentPosition, LayerMask targetLayer, int maxColliders = 3)
        {
            var results = new Collider[maxColliders];
            var size = Physics.OverlapSphereNonAlloc(currentPosition, _detectDistance, results, targetLayer);
            //Debug.Log("Size:" + size + " Posição atual: " + currentPosition + " Distância: " + _detectDistance);
            
            if (size < 1) return null;

            for (var i = 0; i < size; i++)
            {
                var master = results[i].GetComponentInParent<T>();
                if (master == null) continue;
                return results[i].transform.root;
            }
            return null;
        }
    }
}