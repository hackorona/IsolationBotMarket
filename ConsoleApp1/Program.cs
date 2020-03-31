using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ConsoleApp1
{
    class Program
    {
        static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            new MessageHandler("");
            Thread.Sleep(int.MaxValue);
      
        }


        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

                 await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "You said:\n" + e.Message.Text, replyToMessageId: e.Message.MessageId );
               
            }
        }
    }
}
