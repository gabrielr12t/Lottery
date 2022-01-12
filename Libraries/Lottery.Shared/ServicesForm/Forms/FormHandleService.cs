using Lottery.Core.Infrastructure;

namespace Lottery.Shared.ServicesForm.Forms
{
    public class FormHandleService : IFormHandleService
    {
        private readonly IEngine _engine;

        public FormHandleService(IEngine engine)
        {
            _engine = engine;
        }

        public TForm Instance<TForm>()
            where TForm : Form
        {
            return _engine.Resolve<TForm>();
        }
    }
}
