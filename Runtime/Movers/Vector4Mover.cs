using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Vector2 Mover")]
    public class Vector4Mover : Mover<Vector4> {
        protected override Vector4 Lerp(Vector4 a, Vector4 b, float t) => Vector4.Lerp(a, b, t);

        protected override Vector4 Add(Vector4 a, Vector4 b) => a + b;
    }
}
