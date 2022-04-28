namespace HousingRepairsOnlineApi.UseCases
{
    public class NullSendInternalEmailUseCase : ISendInternalEmailUseCase
    {
        public void Execute(string repairRef, string uprn, string address, string sor, string repairDescription, string contactNumber, string image)
        {
            // Intentionally blank
        }
    }
}
