using System.Runtime.CompilerServices;
using Discord;
using Discord.Net;
using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace LoRHelper
{
    class Program
    {
        private readonly DiscordSocketClient client;
        private readonly string token;

        private static HttpClient httpClient = new HttpClient();
        private LoRApiController controller;
        private ulong guildId;

        public Program()
        {
            this.client = new DiscordSocketClient();
            this.token = loadJson()?["discordToken"] ?? "Token not found";
            controller = new LoRApiController(httpClient);
            this.client.Ready += Client_Ready;
            this.client.MessageReceived += MessageHandler;
            this.client.SlashCommandExecuted += SlashCommandsHandler;
        }

        private async Task Client_Ready()
        {
            // Let's build a guild command! We're going to need a guild so lets just put that in a variable.
            var guild = client.Guilds.First();

            // Next, lets create our slash command builder. This is like the embed builder but for slash commands.
            var guildCommand = new SlashCommandBuilder();

            // Note: Names have to be all lowercase and match the regular expression ^[\w-]{3,32}$
            guildCommand.WithName("get-static");

            // Descriptions can have a max length of 100.
            guildCommand.WithDescription("This is my first guild slash command!");

            // Let's do our global command
            var globalCommand = new SlashCommandBuilder();
            globalCommand.WithName("first-global-command");
            globalCommand.WithDescription("This is my first global slash command");

            try
            {
                // Now that we have our builder, we can call the CreateApplicationCommandAsync method to make our slash command.
                await guild.CreateApplicationCommandAsync(guildCommand.Build());

                // With global commands we don't need the guild.
                await client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
            }
            catch (HttpException exception)
            {
                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                Console.WriteLine(json);
            }
        }

        private async Task SlashCommandsHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "get-static":
                    await controller.GetDeckAsync();
                    break;
                default:
                    System.Console.WriteLine(command.Data.Name);
                    await ReplyAsync(null, "");
                    break;
            }
        }

        public async Task StartBotAsync()
        {
            this.client.Log += LogFuncAsync;
            await this.client.LoginAsync(TokenType.Bot, token);
            await this.client.StartAsync();
            await Task.Delay(-1);
            async Task LogFuncAsync(LogMessage message) => Console.WriteLine(message);
        }

        private async Task MessageHandler(SocketMessage message)
        {
            if (message.Author.IsBot)
                return;

            return;
            //await ReplyAsync(message, "C# response works");
        }

        private async Task ReplyAsync(SocketMessage message, string response)
        {
            await message.Channel.SendMessageAsync(response);
        }

        static void Main(string[] args) => new Program().StartBotAsync().GetAwaiter().GetResult();

        Dictionary<string, string>? loadJson()
        {
            using (StreamReader r = new StreamReader("./configs.json"))
            {
                string json = r.ReadToEnd();
                Dictionary<string, string>? response = JsonConvert.DeserializeObject<
                    Dictionary<string, string>
                >(json);

                return response;
            }
        }
    }
}
