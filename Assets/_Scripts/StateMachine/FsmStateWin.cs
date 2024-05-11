using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM._Scripts
{
    public class FsmStateWin : FsmState
    {
        private float _rotationDuration;
        private bool isRotating = false;
        private Quaternion desiredRotation;
        private Quaternion initialRotation;
        private float rotationTime = 0f;

        private Vector3 directionToRotate = Vector3.back;

        private Transform _player;
        public FsmStateWin(Fsm fsm, Transform player, float rotationDuration = 1f) : base(fsm)
        {
             _player = player;
            _rotationDuration = rotationDuration;
        }

        public override void Enter()
        {
            Debug.Log("Enter to win!");
            initialRotation = _player.rotation;
            desiredRotation = Quaternion.LookRotation(directionToRotate, Vector3.up);
            isRotating = true;
        }

        public override void Update()
        {
            if (isRotating)
            {
                rotationTime += Time.deltaTime;
                float t = Mathf.Clamp01(rotationTime / _rotationDuration);
                _player.rotation = Quaternion.Lerp(initialRotation, desiredRotation, t);

                if (t >= 1.0f)
                {
                    isRotating = false;
                    rotationTime = 0f;
                }
            }
        }
    }
}