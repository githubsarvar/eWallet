namespace eWallet.Infrastructure;

public abstract class BaseEntity: IEquatable<BaseEntity>
{        
    public  int Id { get; private set; }   

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is BaseEntity other))
        {
            return false;
        }

        return Equals(other);
    }

    public bool Equals(BaseEntity other) => (other == null) ? false : (Id == other.Id);
    public static bool operator ==(BaseEntity first, BaseEntity secound) => first is not null && secound is not null && first.Equals(secound);
    public static bool operator !=(BaseEntity first, BaseEntity secound) =>!(first == secound);
    public override int GetHashCode() => Id.GetHashCode();
}
