using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM._Scripts
{
    public class FsmStateCheckpoint : FsmState
    {
        private Vector3 directionToRotate;

        public float rotationDuration;
        private bool isRotating = false;
        private Quaternion desiredRotation;
        private Quaternion initialRotation;
        private float rotationTime = 0f;

        private Transform _transform;

        public FsmStateCheckpoint(Fsm fsm, Transform transform) : base(fsm)
        {
            _transform = transform;
            rotationDuration = 0.5f;
            directionToRotate = Vector3.forward;
        }

        public void SetDirectionToRotate(Vector3 dir, float duration=0.5f)
        {
            directionToRotate = dir;
            rotationDuration = duration*1.5f;
        }

        public override void Enter()
        {
            Debug.Log("Checkpoint state enter");
            if (Mathf.Approximately(Vector3.Dot(_transform.forward, directionToRotate), 1f))
            {
                Fsm.SetState<FsmStateIdle>();
            }
            else
            {
                ChangeView(directionToRotate);
            } 
        }

        private void ChangeView(Vector3 direction)
        {
            initialRotation = _transform.rotation;
            desiredRotation = Quaternion.LookRotation(direction, Vector3.up);
            isRotating = true;
        }

        public override void Update()
        {
            if (isRotating)
            {
                rotationTime += Time.deltaTime;
                float t = Mathf.Clamp01(rotationTime / rotationDuration);
                _transform.rotation = Quaternion.Lerp(initialRotation, desiredRotation, t);

                if (t >= 1.0f)
                {
                    isRotating = false;
                    rotationTime = 0f;

                    Fsm.SetState<FsmStateIdle>();
                }
            }
        }
    }
}