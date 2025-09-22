public class ProductSearchDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? ParentCategory { get; set; }
    public List<string> Attributes { get; set; } = new();
}
