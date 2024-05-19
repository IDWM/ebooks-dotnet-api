using System.ComponentModel.DataAnnotations;

namespace ebooks_dotnet8_api.DTOs;

/// <summary>
/// Data transfer object for updating an eBook.
/// </summary>
public class UpdateEBookDto
{
    /// <summary>
    /// Title of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public string? Title { get; set; }

    /// <summary>
    /// Author of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public string? Author { get; set; }

    /// <summary>
    /// Genre of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public string? Genre { get; set; }

    /// <summary>
    /// Format of the eBook.
    /// </summary>
    [StringLength(256, MinimumLength = 3)]
    public string? Format { get; set; }

    /// <summary>
    /// Price of the eBook.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? Price { get; set; }
}
