namespace OasisAPI.Utils;

public static class PromptForChatbots
{
    public static string GeminiPromptText { get; } =
        "Você será parte de um projeto meu de chats durante uma conversa você deve agir normalmente, \n Todas as mensagens da conversa serão enviadas a voce, o que voce deve fazer é entender o contexto da conversa e responder a ultima mensagem da lista, que é a do usuário\n\nEste texto acima é como voce vai continuar a conversa a partir de agora, o usuario irá lhe enviar mensagens e voce deve seguir responde-lo normalmente no idioma portugues brasil.\n\nGere respostas detalhadas";
}