using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class UIGamePanel : MonoBehaviour
{
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Text _scoreValue;

    private bool _restartRequested = false;
    private Player _player;
    public bool RestartRequested => _restartRequested;

    private void Awake()
    {
        _restartBtn.onClick.AddListener(() => _restartRequested = true);
    }

    private void Update()
    {
        _scoreValue.text = _player.Score().ToString();
    }

    public void Show(Player player)
    {
        
        gameObject.SetActive(true);
        _player = player;
    }
    
    public void Hide()
    {
        ResetState();

        gameObject.SetActive(false);
    }

    private void ResetState()
    {
        _restartRequested = false;
    }
}