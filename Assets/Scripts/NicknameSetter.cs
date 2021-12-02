using SolarSystemKritskiy_Main;
using UnityEngine.UI;

public class NicknameSetter : Singleton<NicknameSetter>
{
    private Button _btn;

    private void Start()
    {
        _btn = GetComponentInChildren<Button>();
        _btn.onClick.AddListener(OnClick);
    }
    
    public void OnClick()
    {
        ServiceLocator.Instance.SetNickname(GetComponentInChildren<InputField>().text);
    }
}
