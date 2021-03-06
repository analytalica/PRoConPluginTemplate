//Import various C# things.
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

//Import Procon things.
using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Plugin.Commands;
using PRoCon.Core.Players;
using PRoCon.Core.Players.Items;
using PRoCon.Core.Battlemap;
using PRoCon.Core.Maps;
using PRoCon.Core.HttpServer;

namespace PRoConEvents
{
    public class Sample : PRoConPluginAPI, IPRoConPluginInterface
    {

        //--------------------------------------
        //Class level variables.
        //--------------------------------------

        private bool pluginEnabled = false;
        private String someString = "string";

        private int debugLevel = 1;

        //How does the Debug Level work?
        /*
         * The Debug Level lets you choose which console messages are
         * higher priority than others, and when they appear. The lower the number,
         * the higher the priority. It lets developers insert console output
         * to debug their plugins that can be later suppressed when deployed live.
         * 
         * By default there are 3 levels: 0, 1, and 2, though nothing prevents
         * you from going higher.
         * 
         * For example, if I ran the commands:
         * toConsole(1, "Plugin Enabled!");
         * toConsole(2, "Some Developer Message");
         * 
         * and I had the debug level set to 1, only "Plugin Enabled!" would appear
         * in the console. But if it was set to 2, both strings would appear.
         * If set to 0, neither message is that priority, so there is no console output.
         * (0 suppresses ALL console messages).
         * 
         * The debug level is optional, though strongly recommended.
         */

        //--------------------------------------
        //Plugin constructor. Can be left blank.
        //--------------------------------------

        public Sample()
        {

        }

        //--------------------------------------
        //Description settings for your plugin.
        //--------------------------------------

        public string GetPluginName()
        {
            return "Sample.cs : Plugin Template";
        }

        public string GetPluginVersion()
        {
            return "0.0.2";
        }

        public string GetPluginAuthor()
        {
            return "Analytalica";
        }

        public string GetPluginWebsite()
        {
            return "purebattlefield.org";
        }

        public string GetPluginDescription()
        {
            return @"A basic plugin template. Does nothing.";
        }

        //--------------------------------------
        //Helper Functions
        //--------------------------------------

        //Sends a message to chat.
        //There is a ~126 character limitation to the amount of characters you can use in one chat message.
        //You can split chat messages into multiple lines (multiple chats in-game) using "\n" (newline).

        public void toChat(String message)
        {
			toChat(message, "all");
            /*if (!message.Contains("\n") && !String.IsNullOrEmpty(message))
            {
                this.ExecuteCommand("procon.protected.send", "admin.say", message, "all");
            }
            else if(message != "\n")
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
					if(!String.IsNullOrEmpty(message))
                    toChat(send);
                }
            }*/
        }

        public void toChat(String message, String playerName)
        {
            if (!message.Contains("\n") && !String.IsNullOrEmpty(message))
            {	
				if(playerName == "all"){
					this.ExecuteCommand("procon.protected.send", "admin.say", message, "all");
				}else{
					this.ExecuteCommand("procon.protected.send", "admin.say", message, "player", playerName);
				}
            }
            else if(message != "\n")
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
					if(!String.IsNullOrEmpty(message))
                    toChat(send, playerName);
                }
            }
        }

        public void toConsole(int msgLevel, String message)
        {
            if (debugLevel >= msgLevel)
            {
                this.ExecuteCommand("procon.protected.pluginconsole.write", message);
            }
        }

        //--------------------------------------
        //These methods run when Procon does what's on the label.
        //--------------------------------------

        //Runs when the plugin is compiled.

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion)
        {
            // Depending on your plugin you will need different types of events. See other plugins for examples.
            this.RegisterEvents(this.GetType().Name, "OnPluginLoaded");
        }

        //Runs when the plugin is enabled (checked in Procon)
        //Note that this also runs right after a server restart if it was enabled before.

        public void OnPluginEnable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = true;
            //Say something in the console.
            this.toConsole(1, "Template Plugin Enabled!");
        }

        //Runs when the plugin is disabled (unchecked in Procon)

        public void OnPluginDisable()
        {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = false;
            //Say something in the console.
            this.toConsole(1, "Template Plugin Disabled!");
        }

        //List plugin variables.
        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Some String", typeof(string), this.someString));
            lstReturn.Add(new CPluginVariable("Settings|Debug Level", typeof(string), this.debugLevel.ToString()));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        //Set variables.
        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (strVariable.Contains("Some String"))
            {
                this.someString = strValue;
            }
            else if (strVariable.Contains("Debug Level"))
            {
                try
                {
                    this.debugLevel = Int32.Parse(strValue);
                }
                catch (Exception z)
                {
                    this.toConsole(1, "Invalid debug level! Choose 0, 1, or 2 only.");
                    this.debugLevel = 1;
                }
            }
        }
    }
}