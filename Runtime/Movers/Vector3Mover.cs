using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Vector3 Mover")]
    public class Vector3Mover : Mover<Vector3> {
        protected override Vector3 Lerp(Vector3 a, Vector3 b, float t) => Vector3.Lerp(a, b, t);

        protected override Vector3 Add(Vector3 a, Vector3 b) => a + b;
    }
}
