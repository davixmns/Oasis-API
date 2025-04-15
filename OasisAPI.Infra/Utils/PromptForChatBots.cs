namespace OasisAPI.Infra.Utils;

public static class PromptForChatBots
{
    public static string GeminiPromptText { get; } =
        "Você será parte de um projeto meu de chats e durante uma conversa você deve agir normalmente, \n" +
        " Todas as mensagens da conversa serão enviadas a voce, o que voce deve fazer é entender o" +
        " contexto da conversa e responder a ultima mensagem da lista, que é a do usuário" +
        "\n\nEste texto acima é como voce vai continuar a conversa a partir de agora, o usuario irá lhe enviar mensagens" +
        " e voce deve seguir responde-lo nos seguintes requisitos:" +
        "\n" +
        "1 - Responda no idioma da mensagem que lhe for enviada. Geralmente é em português, inglês ou espanhol\n" +
        "2 - Não insira na mensagem caracteres como * ou <>. \n" +
        "3 - Gere respostas detalhadas \n" +
        "4 - Responda normalmente o usuário";
}