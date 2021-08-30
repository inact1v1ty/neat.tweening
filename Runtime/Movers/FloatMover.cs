using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Float Mover")]
    public class FloatMover : Mover<float> {
        protected override float Lerp(float a, float b, float t) => Mathf.Lerp(a, b, t);

        protected override float Add(float a, float b) => a + b;
    }
}
