namespace Infrastructures.Storages.LogActionStorage
{
   
}
using System;
namespace Infrastructures.Storages.LogActionStorage
{
    public class LogActions
    {
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public string PerformedBy { get; set; }
        public string Description { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
