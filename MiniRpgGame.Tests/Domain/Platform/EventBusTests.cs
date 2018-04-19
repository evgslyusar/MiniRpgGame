﻿using System.Collections.Generic;
using FluentAssertions;
using MiniRpgGame.Domain.Platform;
using Xunit;

namespace Game.Tests.Domain.Platform
{
   public class EventBusTests
    {
        private readonly IEventBus eventBus;

        public EventBusTests()
        {
            eventBus = new EventBus();
        }

        [Fact]
        public void Should_be_able_to_get_subscribed_to_events()
        {
            Event1 capturedEvent = null;

            eventBus.Subscribe<Event1>(@event => capturedEvent = @event);

            var publishedEvent = new Event1
                {
                    Text = "Hello World"
                };

            eventBus.Publish(publishedEvent);

            capturedEvent.Should().NotBeNull();
            capturedEvent.Should().Be(publishedEvent);
        }

        [Fact]
        public void Should_not_get_events_not_subscribed_to()
        {
            Event1 capturedEvent = null;

            eventBus.Subscribe<Event1>(@event => capturedEvent = @event);

            var publishedEvent = new Event2
            {
                Text = "Hello World"
            };

            eventBus.Publish(publishedEvent);

            capturedEvent.Should().BeNull();
        }

        [Fact]
        public void Should_be_able_to_cancel_an_event()
        {
            var stringsPublished = new List<string>();

            var cancelSubscription = eventBus.Subscribe<string>(stringsPublished.Add);

            eventBus.Publish("Before cancellation");

            cancelSubscription();

            eventBus.Publish("Hello World");

            stringsPublished.Should().Contain("Before cancellation");
        }

        [Fact]
        public void Should_handle_resubscription_from_handler()
        {
            Event1 eventFromSubscription = null;

            eventBus.Subscribe<Event1>(@event =>
                {
                    eventFromSubscription = @event;
                    eventBus.Subscribe<Event1>(x => { });
                });

            var publishedEvent1 = new Event1
            {
                Text = "Hello World"
            };

            eventBus.Publish(publishedEvent1);

            eventFromSubscription.Should().NotBeNull();
        }

        [Fact]
        public void Should_handle_cancelation_from_handler()
        {
            Event1 eventFromSubscription = null;

            CancelSubscription cancelEvent = null;

            cancelEvent = eventBus.Subscribe<Event1>(@event =>
            {
                cancelEvent();
                eventFromSubscription = @event;
            });

            var publishedEvent1 = new Event1
            {
                Text = "Hello World"
            };

            eventBus.Publish(publishedEvent1);

            eventFromSubscription.Should().NotBeNull();
        }

        private class Event1
        {
            public string Text { get; set; }
        }

        private class Event2
        {
            public string Text { get; set; }
        }
    }
}