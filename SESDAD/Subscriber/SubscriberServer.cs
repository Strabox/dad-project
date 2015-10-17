﻿using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubscriberServer : MarshalByRefObject, IGeneralControlServices,
        ISubscriberControlServices, ISubscriber
    {
        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;


        public SubscriberServer(string name, string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;

            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Subscriber specific methods.

        public void Receive(Message message)
        {
            //TODO if we need save messages or something like that.
            logServer.LogAction("SubEvent " + name+", " + "MissingPublisherName" 
                + ", " + message.Topic + ", " + "MissingEventNumber");
        }

        // General test and control methods.

        public void Subscribe(string topicName)
        {
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Subscribe(this as ISubscriber, topicName);
        }

        public void Unsubscribe(string topicName)
        {
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Unsubscribe(this as ISubscriber, topicName);
        }

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            //TODO
        }

        public void Status()
        {
            //TODO
        }

        public void Unfreeze()
        {
            //TODO
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

    }
}
