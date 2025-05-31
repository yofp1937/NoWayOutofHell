using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("# Main Data")]
    [SerializeField] float _distance = 5f;
    [SerializeField] LayerMask mask;
    public Ray InteractRay;

    [Header("# External Reference Data")]
    [SerializeField] Camera _camera;
    PlayerUI _playerUI;
    PlayerController _playerController;

    void Awake()
    {
        _playerUI = GetComponent<PlayerUI>();
        _playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        _playerUI.UpdateInteractText(string.Empty);

        // 전방으로 Raycast 보내서 상호작용 가능한 객체 감지
        InteractRay = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(InteractRay.origin, InteractRay.direction * _distance, Color.red);
        RaycastHit hitInfo;
        if(Physics.Raycast(InteractRay, out hitInfo, _distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                _playerUI.UpdateInteractText(interactable.OnLook());
                if(_playerController.Player.Interact.triggered)
                {
                    interactable.BaseInteract(gameObject);
                }
            }
        }
    }
}
