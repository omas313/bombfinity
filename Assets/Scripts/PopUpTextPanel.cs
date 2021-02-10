using UnityEngine;
using TMPro;
using System.Collections;

public class PopUpTextPanel : MonoBehaviour
{
    Animator _animator;
    TextMeshProUGUI _text;

    public IEnumerator EnterWithText(string text)
    {
        _text.SetText(text);
        _animator.SetTrigger("Enter");
        yield return new WaitForSeconds(2f);
        Exit();
        yield return new WaitForSeconds(0.25f);
    }

    void Exit()
    {
        _animator.SetTrigger("Exit");
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
}
