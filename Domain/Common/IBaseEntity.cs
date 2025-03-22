using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common;

public interface IBaseEntity
{
    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}
