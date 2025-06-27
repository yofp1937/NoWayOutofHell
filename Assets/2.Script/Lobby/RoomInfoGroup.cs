using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfoGroup : MonoBehaviour
{
    public RoomInfo RoomInfo;

    Image _image;
    Button _btn;

    RoomSearchHandler _handler;

    void Awake()
    {
        _image = GetComponent<Image>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClickBtn);
    }

    public void Init(RoomInfo roomInfo, RoomSearchHandler handler)
    {
        RoomInfo = roomInfo;
        _handler = handler;
        Debug.Log($"RoomInfo: {RoomInfo}, handler: {handler}");
    }

    void OnClickBtn()
    {
        _handler.ChangeTarget(this);
        SetImageEnable(true);
    }

    public void SetImageEnable(bool active)
    {
        _image.enabled = active;
    }
}
