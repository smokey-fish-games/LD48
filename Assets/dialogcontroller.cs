using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dialogcontroller : MonoBehaviour
{
    [SerializeField] private TMP_Text me;

    static bool runningTextOutput = false;

    float timeBetweenStrings = 2f;
    float timeBetweenChar = 0.01f;

    List<string> ToWrite = new List<string>();

    string StringTowrite = "";

    public static dialogcontroller current = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Debug.LogError("Doofus you moofus you have two status machinusses.");
        }
    }

    public void AddStringToWrite(string thestring)
    {
        if (ToWrite.Count > 0)
        {
            if (thestring == ToWrite[ToWrite.Count - 1])
            {
                return;
            }
        }
        if (thestring == StringTowrite)
        {
            return;
        }

        ToWrite.Add(thestring);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!runningTextOutput && ToWrite.Count > 0)
        {
            // We need to write a string
            StringTowrite = ToWrite[0];
            ToWrite.RemoveAt(0);
            StartCoroutine("writeString");
            runningTextOutput = true;
        }
    }

    IEnumerator writeString()
    {
        string currentOut = "";

        foreach (char c in StringTowrite)
        {
            currentOut += c;
            me.text = currentOut;
            yield return new WaitForSeconds(timeBetweenChar);
        }

        StringTowrite = "";
        yield return new WaitForSeconds(timeBetweenStrings);
        me.text = "";
        runningTextOutput = false;
    }
}
