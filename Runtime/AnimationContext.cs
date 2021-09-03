using System.Collections.Generic;
using UnityEngine;

namespace Neat.Tweening {
    public class AnimationContext : MonoBehaviour {
        public string animationName;

        private readonly List<Tween> tweens = new List<Tween>();
        private int currentState = -1;

        private void Reset() {
            animationName = "Transition";
        }

        public void Register(Tween tween) {
            tweens.Add(tween);
            if (currentState != -1) {
                tween.SetState(currentState);
            }
        }

        public void Unregister(Tween tween) {
            tweens.Remove(tween);
        }

        public void SetState(int state, bool forceInstant = false) {
            tweens.ForEach(tween => tween.SetState(state, forceInstant));
            currentState = state;
        }
    }
}
