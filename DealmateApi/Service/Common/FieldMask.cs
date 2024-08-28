using System.Collections.Immutable;

namespace DealmateApi.Service.Common;

public sealed class FieldMask : IEquatable<FieldMask>
{
    private readonly ImmutableArray<string> paths;
    public ICollection<string> Paths => this.paths;

    public FieldMask(params string[] paths) : this((IEnumerable<string>)paths) { }

    private FieldMask(ImmutableArray<string> paths)
    {
        this.paths = paths; 
    }

    public FieldMask()
    {
        this.paths = ImmutableArray<string>.Empty;
    }

    public FieldMask(IEnumerable<string> paths)
    {
        this.paths = paths.OrderBy(s => s).ToImmutableArray();
    }
    public bool Contains(string path) => this.paths.BinarySearch(path) >= 0;
    public FieldMask WithAdditionalPaths(params string[] paths) => new(this.Paths.Concat(paths));
    public static FieldMask Of(params string[] paths) => new(paths);
    public static FieldMask Of(IEnumerable<string> paths) => new(paths);

    public bool Equals(FieldMask? other)
    {
        return other != null && this.paths.SequenceEqual(other.paths);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is FieldMask other && this.Equals(other);
    }

    public override int GetHashCode()
    {
        return this.Paths.GetHashCode();
    }

    public override string ToString()
    {
        return string.Join(',', this.paths);
    }
    private static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpper(input[0]) + input.Substring(1);
    }
    public static FieldMask CreateFieldMask(string values)
    {
        if (string.IsNullOrWhiteSpace(values))
        {
            throw new Exception("Invalid Fields");
        }

        var paths = values.Split(',')
            .Select(path => path.Trim()) // Remove any extra whitespace from each path
            .Where(path => !string.IsNullOrEmpty(path)) // Exclude any empty paths
            .Select(path => CapitalizeFirstLetter(path))
            .ToArray();

        return new FieldMask(paths);
    }
}
