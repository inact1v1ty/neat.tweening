using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Quaternion Mover")]
    public class QuaternionMover : Mover<Quaternion> {
        protected override Quaternion Lerp(Quaternion a, Quaternion b, float t) => Quaternion.Slerp(a, b, t);

        protected override Quaternion Add(Quaternion a, Quaternion b) => a * b;
    }
}
