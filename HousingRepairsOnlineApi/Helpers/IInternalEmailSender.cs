using HousingRepairsOnlineApi.Domain;

namespace HousingRepairsOnlineApi.Helpers
{
    public interface IInternalEmailSender
    {
        public void Execute(Repair repair);
    }
}
