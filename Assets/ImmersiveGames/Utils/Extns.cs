using System.Collections;
using UnityEngine;
namespace ImmersiveGames.Utils
{
    public static class Extns
    {
        public static IEnumerator Tweeng( this float duration,
            System.Action<float> var, float aa, float zz )
        {
            var sT = Time.time;
            var eT = sT + duration;
        
            while (Time.time < eT)
            {
                var t = (Time.time-sT)/duration;
                var( Mathf.SmoothStep(aa, zz, t) );
                yield return null;
            }
        
            var(zz);
        }

        public static IEnumerator Tweeng( this float duration,
            System.Action<Vector3> var, Vector3 aa, Vector3 zz )
        {
            var sT = Time.time;
            var eT = sT + duration;
        
            while (Time.time < eT)
            {
                var t = (Time.time-sT)/duration;
                var( Vector3.Lerp(aa, zz, Mathf.SmoothStep(0f, 1f, t) ) );
                yield return null;
            }
        
            var(zz);
        }
    }
}
