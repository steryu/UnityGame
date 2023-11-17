using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelectorButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;
    [field: SerializeField] public Button Button { get; private set; }

    public void Init(string name, string description)
    {
        _name.text = name;
        _description.text = description;
        Button.onClick.RemoveAllListeners();
    }
}
