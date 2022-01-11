namespace Lottery.Core.Models.Games
{
    public class Game : BaseEntity, IBaseEntity
    {
        public Game()
        {
            Numbers = new List<int>(15);
            Excludes = new List<int>(3);
        }

        public IList<int> Numbers { get; set; }
        public IList<int> Excludes { get; set; }
    }
}
