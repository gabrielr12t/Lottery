using Lottery.Shared.Components;

namespace Lottery.Shared.ServicesForm.Alerts
{
    internal class AlertService : IAlertService
    {
        public void ErrorAlert(string message)
        {
            ShowAlert(message, AlertForm.enmType.Error);
        }

        public void InfoAlert(string message)
        {
            ShowAlert(message, AlertForm.enmType.Info);
        }

        public void SuccessAlert(string message)
        {
            ShowAlert(message, AlertForm.enmType.Success);
        }

        public void WarningAlert(string message)
        {
            ShowAlert(message, AlertForm.enmType.Warning);
        }

        private void ShowAlert(string message, AlertForm.enmType enmType)
        {
            var form = new AlertForm();
            form.showAlert(message, enmType);
        }
    }
}
