using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace HousingRepairsOnlineApi.Helpers
{
    public class SoREngine
    {
        public string MapSorCode(string location, string problem, string issue)
        {
            var reader = new StreamReader("SoRConfig.json");
            var json = reader.ReadToEnd();
            var soRMapping = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(json);
            var result = soRMapping[location][problem][issue];

            return result;
        }
    }
}
