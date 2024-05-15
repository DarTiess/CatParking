using UnityEngine;

namespace Game.CatView
{
    public class CatAnimation
    {
        private readonly Animator _animator;
        private static readonly int WAIT = Animator.StringToHash("Wait");
        private static readonly int MOVE = Animator.StringToHash("Move");

        public CatAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void Wait()
        {
            _animator.SetBool(WAIT, true);
            _animator.SetBool(MOVE, false);
        }

        public void Move()
        {
            _animator.SetBool(MOVE, true);
            _animator.SetBool(WAIT, false);

        }

        public void Idle()
        {
            _animator.SetBool(MOVE, false);
        }
    }
}