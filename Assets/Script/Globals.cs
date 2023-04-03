using UnityEngine;
using UnityEngine.UI;

public static class Globals
{
    public static int Episode = 0;
    public static int Success = 0;
    public static int Fail = 0;
    public static int Player = 6;
    public static int check = 0;
    private static Text txtDebug = null;

    public static void ScreenText()
    {
        if (txtDebug == null)
        {
            txtDebug = GameObject.Find("txtDebug").gameObject.GetComponent<Text>();
        }
        float SuccessPercent = (Success / (float)(Success + Fail)) * 100;
        if (txtDebug != null)
        {
            txtDebug.text = string.Format("Episode={0}, Success={1}, Fail={2}, Player={3} %{4}"
                , Episode
                , Success
                , Fail
                , Player
                , SuccessPercent.ToString("0") );
        }
    }
}