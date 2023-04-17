using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class HistoryMenu : MonoBehaviour
{
    public GameObject recordPrefab;
    public Transform recordsParent;
    public Button backButton;
    public Button sortByValueButton;
    public Button sortByDateButton;
    public TextMeshProUGUI pageInfoText;
    public int recordsPerPage = 10;

    private List<Record> records;
    private int currentPage = 0;
    private int totalPages;

    private void Start()
    {
        LoadRecords();
        totalPages = Mathf.CeilToInt(records.Count / (float)recordsPerPage);
        //DisplayRecords();
        SortByDate();
    }

    private void LoadRecords()
    {
        // Load your records from PlayerPrefs
        records = new List<Record>();
        for (int i = 1; i <= PlayerPrefs.GetInt("historyCount", 0); i++)
        {
            //Debug.Log("historycount:" + PlayerPrefs.GetInt("historyCount", 0));
            float surviveTime = PlayerPrefs.GetFloat($"surviveTimeHistory{i}", 0f);
            int timestamp = PlayerPrefs.GetInt($"timestamp{i}", 0);
            DateTime dateTime = new DateTime(1970, 1, 1).AddSeconds(timestamp);
            records.Add(new Record(surviveTime, dateTime));
        }
    }

    private void DisplayRecords(int startIndex = 0)
    {
        // Clear existing records
        foreach (Transform child in recordsParent)
        {
            Destroy(child.gameObject);
        }

        // Set the vertical spacing between records
        float verticalSpacing = recordPrefab.GetComponent<RectTransform>().rect.height;
        float topOffset = -112.5f;
        int verticalIdx = 0;

        // Display records
        for (int i = startIndex; i < startIndex + recordsPerPage && i < records.Count; i++)
        {
            Record record = records[i];
            GameObject recordObj = Instantiate(recordPrefab, recordsParent);
            recordObj.GetComponent<RecordUI>().SetRecord(record, i % 2 == 0);

            // Update the local position of the record
            RectTransform rectTransform = recordObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -verticalIdx * verticalSpacing + topOffset);
            verticalIdx += 1;
        }

        pageInfoText.text = $"{currentPage + 1} / {totalPages}";
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("MainMenu"); // Replace with your title scene's name
    }

    public void SortByValue()
    {
        records.Sort((a, b) => b.SurviveTime.CompareTo(a.SurviveTime));
        currentPage = 0;
        DisplayRecords();
    }

    public void SortByDate()
    {
        records.Sort((a, b) => b.Timestamp.CompareTo(a.Timestamp));
        currentPage = 0;
        DisplayRecords();
    }

    public void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            DisplayRecords(currentPage * recordsPerPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            DisplayRecords(currentPage * recordsPerPage);
        }
    }
}
