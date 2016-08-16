Experiment surrounding CQRS and Event Sourcing

# Overview

This is a very simple (and probably naive) implementation of [CQRS](https://en.m.wikipedia.org/wiki/Command–query_separation) and [Event Sourcing](http://martinfowler.com/eaaDev/EventSourcing.html) used to create a very simple invoicing system. 

## Commands

Commands are used to perform an action within the invoicing system, such as creating an invoice, adding new line items, applying a payment, and pretty much anything else that isn't pure data retrieval. Commands themselves don't actually do anything, instead they generate a stream of events that models what should happen when the command is handled.
