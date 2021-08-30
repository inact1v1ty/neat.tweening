using UnityEngine;

namespace Neat.Tweening.Movers {
    [AddComponentMenu("Neat/Movers/Color Mover")]
    public class ColorMover : Mover<Color> {
        protected override Color Lerp(Color a, Color b, float t) => Color.Lerp(a, b, t);

        protected override Color Add(Color a, Color b) => a + b;
    }
}
