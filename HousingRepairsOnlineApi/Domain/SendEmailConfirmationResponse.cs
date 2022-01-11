namespace HousingRepairsOnlineApi.Domain
{
    public class SendEmailConfirmationResponse
    {
        public string BookingReference { get; set; }
        public string Email { get; set; }
        public string GovNotifyId { get; set; }
        public string TemplateId { get; set; }
        public string AppointmentTime { get; set; }
    }
}
