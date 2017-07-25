using UnityEngine;
using UnityEngine.UI;

public class ManageUI : MonoBehaviour
{
    public Button endTurnButton;
    public Button spawnFirefolkButton;
    public Button spawnTreefolkButton;
    public Button spawnUndeadButton;
    public Button manageCharacterButton;

    void Awake()
    {
        endTurnButton.onClick.AddListener(() => { GameEventSignals.DoCurrentTurnEnd(); });
        spawnFirefolkButton.onClick.AddListener(() => { ManageInput.Ins.axisFire1Down = ManageInput.Ins.SpawnCharacter; });
        spawnTreefolkButton.onClick.AddListener(() => { ManageInput.Ins.axisFire1Down = ManageInput.Ins.SpawnCharacter2; });
        spawnUndeadButton.onClick.AddListener(() => { ManageInput.Ins.axisFire1Down = ManageInput.Ins.SpawnCharacter3; });
        manageCharacterButton.onClick.AddListener(() => { ManageInput.Ins.axisFire1Down = ManageInput.Ins.ManageCharacter; });
    }

    void OnDestroy()
    {
        endTurnButton.onClick.RemoveAllListeners();
        spawnFirefolkButton.onClick.RemoveAllListeners();
        spawnTreefolkButton.onClick.RemoveAllListeners();
        spawnUndeadButton.onClick.RemoveAllListeners();
        manageCharacterButton.onClick.RemoveAllListeners();
    }
}
