using OasisAPI.Models;

namespace OasisAPI.Builders;

public interface IMyBuilder<in T>
{
    OasisMessage Build();
}