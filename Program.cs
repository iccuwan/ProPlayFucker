using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProPlayFucker
{
	class Program
	{
		private DiscordSocketClient client;
		private Dictionary<string, string> settings = new Dictionary<string, string>();
		private uint fuckCount = 0;
		private ulong faberId;

		private ulong kokosId = 245512711046299651;
		private ulong toksikRoleId = 767480431695495212;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World! Loading Settings...");
			new Program().MainAsync().GetAwaiter().GetResult();
			Console.ReadLine();
		}

		public async Task MainAsync()
		{
			if (File.Exists("settings.json"))
			{
				using (StreamReader fs = File.OpenText("settings.json"))
				{
					settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(fs.ReadToEnd());
					fuckCount = uint.Parse(settings["fuckCount"]);
					faberId = ulong.Parse(settings["faberId"]);
				}
			}
			else
			{
				Console.WriteLine("settings.json not found!");
				return;
			}
			client = new DiscordSocketClient();
			client.Log += Log;
			await client.LoginAsync(TokenType.Bot, settings["token"]);
			await client.StartAsync();

			client.MessageReceived += MessageReceived;
			client.UserJoined += Client_UserJoined;

			await Task.Delay(-1);
		}

		private async Task Client_UserJoined(SocketGuildUser arg)
		{
			if (arg.Id == kokosId)
			{
				await arg.AddRoleAsync(client.GetGuild(714733496538759248).GetRole(toksikRoleId));
			}
		}

		private async Task MessageReceived(SocketMessage arg)
		{
			if (arg.Content.StartsWith("/calc"))
			{
				string exrp = arg.Content.Substring(6);
				try
				{
					string value = new DataTable().Compute(exrp, null).ToString();
					await arg.Channel.SendMessageAsync(string.Format("{0} = {1}", exrp, value));
				}
				catch
				{
					await arg.Channel.SendMessageAsync("Что-то пошло не так...");
				}
			}
			if (arg.Content.ToLower() == "/ff stat")
			{
				// Вывод статистики
			}
			if (arg.Author.Id == faberId)
			{
				await arg.AddReactionAsync(new Emoji("🖕"));
				if (arg.Content.Length > 150)
				{
					await FuckFaber(arg.Channel);
				}
			}
			if (arg.Content.ToLower().Contains("ff"))
			{
				await FuckFaber(arg.Channel);
			}
			if (arg.Content.ToLower().Contains("cef") || arg.Content.ToLower().Contains("цеф"))
			{
				await arg.Channel.SendMessageAsync("Что-то говном запахло...");
				await arg.AddReactionAsync(new Emoji("💩"));
			}
			if (arg.Content.ToLower().Contains("dgs") || arg.Content.ToLower().Contains("дгс"))
			{
				await arg.Channel.SendMessageAsync("ДГС Сила, цeф могила!"); // В слове цеф используется английская e, чтобы бот не кидал говняшку
			}
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private async Task FuckFaber(ISocketMessageChannel channel)
		{
			fuckCount++;
			await channel.SendMessageAsync(settings["fuckMessage"]);
			settings["fuckCount"] = fuckCount.ToString();
			using (StreamWriter wr = new StreamWriter("settings.json", false))
			{
				wr.Write(JsonConvert.SerializeObject(settings));
			}
		}
	}
}
