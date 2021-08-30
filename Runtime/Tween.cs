using System;
using UnityEngine;

namespace Neat.Tweening {
    public abstract class Tween : MonoBehaviour {
        [SerializeField] protected string animationName;

        public string AnimationName => animationName;
        public abstract void SetState(int state, bool forceInstant = false);
        public abstract void SetValue(object newValue, bool instant = false, float delay = 0f);

        public const int From = 0;
        public const int Normal = 1;
        public const int To = 2;
    }

    [Serializable]
    public struct TweenState<T>
        where T : IEquatable<T> {
        public T Value;
        public float Delay;
        public bool Instant;
    }

    public abstract class Tween<T> : Tween
        where T : IEquatable<T> {

        public float time = 1;

        public bool deltaMode;

        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [NamedArray(new[] {"From", "Normal", "To"}, "State")]
        public TweenState<T>[] states = new TweenState<T>[3];

        [SerializeField] private Accessor<T> accessor;

        private AnimationContext animationContext;
        private CommonDelayOffset commonDelayOffset;

        private bool init = false;

        private T initialValue;

        private T startValue;
        private T endValue;
        private float startTime = -100f;
        private float endTime = -100f;
        private bool done = true;

        private void Reset() {
            animationName = "Transition";
            states = new TweenState<T>[3];
            states[0].Instant = true;
        }

        private void Awake() {
            if (init) return;
            init = true;

            accessor.Init(gameObject);

            animationContext =
                transform.GetComponentInParentWhere<AnimationContext>(c => c.animationName == AnimationName);

            commonDelayOffset =
                transform.GetComponentInParentWhere<CommonDelayOffset>(c => c.animationName == AnimationName);

            initialValue = accessor.Value;

            if (animationContext != null) {
                animationContext.Register(this);
            }
        }

        private void Update() {
            if (!accessor.Valid || done) return;

            if (endTime - startTime < float.Epsilon) {
                done = true;

                return;
            }

            var t = (Time.time - startTime) / (endTime - startTime);
            if (t >= 1) {
                done = true;
            }

            var clamped = Mathf.Clamp01(t);
            var evaluated = animationCurve.Evaluate(clamped);
            var current = this.Lerp(startValue, endValue, evaluated);
            accessor.Value = deltaMode ? this.Add(initialValue, current) : current;
        }

        public void SetValue(T newValue, bool instant = false, float delay = 0f) {
            if (instant) {
                endValue = newValue;
                accessor.Value = deltaMode ? this.Add(initialValue, endValue) : endValue;

                done = true;
            }
            else {
                var distanceTo = (Math.Abs(endTime - (-100f)) < 1e-6 || done)
                    ? accessor.Value
                    : (deltaMode ? this.Add(endValue, initialValue) : endValue);
                var distance = Distance(distanceTo, newValue);

                if (distance < 1e-3) {
                    return;
                }

                startValue = deltaMode ? this.Sub(accessor.Value, initialValue) : accessor.Value;
                startTime = Time.time + delay;

                endValue = newValue;

                endTime = startTime + time;
                done = false;
            }
        }

        public override void SetState(int state, bool forceInstant = false) {
            if (state < 0 || state >= states.Length) {
                throw new ArgumentException(nameof(state));
            }

            var newState = states[state];
            var additionalDelay = commonDelayOffset != null ? commonDelayOffset.GetDelay(state) : 0;
            SetValue(newState.Value, forceInstant || newState.Instant, newState.Delay + additionalDelay);
        }

        public override void SetValue(object newValue, bool instant = false, float delay = 0f) {
            SetValue((T) newValue, instant, delay);
        }

        protected abstract T Lerp(T a, T b, float t);
        protected abstract float Distance(T a, T b);

        protected abstract T Add(T a, T b);
        protected abstract T Sub(T a, T b);
    }
}
