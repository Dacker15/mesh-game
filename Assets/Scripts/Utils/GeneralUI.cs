using UnityEngine;

public static class GeneralUI
{
    public static void SetActivePanel(GameObject[] panels, GameObject panel)
    {
        foreach (var currentPanel in panels)
        {
            if (currentPanel == panel)
            {
                currentPanel.SetActive(true);
            }
            else
            {
                currentPanel.SetActive(false);
            }
        }
    }
}