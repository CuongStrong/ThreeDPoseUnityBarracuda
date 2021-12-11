#if FIRESTORE
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] public string key, args;

    private Text textElement;
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        textElement = GetComponent<Text>();
        textMeshPro = GetComponent<TextMeshProUGUI>();

        LocalizationManager.Instance.OnLocalizationChanged += OnLocalizationChanged;
        OnLocalizationChanged(this, null);
    }

    public void SetKey(string key, string args = "")
    {
        this.key = key;
        this.args = args;
        OnLocalizationChanged(this, null);
    }

    public void SetArgs(string args)
    {
        this.args = args;
        OnLocalizationChanged(this, null);
    }

    private void OnLocalizationChanged(object sender, System.EventArgs e)
    {
        string text = LocalizationManager.Instance.GetText(key, args);

        if (textElement != null)
        {
            textElement.text = text;
        }

        if (textMeshPro != null)
        {
            textMeshPro.text = text;
        }
    }
}
#endif