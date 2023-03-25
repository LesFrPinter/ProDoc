namespace ProDocEstimate.ViewModels
{
    internal class LookupViewModel : ViewModelBase
    {
        private string? custName; public string? CustName { get => custName; set => SetProperty(ref custName, value); }
    }
}