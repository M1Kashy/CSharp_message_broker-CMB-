using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Services;

public class MessageService
{
    ConcurrentDictionary<Guid, ConcurrentQueue<string>>  Queues = new();

    ILogger<MessageService> logger {get; set;}
    IConfiguration config {get; set;}
    public MessageService(
        ILogger<MessageService> logger,
        IConfiguration config
    )
    {
        this.logger = logger;
        this.config = config;
        initialization();
    }

    Task initialization()
    {
        var queues = config.GetSection("Queues")
                            .GetChildren()
                            .Select(qs => new Guid(qs.Value!))
                            .ToList();
        Console.WriteLine($"\t {queues}");
        if (queues is null || queues.Count == 0)
            throw new Exception("Queue is empty!");

        foreach(var q in queues)
        {
            var isOk = this.Queues.TryAdd(q, new ConcurrentQueue<string>());

            if (!isOk)
                throw new Exception("Some wrong in Queues");
            else
            {
                logger.LogInformation($"Created queue: {q} at {DateTime.Now}");
            }
        }
        return Task.CompletedTask;
    }

    public async Task<string?> Get(Guid guid)
    {
        var isOk = this.Queues.TryGetValue(guid, out var msgs);

        if (!isOk)
            return null;

        isOk = msgs!.TryDequeue(out var result);

        if (!isOk)
            return null;

        return result!;
    }

    public async Task<bool> Add(Message message)
    {
         var isOk = this.Queues.TryGetValue(message.id, out var msgs);

        if (!isOk)
            return false;

        msgs!.Enqueue(message.msg);

       return true;
    }
}