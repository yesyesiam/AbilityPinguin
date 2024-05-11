using UnityEngine;
using UnityEngine.EventSystems;

namespace FSM._Scripts
{
    public class FsmStateIdle : FsmState
    {
        private RowManager _rowManager;
        private TestOne _playerControl;
        private AbilityStorage _abilityStorage;

        public FsmStateIdle(Fsm fsm, TestOne playerControl, RowManager rowManager, AbilityStorage abilityStorage) : base(fsm)
        {
            _playerControl = playerControl;
            _rowManager = rowManager;
            _abilityStorage = abilityStorage;
        }
        
        public override void Enter()
        {
            Debug.Log("Idle state Enter");
        }

        public override void Exit()
        {
            Debug.Log("Idle state Exit");
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0)&& !EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.TryGetComponent<Platform>(out var platform))
                    {
                        Debug.Log(platform.index);
                        if (_abilityStorage.IsCurrentAbilityNotNull)
                        {
                            _abilityStorage.SetTarget(platform);
                            _playerControl.SetTarget(platform);
                        }
                        else
                        {
                            _playerControl.PlayClickEffect(platform.transform.position);
                            if (_rowManager.MoveToPlatform(platform))
                            {
                                Fsm.SetState<FsmStateMove>();
                            }
                        }
                    }
                }
            }
        }
    }
}
