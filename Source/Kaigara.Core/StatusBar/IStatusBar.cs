namespace Kaigara.StatusBar;
public interface IStatusBar
{
    string? Status { get; }
    double? Progress { get; }
    void Ready();
    void UpdateStatus(string? status, double? progress = null);
}
