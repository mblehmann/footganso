public class Team
{
    public string Name { get; }

    public Team(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}