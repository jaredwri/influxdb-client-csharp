
I wanted to thank you in advance for taking the time to look this over.  I understand your current api serves a much broader audience and i'm not asking for anything to change.  I only ask that you consider the simple scenario as well as an additive library for this use case.   I'd be willing to help if you're open to the idea.  


## Goals
- To create and demonstrate my idea of a simple async write api for microservice/iot measurement consumers using .net core libraries.
- to use DI of choice.  
- Keep it simple (KISS) for my use case knowing this example will not cover all use cases. 

What this is **NOT**

- I'm *not* proposing the pull request be accepted.  It's just a way for you to better understand my use case and concerns.  I expect rejection. 
- I'm *not* proposing anything change in terms of style.  If you disagree with a way that I've mocked something together I totally understand it's not your preference.  I'm just asking that you consider the intent vs the implementation. 
- It's *not* production quality code.  
- It does *not* maintain feature parity.  I removed functionality or methods in order to better demonstrate.   

Considerations:
- Trying to target .net core 3.X applications/services.
- Using standard DI to allow for easier internal changes and/or refactorings  
- Allow the consumer to have full control of the operations they need to utilize.
- Added interfaces to allow mockability of the library in consumer logic .
- Tried to account for current features such as Jitter and automatic retry-after logic using Polly
- Use .net framework standard http client.  
>I actually love restsharp but I've been burned by it on it having a dependency chain reliance in some legacy applications that would cause a `System.MissingMethodException` to occur due to a dependency mismatch at runtime at the time of execution.
 
 ### DI
I usually use autofac, but I can accomplish what I want by providing the service collection to autofac or whatever provider I choose.  There is a self contained container/serviceprovider/scope at the `InfluxDBClient`  level that is not exposed to the `Startup` service collection.   
 
 #### Structure
I copied existing classes into 1 of 2 new projects for demonstration purposes.  For the write side of things their really wasn't much that was required.  
- `Client.Core.Common` contains what would be considered models/exceptions and or contracts/interfaces.   I tried to keep the dependency to as little as possible with the assumption that `NodaTime` and `Newtonsoft.Json` are de facto . 
 
 - `Client.Core.Proposal` should contain client related configuration object, exceptions and communication related classes.
 
- `Client.Core.Proposal.UseCase` is a sample web api project that demonstrates a basic micro-service use case.  Invoke some api resource, do some processing and capture a measurement.  The configuration is in `appsettings.development.json`, `Startup.cs`.  The `Example` controller  contains the rest.

