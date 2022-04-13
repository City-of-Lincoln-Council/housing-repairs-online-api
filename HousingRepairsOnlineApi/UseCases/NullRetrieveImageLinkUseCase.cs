namespace HousingRepairsOnlineApi.UseCases
{
    public class NullRetrieveImageLinkUseCase : IRetrieveImageLinkUseCase
    {
        public string Execute(string fileName)
        {
            return fileName;
        }
    }
}
