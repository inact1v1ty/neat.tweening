using UnityEngine;

namespace Neat.Tweening {
    public class CommonDelayOffset : MonoBehaviour {
        public string animationName;

        [NamedArray(new[] {"From", "Normal", "To"}, "State")]
        public float[] delays;

        private CommonDelayOffset parent;


        private void Reset() {
            animationName = "Transition";
            delays = new float[3];
        }

        private void Awake() {
            var currentTransform = transform.parent;

            if (currentTransform == null) return;

            parent =
                currentTransform.GetComponentInParentWhere<CommonDelayOffset>(c => c.animationName == animationName);
        }

        public float GetDelay(int idx) {
            return (parent != null ? parent.GetDelay(idx) : 0) + (idx < delays.Length ? delays[idx] : 0);
        }
    }
}
