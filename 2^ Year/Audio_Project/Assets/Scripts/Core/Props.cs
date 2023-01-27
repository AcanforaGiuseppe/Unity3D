using System;
using System.IO;
using System.Collections.Generic;

static public class Props
{
    static bool readed = false;

    static string fileToRead = "Assets/Scripts/Core/Properties.txt";
    static StreamReader reader;

    static Dictionary<string, float> values;

    //--------------------------------------MAIN METHODS-------------------------------------------

    static public void Init()
    {
        // READING ALGORITHM
        if (!readed)
        {
            values = new Dictionary<string, float>();
            reader = new StreamReader(fileToRead);

            string line;
            while((line = reader.ReadLine()) != null)
            {
                string[] splittedLine = line.Split('=');
                values.Add(splittedLine[0].ToLower(), float.Parse(splittedLine[1]));
            }

            readed = true;
        }
    }

    //--------------------------------------OTHER METHODS------------------------------------------

    static public float GetValue(string key)
    {
        float n = 0;

        if (values.ContainsKey(key.ToLower()))
        {
            n = values[key.ToLower()];
        }

        return n;
    }
}
