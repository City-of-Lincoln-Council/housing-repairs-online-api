namespace HousingRepairsOnlineApi.Domain
{
    public class SendSmsConfirmationResponse
    {
        public string BookingReference { get; set; }
        public string PhoneNumber { get; set; }
        public string GovNotifyId { get; set; }
        public string TemplateId { get; set; }
        public string AppointmentTime { get; set; }
    }
}
