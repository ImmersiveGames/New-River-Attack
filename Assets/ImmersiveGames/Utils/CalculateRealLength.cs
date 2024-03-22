using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public static class CalculateRealLength
    {
        // Método estático para calcular o comprimento real do objeto montado com base nos colliders
        public static float CalculateCollidersLength(IEnumerable<Collider> colliders)
        {
            var enumerable = colliders as Collider[] ?? colliders.ToArray();
            return !enumerable.Any() ? 0f : enumerable.Select(collider => collider.bounds.size.z).Prepend(0f).Max();
        }

        public static Bounds getBounds(GameObject objeto){
                Bounds bounds;
                Renderer childRender;
                bounds = getRenderBounds(objeto);
                if(bounds.extents.x == 0){
                    bounds = new Bounds(objeto.transform.position,Vector3.zero);
                    foreach (Transform child in objeto.transform) {
                        childRender = child.GetComponent<Renderer>();
                        if (childRender) {
                            bounds.Encapsulate(childRender.bounds);
                        }else{
                            bounds.Encapsulate(getBounds(child.gameObject));
                        }
                    }
                }
                return bounds;
            }

        static Bounds getRenderBounds(GameObject objeto){
            Bounds bounds = new  Bounds(Vector3.zero,Vector3.zero);
            Renderer render = objeto.GetComponent<Renderer>();
            if(render!=null){
                return render.bounds;
            }
            return bounds;
        }
    }
}