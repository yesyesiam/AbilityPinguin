using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM._Scripts
{
    public class FsmStateMove : FsmState
    {
        private TestOne _playerControl;
        private Transform _transform;
        private RowManager _rowManager;
        public FsmStateMove(Fsm fsm, TestOne playerControl, RowManager rowManager) : base(fsm) {
            _playerControl = playerControl;
            _transform = playerControl.transform;
            _rowManager = rowManager;
        }
        public override void Enter()
        {
            _playerControl.MoveToPoint(_transform, _rowManager.TargetPointPosition, _playerControl.ApplyMove);
            AudioManager.instance.PlaySoundEffectByIndex(2);
        }

        public override void Exit()
        {
            Debug.Log("Move state Exit");
        }
    }
}