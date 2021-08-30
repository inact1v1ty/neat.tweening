using UnityEngine;


namespace Neat.Tweening.Tweens {
    [AddComponentMenu("Neat/Tweens/Vector4 Tween")]
    public class Vector4Tween : Tween<Vector4> {
        protected override Vector4 Lerp(Vector4 a, Vector4 b, float t) => Vector4.Lerp(a, b, t);

        protected override float Distance(Vector4 a, Vector4 b) => Vector4.Distance(a, b);

        protected override Vector4 Add(Vector4 a, Vector4 b) => a + b;
        protected override Vector4 Sub(Vector4 a, Vector4 b) => a - b;
    }
}
