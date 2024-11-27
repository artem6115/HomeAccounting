namespace TelegramBot
{
    public class Class1
    {
        private static IReplyMarkup GetStartButtons()
        {
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>>
                {
                    new List<KeyboardButton>{ new KeyboardButton ("О программе"), new KeyboardButton ("Команды") },
                    new List<KeyboardButton>{ new KeyboardButton ("Формат записей") },
                    new List<KeyboardButton>{ new KeyboardButton ("Сделать отчёт") }


                }
            );
        }
        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
    CancellationToken cancellationToken)
        {
        }
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            String text;
            String textInfo;
            String type = "прочее";
            String cost;
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)

            { // await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Connect");
                if (Setting.idChat == 0)
                {
                    Setting.idChat = update.Message.Chat.Id;
                    Setting.Save();
                }
                if (skip) return;
                if (update.Message.Type != MessageType.Text) return;

                text = update.Message.Text;
                text = text.ToLower().Trim();

                if (text == "/start")
                {

                    string textMessage = "Бот запущен, и при запуске программы\nданные синхронизируются\n" +
                                         "Домашняя бухгалтерия готова к работе.\n" +
                                         "Приятного пользования";
                    SendMessage(botClient, textMessage);


                    return;
                }

                if (text == "о программе")
                {

                    SendMessage(botClient, textMessageInfo);


                    return;
                }

                if (text == "команды")
                {
                    string textMessage = "@del-удаляет последнюю запись\n@get-показывает 5 последних записей\n@get [число] - показывает указаное число записей";
                    SendMessage(botClient, textMessage);


                    return;
                }

                if (text == "формат записей")
                {


                    SendMessage(botClient, textMessageFormat);


                    return;
                }

                if (text == "сделать отчёт")
                {

                    string textMessage = Note.CreateTxtReport(Note.Range(Notes, new DateTime(nowYear, nowMonth, 1), new DateTime(nowYear, nowMonth, 1, 0, 0, 0).AddMonths(1)), 5);
                    SendMessage(botClient, textMessage);


                    return;
                }


                if (text == "@del")
                {
                    if (Notes.Count > 0) Notes.Remove(Notes.Last());
                    Note.Save(Notes);
                    return;
                }
                if (text.Split(' ').First() == "@get")
                {
                    string textMessage;
                    var buff = text.Split(' ');
                    if (buff.Count() > 1)
                    {

                        if (int.TryParse(buff[1].Trim(), out int k))
                        {
                            textMessage = Note.GetNotes(Notes, k);
                            SendMessage(botClient, textMessage);
                            return;
                        }
                    }
                    textMessage = Note.GetNotes(Notes, 5);
                    SendMessage(botClient, textMessage);
                    return;
                }
                if (text[0] == '$')
                {

                    text = text.Split('.').Last().Trim();
                    if (int.TryParse(text, out int s))
                    {
                        Setting.capital = s;
                        Setting.Save();
                    }
                    return;
                }


                var group = (text.Split('.'));
                if (group.Length > 1)
                {
                    if (group[0] != "")
                        type = group[0];
                    text = group[1];
                }
                else text = group[0];


                var InfoParametrs = text.Trim().Split(' ');

                cost = InfoParametrs.Last();
                if (cost == null || cost == "") return;
                cost = (cost[0] == '+') ? cost.Remove(0, 1) : cost.Insert(0, "-");
                textInfo = String.Join(" ", InfoParametrs.Take(InfoParametrs.Length - 1));
                var note = Note.CreateNote((update.Message.Date.AddHours(5)).ToString(), type, textInfo, cost);
                if (note is null)
                {
                    using (var file = new StreamWriter(Setting.PathErroreFile, true))
                    {
                        file.WriteLine($"{(update.Message.Date.AddHours(5)).ToString()}#{type}#{textInfo}#{cost}");
                    }
                }
                else
                {
                    Notes.Add(note);
                    Note.Save(Notes);
                }


            }


        }


        public static void SendMessage(ITelegramBotClient botClient, string text)
        {
            try
            {
                botClient.SendTextMessageAsync(Setting.idChat, text, replyMarkup: GetStartButtons());
            }
            catch
            {
                MessageBox.Show("Не удалось ответить телеграмм пользователю", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void BotStart()
        {
            if (!apiTrue)
            {
                MessageBox.Show("Отсутствует api бота, подключение невозможно\n" +
                    "Для устранения проблемы перезапустите программу\n" +
                    "и введите верный api", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            botWowk = true;
            panel3.BackColor = System.Drawing.Color.Lime;
            bot = new TelegramBotClient(Setting.api);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );


        }

    }
}
