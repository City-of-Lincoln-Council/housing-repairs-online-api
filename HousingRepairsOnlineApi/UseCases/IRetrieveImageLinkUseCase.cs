using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IRetrieveImageLinkUseCase
    {
        public Task<string> Execute(string fileName);
    }
}
