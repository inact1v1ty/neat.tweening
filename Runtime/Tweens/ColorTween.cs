using UnityEngine;


namespace Neat.Tweening.Tweens {
    [AddComponentMenu("Neat/Tweens/Color Tween")]
    public class ColorTween : Tween<Color> {
        protected override Color Lerp(Color a, Color b, float t) {
            return Color.Lerp(a, b, t);
        }

        protected override float Distance(Color a, Color b) => Mathf.Max(
            Mathf.Abs(a.r - b.r),
            Mathf.Abs(a.g - b.g),
            Mathf.Abs(a.b - b.b),
            Mathf.Abs(a.a - b.a)
        );

        protected override Color Add(Color a, Color b) => a + b;
        protected override Color Sub(Color a, Color b) => a - b;
    }
}
