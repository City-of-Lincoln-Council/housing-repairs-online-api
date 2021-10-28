using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Helpers
{
    public class SoREngine : ISoREngine
    {
        private readonly IDictionary<string, IDictionary<string, IDictionary<string, string>>> soRMapping;

        public SoREngine(IDictionary<string, IDictionary<string, IDictionary<string, string>>> soRMapping)
        {
            this.soRMapping = soRMapping;
        }

        public string MapSorCode(string location, string problem, string issue)
        {
            var result = soRMapping[location][problem][issue];

            return result;
        }
    }
}
