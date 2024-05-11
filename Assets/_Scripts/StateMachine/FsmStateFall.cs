using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM._Scripts
{
    public class FsmStateFall : FsmState
    {
        private TestOne _playerControl;
        private Transform _transform;
        private RowManager _rowManager;
        public FsmStateFall(Fsm fsm, TestOne playerControl, RowManager rowManager) : base(fsm)
        {
            _playerControl = playerControl;
            _transform = playerControl.transform;
            _rowManager = rowManager;
        }

        public override void Enter()
        {
            Debug.Log("Fall state Enter");
            _playerControl.StartCoroutine(FallDown(_transform.position, _transform.position + Vector3.down * 4));
        }

        public override void Exit()
        {
            Debug.Log("Move state Exit");
        }

        IEnumerator FallDown(Vector3 fromPosition, Vector3 toPosition)
        {
            float duration = 1f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                float curvedT = t * t;

                Vector3 newPosition = Vector3.Lerp(fromPosition, toPosition, curvedT);

                _transform.position = newPosition;

                yield return null;
            }

            _transform.position = toPosition;
            _playerControl.GameStateChangedEvent?.Invoke(WinLoseMenu.GameState.Defeat);
        }
    }
}