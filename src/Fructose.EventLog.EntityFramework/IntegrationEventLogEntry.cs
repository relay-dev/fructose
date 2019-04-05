﻿using Fructose.Common.EventBus;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Fructose.EventLog.EntityFramework
{
    public class IntegrationEventLogEntry
    {
        private IntegrationEventLogEntry() { }

        public IntegrationEventLogEntry(IntegrationEvent integrationEvent)
        {
            EventId = integrationEvent.Id;
            CreationTime = integrationEvent.CreationDate;
            EventTypeName = integrationEvent.GetType().FullName;
            Content = JsonConvert.SerializeObject(integrationEvent);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
        }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }

        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();

        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }

        public IntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(Content, type) as IntegrationEvent;

            return this;
        }
    }
}
