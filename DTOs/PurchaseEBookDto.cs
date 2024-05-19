using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet8_api.DTOs;

/// <summary>
/// Data transfer object for purchasing an eBook.
/// </summary>
public class PurchaseEBookDto
{
    /// <summary>
    /// ID of the eBook to purchase.
    /// </summary>
    public required int EBookId { get; set; }

    /// <summary>
    /// Quantity of the eBook to purchase.
    /// </summary>
    [Range(1, int.MaxValue)]
    public required int Quantity { get; set; }

    /// <summary>
    /// Total price to pay for the purchase.
    /// </summary>
    [Range(1, int.MaxValue)]
    public required int TotalPrice { get; set; }
}
