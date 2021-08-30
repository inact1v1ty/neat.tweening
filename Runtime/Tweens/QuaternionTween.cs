using UnityEngine;


namespace Neat.Tweening.Tweens {
    [AddComponentMenu("Neat/Tweens/Quaternion Tween")]
    public class QuaternionTween : Tween<Quaternion> {
        protected override Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.Slerp(a, b, t);

        protected override float Distance(Quaternion a, Quaternion b) => Quaternion.Angle(a, b);

        protected override Quaternion Add(Quaternion a, Quaternion b) => a * b;
        protected override Quaternion Sub(Quaternion a, Quaternion b) => a * Quaternion.Inverse(b);
    }
}
