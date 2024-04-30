using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=WebhookEndpoint.db"));


var app = builder.Build();



app.UseHttpsRedirection();

app.MapPost("/webhook/test", (HttpContext context) =>
{
    var headers = context.Request.Headers;

    foreach (var (key, value) in headers)
    {
        Console.WriteLine($"{key}: {value}");
    }

    return Results.StatusCode(200);
});


app.MapPost("/webhook/auth", (IConfiguration config, HttpContext context) =>
{
    Console.WriteLine(config["auth-key"]);
    if (context.Request.Headers["auth-key"] != config["auth-key"])
    {
        return Results.StatusCode(401);
    }

    // Process webhook...

    return Results.StatusCode(200);
});


app.MapPost("/webhook/errors", (HttpContext httpContext) =>
{
    var httpStatus = httpContext.Request.Headers["http-response-code"];
    Console.WriteLine(httpStatus);

    try
    {
        var httpStatusInteger = int.Parse(httpStatus!);
        if (httpStatusInteger >= 100 && httpStatusInteger <= 599)
            return Results.StatusCode(httpStatusInteger);
        else
            return Results.StatusCode(400);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Header status code not an integer: {ex.Message} ");
        return Results.StatusCode(400);
    }

});


app.MapPost("/webhook/hmac", async (HttpContext httpContext, AppDbContext dbContext, IConfiguration config) =>
{
    var hmacHash = httpContext.Request.Headers["Marketplacer-Hmac-256"];
    //Console.WriteLine($"--> Received Hash: {hmacHash}");

    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();

    string generatedHash = HmacHash.GenerateHmacHash(config["hmac-secret"]!, body);

    if (generatedHash != hmacHash)
    {
        Console.WriteLine("HMAC hashes do not match");    
        return Results.StatusCode(412);
    }
    
    Console.WriteLine("HMAC hash validated");
    return Results.Ok("HMAC hash validated");
    
});


app.MapPost("/webhook/sequencing", async (HttpContext httpContext, AppDbContext dbContext) =>
{
    if (!httpContext.Request.HasJsonContentType())
    {
        return Results.StatusCode(415);
    }

    Console.WriteLine(httpContext.Request.Headers["Marketplacer-Sequence"]);
    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();
    var jsonBody = JsonConvert.DeserializeObject<dynamic>(body);


    var webhookEvent = new WebhookEvent
    {
        WebhookId = jsonBody!.id,
        Sequence = int.Parse(httpContext.Request.Headers["Marketplacer-Sequence"]!),
        WebhookPayload = body
    };

    await dbContext.WebhookEvents.AddAsync(webhookEvent);
    await dbContext.SaveChangesAsync();



    //httpContext.Response.StatusCode = StatusCodes.Status200OK;
    return Results.StatusCode(200);
});


app.MapPost("/webhook/payload-parse", async (HttpContext httpContext, AppDbContext dbContext) =>
{
    if (!httpContext.Request.HasJsonContentType())
    {
        return Results.StatusCode(415);
    }

    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();
    var jsonBody = JsonConvert.DeserializeObject<dynamic>(body);


    var webhookEvent = new WebhookEvent
    {
        WebhookId = jsonBody!.id,
        Sequence = int.Parse(httpContext.Request.Headers["Marketplacer-Sequence"]!),
        WebhookPayload = body,
        WebhookEventType = jsonBody!.event_name,
        WebhookObjectType = jsonBody!.payload.data.node.__typename,
        WebhookObjectId = jsonBody!.payload.data.node.id
    };

    await dbContext.WebhookEvents.AddAsync(webhookEvent);
    await dbContext.SaveChangesAsync();



    //httpContext.Response.StatusCode = StatusCodes.Status200OK;
    return Results.StatusCode(200);

});


app.MapPost("/webhook/payload-diff", async (HttpContext httpContext, AppDbContext dbContext) =>
{
    if (!httpContext.Request.HasJsonContentType())
    {
        return Results.StatusCode(415);
    }

    using var reader = new StreamReader(httpContext.Request.Body);
    var body = await reader.ReadToEndAsync();
    var jsonBody = JsonConvert.DeserializeObject<dynamic>(body);

    string webhookObjectId = jsonBody!.payload.data.node.id;

    Console.WriteLine($"Webhook event for object ID: {webhookObjectId} ");


    var lastEventRecordForObject = await dbContext.WebhookEvents
        .Where(row => row.WebhookObjectId == webhookObjectId)
        .OrderByDescending(row => row.Sequence)
        .FirstOrDefaultAsync();

    var webhookEvent = new WebhookEvent
    {
        WebhookId = jsonBody!.id,
        Sequence = int.Parse(httpContext.Request.Headers["Marketplacer-Sequence"]!),
        WebhookPayload = body,
        WebhookEventType = jsonBody!.event_name,
        WebhookObjectType = jsonBody!.payload.data.node.__typename,
        WebhookObjectId = jsonBody!.payload.data.node.id
    };

    await dbContext.WebhookEvents.AddAsync(webhookEvent);
    await dbContext.SaveChangesAsync();

    if (lastEventRecordForObject == null)
    {
        Console.WriteLine("Nothing to diff on");
        return Results.StatusCode(204);
    }

    var lastEventPayload = JsonConvert.DeserializeObject<dynamic>(lastEventRecordForObject.WebhookPayload);

    JToken diff = JsonDiff.ReturnTheDifference(jsonBody, lastEventPayload!);

    if (diff != null)
    {
        Console.WriteLine(JsonConvert.SerializeObject(diff));
        return Results.Ok("There was a difference");
    }
    else
    {
        return Results.StatusCode(204);
    }


});

app.Run();


