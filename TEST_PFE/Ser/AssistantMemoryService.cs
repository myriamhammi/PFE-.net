using TEST_PFE.Models;

namespace TEST_PFE.Ser
{
    public class AssistantMemoryService : IAssistantMemoryService
    {
        private readonly Dictionary<string, AssistantMemory> _memoryStore = new();

        public AssistantMemory GetMemory(string userId)
        {
            if (!_memoryStore.ContainsKey(userId))
            {
                _memoryStore[userId] = new AssistantMemory
                {
                    UserId = userId,
                    LastVisit = DateTime.Now
                };
            }

            return _memoryStore[userId];
        }

        public void SaveChoice(string userId, string choice)
        {
            var mem = GetMemory(userId);
            mem.History.Add(choice);
            mem.LastVisit = DateTime.Now;
        }
    }
}
