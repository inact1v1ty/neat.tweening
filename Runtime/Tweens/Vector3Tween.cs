using UnityEngine;


namespace Neat.Tweening.Tweens {
    [AddComponentMenu("Neat/Tweens/Vector3 Tween")]
    public class Vector3Tween : Tween<Vector3> {
        protected override Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);

        protected override float Distance(Vector3 a, Vector3 b) => Vector3.Distance(a, b);

        protected override Vector3 Add(Vector3 a, Vector3 b) => a + b;
        protected override Vector3 Sub(Vector3 a, Vector3 b) => a - b;
    }
}
