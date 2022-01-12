namespace Lottery.Shared.ServicesForm.Alerts
{
    public interface IAlertService
    {
        void InfoAlert(string message);
        void WarningAlert(string message);
        void SuccessAlert(string message);
        void ErrorAlert(string message);
    }
}
