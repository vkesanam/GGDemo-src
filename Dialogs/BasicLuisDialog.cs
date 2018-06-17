using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

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
        string customerName;
        string custMobileNumber;
        string custEmailID;
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
            var activity = context.Activity as Activity;
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                var isTyping = activity.CreateReply("Nerdibot is thinking...");
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);

                // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
                Thread.Sleep(2500);
            }

            //Thread.Sleep(2500);

            PromptDialog.Text(
               context: context,
               resume: CustomerNameFromGreeting,
               prompt: "May i know your Name please?",
               retry: "Sorry, I don't understand that.");
        }
        public async Task CustomerNameFromGreeting(IDialogContext context, IAwaitable<string> result)
        {
            //string response = await result;
            //customerName = response;

            PromptDialog.Text(
            context: context,
            resume: CustomerMobileNumberHandler,
            prompt: "What is the best number to contact you?",
            retry: "Sorry, I don't understand that.");
        }
        public async Task CustomerMobileNumberHandler(IDialogContext context, IAwaitable<string> result)
        {
            //string response = await result;
            //custMobileNumber = response;

            PromptDialog.Text(
            context: context,
            resume: CustomerEmailHandler,
            prompt: "What is your email id?",
            retry: "Sorry, I don't understand that.");
        }
        public async Task CustomerEmailHandler(IDialogContext context, IAwaitable<string> result)
        {
            //string response = await result;
            //custEmailID = response;

            await context.PostAsync("Many Thanks " + customerName + ".We have 3 models in C Class, Sedan, Coupe’ and Cabriolet.");
            //context.Wait(MessageReceived);
            var activity1 = context.Activity as Activity;
            if (activity1.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new System.Uri(activity1.ServiceUrl));
                var isTyping = activity1.CreateReply("Nerdibot is thinking...");
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);

                // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
                Thread.Sleep(2500);
            }

            var reply = context.MakeMessage();

            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            reply.Attachments = GetCardsAttachments();

            await context.PostAsync(reply);

            var activity = context.Activity as Activity;
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                var isTyping = activity.CreateReply("Nerdibot is thinking...");
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);

                // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
                Thread.Sleep(2500);
            }

            PromptDialog.Text(
           context: context,
           resume: CustomerPreferenceHandler,
           prompt: "What's your preference?",
           retry: "Sorry, I don't understand that.");

            //await context.PostAsync("");
            //var activity2 = context.Activity as Activity;
            //if (activity2.Type == ActivityTypes.Message)
            //{
            //    var connector = new ConnectorClient(new System.Uri(activity2.ServiceUrl));
            //    var isTyping = activity2.CreateReply("Nerdibot is thinking...");
            //    isTyping.Type = ActivityTypes.Typing;
            //    await connector.Conversations.ReplyToActivityAsync(isTyping);

            //    // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
            //    Thread.Sleep(2500);
            //}
           
           // context.Wait(MessageReceived);


        }
        public async Task CustomerPreferenceHandler(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Sure, that’s an amazing choice.");

            var activity = context.Activity as Activity;
            if (activity.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new System.Uri(activity.ServiceUrl));
                var isTyping = activity.CreateReply("Nerdibot is thinking...");
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);

                // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
                Thread.Sleep(2500);
            }
            await context.PostAsync($@"{Environment.NewLine}The below are the specifications:
                                    {Environment.NewLine}Color : Alabaster Silver Metallic, Bold Beige Metallic, Crystal Black Pearl, Deep Sapphire Blue, Habanero Red, Polished Metal Metallic, Tafeta White.
                                    {Environment.NewLine}No of Doors: 4,
                                    {Environment.NewLine}Function: Automatic,
                                    {Environment.NewLine}Displacement: 1497,
                                    {Environment.NewLine}Cylinders: 6");

            var activity2 = context.Activity as Activity;
            if (activity2.Type == ActivityTypes.Message)
            {
                var connector = new ConnectorClient(new System.Uri(activity2.ServiceUrl));
                var isTyping = activity2.CreateReply("Nerdibot is thinking...");
                isTyping.Type = ActivityTypes.Typing;
                await connector.Conversations.ReplyToActivityAsync(isTyping);

                // DEMO: I've added this for demonstration purposes, so we have time to see the "Is Typing" integration in the UI. Else the bot is too quick for us :)
                Thread.Sleep(2500);
            }

            PromptDialog.Confirm(
            context: context,
            resume: InteriorProcess,
            prompt: "Would you also like to see the interiors?",
            retry: "Sorry, I don't understand that.");
        }
        public async Task InteriorProcess(IDialogContext context, IAwaitable<bool> argument)
        {
            var answer = await argument;
            if (answer)
            {
                var reply = context.MakeMessage();

                reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                reply.Attachments = GetInteriorAttachments();

                await context.PostAsync(reply);

                //context.Wait(MessageReceived);
            }
            else
            {

            }
        }
        private static IList<Attachment> GetInteriorAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "",
                    "",
                    "",
                    new CardImage(url: "https://cdn.pixabay.com/photo/2017/06/06/20/56/car-2378419_960_720.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "", value: "")),
                GetHeroCard(
                     "",
                     "",
                    "",
                    new CardImage(url: "http://www.autoguide.com/blog/wp-content/uploads/2017/10/2017-Mazda-CX-5-5.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "", value: "")),
                GetHeroCard(
                     "",
                     "",
                    "",
                    new CardImage(url: "https://mrkustom.com/wp-content/uploads/2012/11/Custom-Car-Seats-Mr-Kustom-Chicago.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "", value: "")),

            };
        }
        private static IList<Attachment> GetCardsAttachments()
        {
            return new List<Attachment>()
            {
                GetHeroCard(
                    "Sedan",
                    "",
                    "While most compact sedans look a bit off with their deliberately-shortened side profiles, the Ford has managed to give the Figo Aspire a clean look. Being a Ford, the Figo Aspire offers a fun driving experience, especially the diesel variant.",
                    new CardImage(url: "https://www.drivespark.com/car-image/540x400x80/car/37642259-honda_city.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Read more", value: "http://www.gargash.ae/")),
                GetHeroCard(
                     "Coupe",
                     "",
                    "Ferrari has officially launched their brand new V12 powered GT in India - the 812 Superfast. The Ferrari 812 Superfast replaces the F12 Berlinetta, a model that was quite popular with Ferrari customers in the country.",
                    new CardImage(url: "https://auto.ndtvimg.com/car-images/medium/aston-martin/db11/aston-martin-db11.jpg?v=7"),
                    new CardAction(ActionTypes.OpenUrl, "Read more", value: "http://www.gargash.ae/")),
                GetHeroCard(
                     "Cabriolet",
                     "",
                    "Ferrari has officially launched their brand new V12 powered GT in India - the 812 Superfast. The Ferrari 812 Superfast replaces the F12 Berlinetta, a model that was quite popular with Ferrari customers in the country.",
                    new CardImage(url: "http://cdn1.carbuyer.co.uk/sites/carbuyer_d7/files/car_images/mercedes-c-class-conv_0.jpg"),
                    new CardAction(ActionTypes.OpenUrl, "Read more", value: "http://www.gargash.ae/")),

            };
        }
        private static Attachment GetHeroCard(string title, string subtitle, string text, CardImage cardImage, CardAction cardAction)
        {
            var heroCard = new HeroCard
            {
                Title = title,
                Subtitle = subtitle,
                Text = text,
                Images = new List<CardImage>() { cardImage },
                Buttons = new List<CardAction>() { cardAction },
            };

            return heroCard.ToAttachment();
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