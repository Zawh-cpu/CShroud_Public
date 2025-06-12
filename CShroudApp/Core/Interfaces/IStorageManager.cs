namespace CShroudApp.Core.Interfaces;

public interface IStorageManager
{
    public string? RefreshToken { get; set; }
    public TEntity? GetValue<TEntity>(string key) where TEntity : class;
    public Task SetValue(string key, object data, bool saveChanges = true);
    public Task DelValue(string key, bool saveChanges = true);
}