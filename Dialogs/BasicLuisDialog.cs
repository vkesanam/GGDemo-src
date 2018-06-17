using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hello, Welcome to Gargash Enterprises, I am here to help make a better decision. How can I help you today?");
            context.Wait(MessageReceived);
        }
        [LuisIntent("Class")]
        public async Task ClassIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("That’s an amazing choice, to assist you better, I need some more information like name, contact number and email id.I am requesting this information, just in case if I will have to seek my Sales support at office. Hope you don’t mind?");
            //context.Wait(MessageReceived);
            //var activity = context.Activity as Activity;
            //if (activity.Type == ActivityTypes.Message)
            //{
            //    var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
            //    var isTyping = activity.CreateReply("Nerdibot is thinking...");
            //    isTyping.Type = ActivityTypes.Typing;
            //    await connector.Conversations.ReplyToActivityAsync(isTyping);

            //    // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
            //    Thread.Sleep(2500);
            //}

            //Thread.Sleep(2500);

            PromptDialog.Text(
               context: context,
               resume: CustomerNameFromGreeting,
               prompt: "May i know your Name please?",
               retry: "Sorry, I don't understand that.");
        }
        public virtual async Task CustomerNameFromGreeting(IDialogContext context, IAwaitable<string> result)
        {
            string response = await result;
            customerName = response;

            PromptDialog.Text(
            context: context,
            resume: CustomerMobileNumberHandler,
            prompt: "What is the best number to contact you?",
            retry: "Sorry, I don't understand that.");
        }
        public virtual async Task CustomerMobileNumberHandler(IDialogContext context, IAwaitable<string> result)
        {
            string response = await result;
            custMobileNumber = response;

            PromptDialog.Text(
            context: context,
            resume: CustomerEmailHandler,
            prompt: "What is your email id?",
            retry: "Sorry, I don't understand that.");
        }
        public virtual async Task CustomerEmailHandler(IDialogContext context, IAwaitable<string> result)
        {
            string response = await result;
            custEmailID = response;

            await context.PostAsync("Many Thanks " + customerName + ".We have 3 models in C Class, Sedan, Coupe’ and Cabriolet. What's your preference?");


            //var reply = context.MakeMessage();

            //reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            //reply.Attachments = GetCardsAttachments();

            //await context.PostAsync(reply);

            //context.Wait(MessageReceived);
        }
        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}