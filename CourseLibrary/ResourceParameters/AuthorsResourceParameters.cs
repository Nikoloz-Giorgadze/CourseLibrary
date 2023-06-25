namespace CourseLibrary.ResourceParameters;

public class AuthorsResourceParameters
{
    const int maxPageSize = 20;
    public string? MainCategory { get; set; }
    public string? SearchQuery { get; set; }
    public int PageNumber { get; set; } = 1;
    public int _PageSize { get; set; } = 10;
    public int PageSize
    {
        get => _PageSize;
        set => _PageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}
