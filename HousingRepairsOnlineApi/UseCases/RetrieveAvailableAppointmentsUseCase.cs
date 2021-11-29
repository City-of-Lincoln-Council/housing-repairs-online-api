using System;
using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.UseCases
{
    public class RetrieveAvailableAppointmentsUseCase
    {
        public async Task Execute(string repairCode, string uprn)
        {
            if (repairCode == string.Empty)
            {
                throw new ArgumentException();
            }
            throw new ArgumentNullException();
        }
    }
}
