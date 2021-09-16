using System;
using UnityEngine;

namespace Neat.Tweening {
    public enum MoverType {
        PingPong = 0,
        Repeat = 1
    }

    public abstract class Mover<T> : MonoBehaviour
        where T : IEquatable<T> {
        public float speed = 1;

        public MoverType moverType = MoverType.PingPong;

        public bool deltaMode;

        public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public T min;
        public T max;

        [Range(0, 2)] public float offset;

        [SerializeField] private Accessor<T> accessor;

        private T initialValue;

        private bool init = false;

        private void Awake() {
            if (init) return;
            init = true;

            accessor.Init(gameObject);

            initialValue = accessor.Value;
        }

        private void Update() {
            if (!accessor.Valid) return;

            var it = Time.time * speed + offset;
            var t = moverType == MoverType.Repeat ? Mathf.Repeat(it, 1) : Mathf.PingPong(it, 1);

            var clamped = Mathf.Clamp01(t);
            var evaluated = animationCurve.Evaluate(clamped);
            var current = this.Lerp(min, max, evaluated);
            accessor.Value = deltaMode ? this.Add(initialValue, current) : current;
        }

        protected abstract T Lerp(T a, T b, float t);

        protected abstract T Add(T a, T b);
    }
}
