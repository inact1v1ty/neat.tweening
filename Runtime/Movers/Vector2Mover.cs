using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Vector2 Mover")]
    public class Vector2Mover : Mover<Vector2> {
        protected override Vector2 Lerp(Vector2 a, Vector2 b, float t) => Vector2.Lerp(a, b, t);

        protected override Vector2 Add(Vector2 a, Vector2 b) => a + b;
    }
}
