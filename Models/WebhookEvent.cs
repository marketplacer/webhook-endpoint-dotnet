using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents an individual webhook event emitted by Marketplacer
/// </summary>
public class WebhookEvent
{
    /// <summary>
    /// Database Primary Key for table row. Also used for "received sequence" ordering 
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The Marketplacer webhook sequence id. This is the order in which
    /// webhooks were generated at source
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// This is the unique Id for the Marketplacer webhook event
    /// </summary>
    public required string WebhookId { get; set; }

    /// <summary>
    /// This is the full serialzed webhook payload
    /// </summary>
    public required string WebhookPayload { get; set; }

    /// <summary>
    /// Represents the type of event, most usually: Create, Update or Destroy
    /// </summary>
    public string? WebhookEventType { get; set; }

    /// <summary>
    /// The object or domain entity that the event relates to, e.g. Advert, Seller, Invoice etc.
    /// </summary>
    public string? WebhookObjectType { get; set; }

    /// <summary>
    /// The unique (Marketplacer) Id of the object that the event relates to
    /// </summary>
    public string? WebhookObjectId { get; set; }

    /// <summary>
    /// The time the webhhook event was created / received locally
    /// </summary>
    public string? CreatedAt { get; set; }
}