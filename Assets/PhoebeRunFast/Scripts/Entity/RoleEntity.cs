using QMVC;

public class RoleEntity : IEntity
{
    public RoleData roleData;

	public IArchitecture GetArchitecture()
	{
		return PhoebeRunFast.Interface;
	}
}