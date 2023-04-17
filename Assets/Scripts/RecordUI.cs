using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RecordUI : MonoBehaviour
{
    public TextMeshProUGUI surviveTimeText;
    public TextMeshProUGUI dateText;
    public Image background;

    public Color evenRowColor;
    public Color oddRowColor;

    public void SetRecord(Record record, bool isEvenRow)
    {
        surviveTimeText.text = record.SurviveTime.ToString("0.00");
        dateText.text = record.Timestamp.ToString("yyyy-MM-dd HH:mm");
        background.color = isEvenRow ? evenRowColor : oddRowColor;
    }
}