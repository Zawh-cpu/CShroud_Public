namespace CShroudApp.Infrastructure.VpnCores.SingBox.Config;

/*
 *"servers": [],
    "rules": [],
    "final": "",
    "strategy": "",
    "disable_cache": false,
    "disable_expire": false,
    "independent_cache": false,
    "cache_capacity": 0,
    "reverse_mapping": false,
    "client_subnet": "",
    "fakeip": {}
 * 
 */

public class DnsObject
{
    public List<Dictionary<string, object>> Servers { get; set; } = new();
    public List<Rule> Rules { get; set; } = new();
    public string? Final { get; set; }
    public string? Strategy  { get; set; }
    public bool? DisableCache { get; set; }
    public bool? DisableExpire { get; set; }
    public bool? IndependentCache { get; set; }
    public int? CacheCapacity { get; set; }
    public bool? ReverseMapping { get; set; }
    public string? ClientSubnet { get; set; }
    public Dictionary<string, object>? Fakeip { get; set; }


    public class Rule
    {
        public required string Server { get; set; }
        public List<string>? Domain { get; set; }
        public List<string>? RuleSet { get; set; }
    }
}