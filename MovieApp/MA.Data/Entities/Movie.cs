using System.Runtime.InteropServices.JavaScript;

namespace MA.Data.Entities;

public class Movie : IBaseEntity
{
    public string Title { get; set; }
    public string Category { get; set; }
    public string Director { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public double Rating { get; set; }
    public string AboutMovie { get; set; }
    public List<string> Cast { get; set; }
    public double MovieLength { get; set; }

    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}