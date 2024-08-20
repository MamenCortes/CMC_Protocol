using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EventTime
{
    private float Onset { get; set; }
    private float Duration { get; set; }
    private string Condition {  get; set; }
    
    public EventTime(float onset, float duration, string condition)
    {
        Onset = onset;
        Duration = duration;
        Condition = condition;
    }
    public EventTime(float onset, float duration)
    {
        Onset = onset;
        Duration = duration;
        Condition = null;
    }

    public override string ToString()
    {
        if (Condition == null)
        {
            return $"Onset: {Onset}, Duration: {Duration}";
        }
        else
        {
            return $"Onset: {Onset}, Duration: {Duration}, Condition: {Condition}";
        }
    }

    public string ToCSV(string delimiter)
    {
        string formattedOnset = Onset.ToString(CultureInfo.InvariantCulture); 
        string formattedDuration = Duration.ToString(CultureInfo.InvariantCulture);

        return $"{formattedOnset}{delimiter}{formattedDuration}{delimiter}{Condition}\n";
        
    }

}
