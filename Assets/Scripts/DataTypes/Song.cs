using UnityEngine;

public class Song
{
    private string name;
    public string Name
    {
        get { return name; }
        private set { name = value; }
    }

    private float length;
    public float Length
    {
        get { return length; }
        private set { length = value; }
    }

    private string filePath;
    public string FilePath
    {
        get { return filePath; }
        private set { filePath = value; }
    }

    private WWW link;
    public WWW Link
    {
        get { return link; }
        private set { link = value; }
    }

    public Song(string name, string filePath)
    {
        Name = name;
        FilePath = filePath;
    }
}
