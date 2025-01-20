using TMPro;
using UnityEngine;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed;
    public float lifetime = 1f;

    private RectTransform rectTransform;

    void Start()
    {
        Destroy(gameObject, lifetime);

        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0f, -moveSpeed * Time.deltaTime);
    }
}
