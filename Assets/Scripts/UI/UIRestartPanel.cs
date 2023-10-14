using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIRestartPanel : MonoBehaviour
{
    [SerializeField] private Button _startBtn;
    [SerializeField] private Text _scoreValue;

    public async Task Show(int playerScore, CancellationToken cancellationToken)
    {
        gameObject.SetActive(true);
        
        _scoreValue.text = playerScore.ToString();
        _startBtn.onClick.AddListener(StartBtn_OnClick);
       
        var statBtnClicked = false;
        while (!statBtnClicked)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
        }

        gameObject.SetActive(false);
        
        return;

        void StartBtn_OnClick()
        {
            _startBtn.onClick.RemoveListener(StartBtn_OnClick);
            statBtnClicked = true;
        }
    }
}