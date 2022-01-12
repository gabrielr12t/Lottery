namespace Lottery.Shared.ServicesForm.Forms
{
    public interface IFormHandleService
    {
        TForm Instance<TForm>() where TForm : Form;
    }
}
