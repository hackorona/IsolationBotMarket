using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ConsoleApp1.Models
{
    public class MessageHandler
    {
        private TelegramBotClient botClient;
        private Queue<MessageEventArgs> _messagesQueue;
        private Management _management;
        private ProductsManagement _productsManagement;
        public MessageHandler(string _apiKey)
        {
            _management = new Management();
            _messagesQueue = new Queue<MessageEventArgs>();
            botClient = new TelegramBotClient(_apiKey);
            _productsManagement = new ProductsManagement();
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Task.Run(() => HandleMessagesInQueue());
        }

        private void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            _messagesQueue.Enqueue(e);
        }

        private void HandleMessagesInQueue()
        {
            while (true)
            {
                if (_messagesQueue.Count > 0)
                {
                    var message = _messagesQueue.Dequeue();
                    _ = ParseMessageAsync(message);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private async Task ParseMessageAsync(MessageEventArgs e)
        {
            var uId = e.Message.Chat.Id;
            var userExist = _management.IsUserExist(e.Message.Chat.Id);
            if (!userExist)
            {
                _management.AddUser(e.Message.Chat.Id);
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "ברוך הבא נצטרך קצת פרטים עליך");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "שם פרטי?");
                return;
            }

            if (_management.GetUserById(uId).state != UserState.Complete)
            {
                _ = ParseRegistartion(_management.GetUserById(uId), e);
                return;
            }

            if (e.Message.Text != null)
            {
                _ = ParseAction(_management.GetUserById(uId), e);
            }
        }

        private async Task ParseRegistartion(User user, MessageEventArgs e)
        {
            switch (user.state)
            {
                case UserState.RequireFirstName:
                    user.UpdateFirstName(e.Message.Text);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $" ברוך הבא {e.Message.Text}");
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "ועכשיו שם משפחה");
                    return;
                case UserState.RequireLastName:
                    user.UpdateLastName(e.Message.Text);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"תודה רבה");
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "באיזו עיר גרים?");
                    return;
                case UserState.RequireCity:
                    user.UpdateCity(e.Message.Text);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"תודה רבה");
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "באיזו רחוב גרים?");
                    return;
                case UserState.RequireAddress:
                    user.UpdateAddress(e.Message.Text);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"תודה רבה הרישום הצלחי");
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $".'.... אם אתה רוצה לשתף משהו, אנא כתוב 'ישלי ....'. אם אתה זקוק למשהו, אנא כתוב 'צריך");
                    return;
            }
        }

        private async Task ParseAction(User user, MessageEventArgs e)
        {
            string request;
            List<string> _input = e.Message.Text.Split(' ').ToList();
            User _user;

            var uId = e.Message.Chat.Id;
            string action = _input[0];
            _input.RemoveAt(0);
            request = string.Join(" ", _input);
            if (action == "צריך" || action == "יש")
            {
                if (action == "יש")
                {
                    _productsManagement.AddProduct(request, uId);
                    await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"תודה רבה");
                }
                else
                {
                   long userWithProduct = _productsManagement.GetProduct(request);
                    if(userWithProduct != 0)
                    {
                       _user = _management.GetUserById(userWithProduct);
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"{_user.UserData()}");
                        await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: $"תודה רבה");
                    }
                }
             

            }
            else if (e.Message.Text != null)
            {
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "You said:\n" + e.Message.Text, replyToMessageId: e.Message.MessageId);
            }
        }
    }
}
