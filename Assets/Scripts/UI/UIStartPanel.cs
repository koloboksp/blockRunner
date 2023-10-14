using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIStartPanel : MonoBehaviour
{
    [SerializeField] private Button _startBtn;

    public async Task Show(CancellationToken cancellationToken)
    {
        gameObject.SetActive(true);
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
            statBtnClicked = true;
        }
    }
}