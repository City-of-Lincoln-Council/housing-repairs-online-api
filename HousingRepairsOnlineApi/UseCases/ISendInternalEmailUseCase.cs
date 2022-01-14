using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface ISendInternalEmailUseCase
    {
        public Task Execute( string repairRef, string uprn, string address, string sor, string repairDescription, string contactNumber, string image);

    }
}
