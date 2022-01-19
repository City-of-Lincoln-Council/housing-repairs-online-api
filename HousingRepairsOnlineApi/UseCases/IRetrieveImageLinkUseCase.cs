using System.Threading.Tasks;

namespace HousingRepairsOnlineApi.UseCases
{
    public interface IRetrieveImageLinkUseCase
    {
        public string Execute(string fileName);
    }
}
