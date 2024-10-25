using System.Collections.Generic;
public class CSV_Importer
{
    public static List<Dictionary<string, object>> Spawn_Design = new List<Dictionary<string, object>>(CSVReader.Read("Spawner"));
    public static List<Dictionary<string, object>> Summon = new List<Dictionary<string, object>>(CSVReader.Read("Summon"));
    public static List<Dictionary<string, object>> Quest = new List<Dictionary<string, object>>(CSVReader.Read("Quest"));
}
