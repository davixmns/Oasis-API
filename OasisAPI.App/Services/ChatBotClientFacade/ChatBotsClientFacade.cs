using Domain.Entities;
using Domain.Utils;
using OasisAPI.Infra.Clients.Interfaces;
using OasisAPI.Infra.Dto;

namespace OasisAPI.App.Services.ChatBotClientFacade;

public class ChatBotsClientFacade : IChatBotsClientFacade
{
    private readonly IDictionary<ChatBotEnum, IChatBotClient> _chatBotClients;

    public ChatBotsClientFacade(IEnumerable<IChatBotClient> chatBotClients)
    {
        _chatBotClients = chatBotClients.ToDictionary(client => client.ChatBotEnum);
    }
    
    public async Task<IEnumerable<ChatBotMessageDto>> StartConversationWithChatBots(string message, HashSet<ChatBotEnum> selectedChatBots)
    {
        // Coletar as funções de execução
        var tasks = new List<Func<Task<ChatBotMessageDto>>>();
        
        // Obtém o título do chat do Gemini
        if (_chatBotClients[ChatBotEnum.Gemini] is IGetChatTitle geminiClient)
        {
            tasks.Add(() => geminiClient.GetChatTitleAsync(message));
        }

        // Adicionar as tarefas de acordo com os chatbots selecionados
        foreach (var chatBotEnum in selectedChatBots)
        {
            var actualClient = _chatBotClients[chatBotEnum];

            switch (actualClient)
            {
                case ICreateThreadAndSendMessage client:
                    tasks.Add(() => client.CreateThreadAndSendMessageAsync(message));
                    break;
            }
        }

        // Executa todas as tarefas ao final
        return await ExecuteChatBotsTasks(tasks);
    }
    
    public async Task<IEnumerable<ChatBotMessageDto>> ContinueConversationWithChatBotsAsync(string message, IList<string> allMessages, HashSet<OasisChatBotDetails> chatBotDetailsSet)
    {
        // Coletar as funções de execução
        var tasks = new List<Func<Task<ChatBotMessageDto>>>();

        // Adicionar as tarefas de acordo com os detalhes dos chatbots
        foreach (var chatBotAndThreadDto in chatBotDetailsSet)
        {
            var actualClient = _chatBotClients[chatBotAndThreadDto.ChatBotEnum];

            switch (actualClient)
            {
                case ISendMessageToThread client:
                    tasks.Add(() => client.SendMessageToThreadAsync(chatBotAndThreadDto.ThreadId!, message));
                    break;

                case ISendAllMessagesToThread client:
                    tasks.Add(() => client.SendAllMessagesAsync(allMessages));
                    break;
            }
        }

        // Executa todas as tarefas ao final
        return await ExecuteChatBotsTasks(tasks);
    }
    
    private async Task<IEnumerable<ChatBotMessageDto>> ExecuteChatBotsTasks(IEnumerable<Func<Task<ChatBotMessageDto>>> chatBotTasks)
    {
        // Executa todas as funções de tarefa e captura erros, se necessário
        var handlingTasks = chatBotTasks.Select(async task =>
        {
            try
            {
                return await task();
            }
            catch (Exception)
            {
                // Retorna uma mensagem de erro para o chatbot que falhou
                return new ChatBotMessageDto
                {
                    Message = "Internal error processing message",
                    ThreadId = "",
                    ChatBotEnum = ChatBotEnum.User 
                };
            }
        });
        
        // Retorna todas as respostas após executar as tarefas
        return await Task.WhenAll(handlingTasks);
    }
}
