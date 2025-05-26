using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIView : MonoBehaviour, IEndGameUIView
{
    [SerializeField] private Button _closeEndGameUI;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _loseText;
    [SerializeField] private CanvasGroup canvasGroup;

    public Button CloseEndGameUI => _closeEndGameUI;
    public CanvasGroup CanvasGroup => canvasGroup;

    public void ShowUI(bool isWin)
    {
        if(isWin)
        {
            _winText.enabled = true;
            _loseText.enabled = false;
        }
        else
        {
            _winText.enabled = false;
            _loseText.enabled = true;
        }

        canvasGroup.Show();
    }

    public void HideUI()
    {
        canvasGroup.Hide();
    }
}