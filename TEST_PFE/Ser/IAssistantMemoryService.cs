using TEST_PFE.Models;

namespace TEST_PFE.Ser
{
    public interface IAssistantMemoryService
    {
        AssistantMemory GetMemory(string userId);
        void SaveChoice(string userId, string choice);
    }

}
