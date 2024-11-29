using CyberCareServices.ViewModels;

public class ComponentsViewModel
{
    public List<ComponentViewModel> Components { get; set; }
    public PageViewModel PageViewModel { get; set; }
    public string SortField { get; set; }
    public string SortOrder { get; set; }
    public string SearchQuery { get; set; }
}
